import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, forkJoin } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { CreditMemoService } from '../../../shared/services/credit-memo.service';
import { OrderService } from '../../../shared/services/order.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { CreditMemo } from '../../../shared/models/credit-memo';
import { CreditMemoLineItem } from '../../../shared/models/credit-memo-line-item';
import { Customer } from '../../../shared/models/customer';
import { OrderLineItem } from '../../../shared/models/order-line-item';

@Component({
  selector: 'app-credit-memo-form',
  templateUrl: './credit-memo-form.component.html',
  styleUrls: ['./credit-memo-form.component.css']
})
export class CreditMemoFormComponent implements AfterContentInit {
  creditMemo: CreditMemo = new CreditMemo();
  customers: Customer[];
  orderItems: OrderLineItem[];

  constructor(
    private creditMemoService: CreditMemoService,
    private orderService: OrderService,
    private customerService: CustomerService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.fetchLists();
    setTimeout(() => this.loadCreditMemo(), 3000);
  };

  saveCreditMemo(creditMemoForm: NgForm) {
    if (creditMemoForm.valid) {
      this.creditMemoService
        .saveCreditMemo(this.creditMemo)
        .subscribe(() => {
          if (this.creditMemo.id == 0) {
            this.loadCreditMemo();
          }
          this.util.showSuccessMessage("Credit memo saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  loadCreditMemo() {
    this.route.paramMap.subscribe(params => {
      var routeParam = params.get("id");
      var id = parseInt(routeParam);

      if (id == 0) {
        this.creditMemo = new CreditMemo();
      } else {
        this.creditMemoService
          .getCreditMemoById(id)
          .subscribe(creditMemo => {
            //this.creditMemo = new CreditMemo(creditMemo);
            //this.creditMemo.lineItems = creditMemo.lineItems.map(lineItem => {
            //  var creditMemoLineItem = new CreditMemoLineItem(lineItem);
            //  creditMemoLineItem.selectedItem = this.filterItems(lineItem.itemName)[0];
            //  return creditMemoLineItem;
            //});
            //this.creditMemo.selectedCustomer = this.filterCustomers(creditMemo.customerName)[0];
          });
      }
    });
  };

  fetchLists() {
    this.customerService
      .getCustomerLookup()
      .subscribe(data => this.customers = data);
  };

  customerSelected(customer: Customer) {
    if (customer && customer.id) {
      this.orderService
        .getOrderLineItemLookup(customer.id)
        .subscribe(data => this.orderItems = data);
    }
  }

  // Line items
  addLineItem() {
    this.creditMemo.lineItems.push(new CreditMemoLineItem());
  };

  removeLineItem(index: number) {
    if (confirm('Are you sure you want to remove this item?')) {
      this.creditMemo.lineItems.splice(index, 1);
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
      map(term => this.filterItems(term))
    );

  customerFormatter = (x: { name: string }) => x.name;
  itemFormatter = (x: { itemName: string, orderCode: string }) => `${x.itemName} - Order # ${x.orderCode}`;

  private filterCustomers(value: string): Customer[] {
    const filterValue = value.toLowerCase();

    return this.customers.filter(customer => customer.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }

  private filterItems(value: string): OrderLineItem[] {
    const filterValue = value.toLowerCase();

    return this.orderItems.filter(item => item.itemName.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }
}
