import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, forkJoin } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
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
export class InvoiceFormComponent implements AfterContentInit {
  invoice: Invoice = new Invoice();
  customers: Customer[];
  orders: Order[];
  creditMemos: CreditMemo[];

  selectedCustomer: Customer;

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
    this.fetchCustomers();
    //setTimeout(() => this.loadInvoice(), 3000);
  };

  saveInvoice(invoiceForm: NgForm) {
    if (invoiceForm.valid) {
      this.invoiceService
        .saveInvoice(this.invoice)
        .subscribe(() => {
          if (this.invoice.id == 0) {
            this.loadInvoice();
          }
          this.util.showSuccessMessage("Invoice saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  loadInvoice() {
    //this.route.paramMap.subscribe(params => {
    //  var routeParam = params.get("id");
    //  var id = parseInt(routeParam);

    //  if (id == 0) {
    //    this.invoice = new Invoice();
    //  } else {
    //    this.invoiceService
    //      .getInvoiceById(id)
    //      .subscribe(invoice => {
    //        this.invoice = new Invoice(invoice);
    //        this.invoice.lineItems = invoice.lineItems.map(lineItem => {
    //          var invoiceLineItem = new LineItem(lineItem);
    //          invoiceLineItem.selectedItem = this.filterItems(lineItem.itemName)[0];
    //          return invoiceLineItem;
    //        });
    //        //this.invoice.selectedCustomer = this.filterCustomers(invoice.customerName)[0];
    //      });
    //  }
    //});
  };

  fetchCustomers() {
    this.customerService
      .getCustomerLookup()
      .subscribe(customers => this.customers = customers);
  };

  customerSelected(customer: Customer) {
    if (customer && customer.id && customer.id != 0) {
      forkJoin(
        this.orderService.getOrderLookup(customer.id),
        this.creditMemoService.getCreditMemoLookup(customer.id)
      ).subscribe(([orderResponse, creditMemoResponse]) => {
        var orders = orderResponse.map(val => {
          var item = new InvoiceLineItem();
          item.code = val.code;
          item.orderId = val.id;
          item.totalAmount = val.totalAmount;
          item.date = val.date;
          item.type = InvoiceLineItemType.Order;
          return item;
        });
        console.log(orders);
        var creditMemos = creditMemoResponse.map(val => {
          var item = new InvoiceLineItem();
          item.creditMemoId = val.id;
          item.code = val.code;
          item.date = val.date;
          item.totalAmount = val.totalAmount;
          item.type = InvoiceLineItemType.CreditMemo;
          return item;
        });
        console.log(creditMemos);

        this.invoice.lineItems = orders.concat(creditMemos);
      });
    }
  };

  // Line items
  addLineItem() {
    this.invoice.lineItems.push(new InvoiceLineItem());
  };

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
      map(term => term.length < 2 ? [] : this.filterCustomers(term))
    );

  searchItem = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => this.filterOrders(term))
    );

  customerFormatter = (x: { name: string }) => x.name;
  itemFormatter = (x: { name: string }) => x.name;

  private filterCustomers(value: string): Customer[] {
    const filterValue = value.toLowerCase();

    return this.customers.filter(customer => customer.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }

  private filterOrders(value: string): Order[] {
    const filterValue = value.toLowerCase();

    return this.orders.filter(order => order.code.toString().startsWith(filterValue)).splice(0, 10);
  }
}
