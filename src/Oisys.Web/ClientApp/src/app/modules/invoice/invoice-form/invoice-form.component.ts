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
      this.isSaving = true;
      this.saveInvoiceSub = this.invoiceService
        .saveInvoice(this.invoice)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    if (this.invoice.id == 0) {
      this.setInvoice(undefined);
    }
    this.util.showSuccessMessage('Invoice saved successfully.');
  };

  saveFailed = (error) => {
    this.util.showErrorMessage('An error occurred while saving. Please try again.');
    this.isSaving = false;
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  customerSelected(customer: Customer) {
    if (this.invoice.customer && this.invoice.customer.id && this.invoice.customer.id != 0) {
      this.fetchItemsSub = forkJoin(
        this.orderService.getOrderLookup(customer.id),
        this.creditMemoService.getCreditMemoLookup(customer.id)
      ).subscribe(([orderResponse, creditMemoResponse]) => {
        var orders = orderResponse.map(val => this.createInvoiceLineItem(val, InvoiceLineItemType.Order));
        var creditMemos = creditMemoResponse.map(val => this.createInvoiceLineItem(val, InvoiceLineItemType.CreditMemo));
        this.invoice.lineItems = orders.concat(creditMemos);
      });
    }
  };

  createInvoiceLineItem(value: any, type: InvoiceLineItemType): InvoiceLineItem {
    var lineItem = new InvoiceLineItem();
    lineItem.code = value && value.code || 0;
    lineItem.date = value && value.date || undefined;
    lineItem.totalAmount = value && value.totalAmount || 0;
    lineItem.type = type;
    lineItem.orderId = type == InvoiceLineItemType.Order ? (value && value.id || 0) : 0
    lineItem.creditMemoId = type == InvoiceLineItemType.CreditMemo ? (value && value.id || 0) : 0;
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
