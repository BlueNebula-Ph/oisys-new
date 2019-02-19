import { Component, AfterContentInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { OrderService } from '../../../shared/services/order.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Order } from '../../../shared/models/order';
import { LineItem } from '../../../shared/models/line-item';

@Component({
  selector: 'app-order-form',
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.css']
})
export class OrderFormComponent implements AfterContentInit, OnDestroy {
  order = new Order();
  getOrderSub: Subscription;
  saveOrderSub: Subscription;
  isSaving = false;

  @ViewChild('customer') customerField: ElementRef;

  constructor(
    private orderService: OrderService,
    private customerService: CustomerService,
    private inventoryService: InventoryService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.loadOrderForm();
  };

  ngOnDestroy() {
    if (this.getOrderSub) { this.getOrderSub.unsubscribe(); }
    if (this.saveOrderSub) { this.saveOrderSub.unsubscribe(); }
  }

  loadOrderForm() {
    const orderId = +this.route.snapshot.paramMap.get('id');
    if (orderId && orderId != 0) {
      this.loadOrder(orderId);
    } else {
      this.setOrder(undefined);
    }
  };

  setOrder(order: any) {
    this.order = order ? new Order(order) : new Order();
    this.customerField.nativeElement.focus();
  };

  loadOrder(id: number) {
    this.getOrderSub = this.orderService
      .getOrderById(id)
      .subscribe(order => this.setOrder(order));
  };

  saveOrder(orderForm: NgForm) {
    this.isSaving = true;
    if (orderForm.valid) {
      this.saveOrderSub = this.orderService
        .saveOrder(this.order)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    if (this.order.id == 0) {
      this.setOrder(undefined);
    }
    this.util.showSuccessMessage("Order saved successfully.");
  };

  saveFailed = (error) => {
    this.util.showErrorMessage("An error occurred while saving. Please try again.");
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  // Line items
  addLineItem() {
    if (this.order && this.order.lineItems) {
      this.order.lineItems.push(new LineItem());
      this.order.updateLineItems();
    }
  };

  removeLineItem(index: number) {
    if (confirm('Are you sure you want to remove this item?')) {
      this.order.lineItems.splice(index, 1);
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

  searchItem = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? [] :
        this.inventoryService.getItemLookup(term)
          .pipe(
            map(items => items.splice(0, 10))
          )
      )
    );

  customerFormatter = (x: { name: string }) => x.name;
  itemFormatter = (x: { name: string }) => x.name;
}
