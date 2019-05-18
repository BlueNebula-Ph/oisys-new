import { Component, AfterContentInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, forkJoin, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { InvoiceService } from '../../../shared/services/invoice.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { OrderService } from '../../../shared/services/order.service';
import { CreditMemoService } from '../../../shared/services/credit-memo.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Invoice } from '../../../shared/models/invoice';
import { Customer } from '../../../shared/models/customer';
import { Order } from '../../../shared/models/order';
import { CreditMemo } from '../../../shared/models/credit-memo';
import { InvoiceLineItem } from '../../../shared/models/invoice-line-item';
import { InvoiceLineItemType } from '../../../shared/models/invoice-line-item-type';

@Component({
  selector: 'app-invoice-form',
  templateUrl: './invoice-form.component.html',
  styleUrls: ['./invoice-form.component.css']
})
export class InvoiceFormComponent implements AfterContentInit, OnDestroy {
  invoice: Invoice = new Invoice();

  orders = new Array<Order>();
  creditMemos = new Array<CreditMemo>();

  getInvoiceSub: Subscription;
  saveInvoiceSub: Subscription;
  fetchItemsSub: Subscription;

  isSaving = false;

  @ViewChild('customer') customerField: ElementRef;

  get isCustomerDisabled() {
    return this.invoice.id && this.invoice.id != 0;
  }

  constructor(
    private invoiceService: InvoiceService,
    private customerService: CustomerService,
    private orderService: OrderService,
    private creditMemoService: CreditMemoService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.loadInvoiceForm();
  };

  ngOnDestroy() {
    if (this.getInvoiceSub) { this.getInvoiceSub.unsubscribe(); }
    if (this.saveInvoiceSub) { this.saveInvoiceSub.unsubscribe(); }
    if (this.fetchItemsSub) { this.fetchItemsSub.unsubscribe(); }
  };

  loadInvoiceForm() {
    const invoiceId = +this.route.snapshot.paramMap.get('id');
    if (invoiceId && invoiceId != 0) {
      this.loadInvoice(invoiceId);
    } else {
      this.setInvoice(undefined);
    }
  };

  setInvoice(invoice: any) {
    this.invoice = invoice ? new Invoice(invoice) : new Invoice();
    this.customerField.nativeElement.focus();
  };

  loadInvoice(id: number) {
    this.getInvoiceSub = this.invoiceService
      .getInvoiceById(id)
      .subscribe(invoice => this.setInvoice(invoice));
  };

  saveInvoice(invoiceForm: NgForm) {
    if (invoiceForm.valid) {
      if (this.invoice.totalAmount < 0) {
        if (!confirm('Total amount for this invoice is less than zero. Are you sure you want to continue?')) {
          return;
        }
      }

      this.isSaving = true;
      this.saveInvoiceSub = this.invoiceService
        .saveInvoice(this.invoice)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    this.loadInvoiceForm();
    this.util.showSuccessMessage('Invoice saved successfully.');
  };

  saveFailed = (error) => {
    this.isSaving = false;
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  customerSelected(customer: Customer) {
    if (customer && customer.id) {
      this.loadOrdersAndCreditMemos(customer.id, true);
    }
  };

  loadOrdersAndCreditMemos(id: number, isNew: boolean) {
    this.fetchItemsSub = forkJoin(
      this.orderService.getOrderLookup(id),
      this.creditMemoService.getCreditMemoLookup(id)
    ).subscribe(([orderResponse, creditMemoResponse]) => {
      let orders = orderResponse.map(val => this.createInvoiceLineItem(val, InvoiceLineItemType.Order));
      let creditMemos = creditMemoResponse.map(val => this.createInvoiceLineItem(val, InvoiceLineItemType.CreditMemo));

      if (isNew) {
        this.invoice.lineItems = orders.concat(creditMemos);
      } else {
        let newItems = orders.concat(creditMemos);
        newItems.forEach((val) => {
          let itemInList = this.invoice.lineItems.find(x => x.creditMemoId == val.creditMemoId && x.orderId == val.orderId);
          if (!itemInList) {
            this.invoice.lineItems.push(val);
          }
        });
      }
    });
  };

  createInvoiceLineItem(value: any, type: InvoiceLineItemType): InvoiceLineItem {
    var lineItem = new InvoiceLineItem();
    lineItem.code = value && value.code || 0;
    lineItem.date = value && value.date || undefined;
    lineItem.totalAmount = value && value.totalAmount || 0;
    lineItem.type = type;
    lineItem.orderId = type == InvoiceLineItemType.Order ? (value && value.id || null) : null
    lineItem.creditMemoId = type == InvoiceLineItemType.CreditMemo ? (value && value.id || null) : null;
    return lineItem;
  };

  // Line items
  removeLineItem(index: number) {
    if (confirm('Are you sure you want to remove this item?')) {
      this.invoice.lineItems.splice(index, 1);
    }
  };

  // Autocomplete
  searchCustomer = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? [] :
        this.customerService.getCustomerLookup(0, 0, term)
          .pipe(
            map(customers => customers.splice(0, 10))
          )
      )
    );

  customerFormatter = (x: { name: string }) => x.name;
}
