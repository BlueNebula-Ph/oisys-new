import { Component, AfterContentInit, ElementRef, ViewChild, OnDestroy } from '@angular/core';

import { ActivatedRoute, Event } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { DeliveryService } from '../../../shared/services/delivery.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { OrderService } from '../../../shared/services/order.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Delivery } from '../../../shared/models/delivery';
import { DeliveryLineItem } from '../../../shared/models/delivery-line-item';
import { Customer } from '../../../shared/models/customer';
import { City } from '../../../shared/models/city';

@Component({
  selector: 'app-delivery-form',
  templateUrl: './delivery-form.component.html',
  styleUrls: ['./delivery-form.component.css']
})
export class DeliveryFormComponent implements AfterContentInit, OnDestroy {
  delivery: Delivery = new Delivery();

  customersList = new Array<Customer>();
  selectedCustomer: Customer;

  getDeliverySub: Subscription;
  getOrderLineItemSub: Subscription;
  saveDeliverySub: Subscription;

  isSaving = false;

  @ViewChild('province') provinceField: ElementRef;
  @ViewChild('customer') customerField: ElementRef;

  constructor(
    private deliveryService: DeliveryService,
    private provinceService: ProvinceService,
    private customerService: CustomerService,
    private orderService: OrderService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.loadDeliveryForm();
  };

  ngOnDestroy() {
    if (this.getDeliverySub) { this.getDeliverySub.unsubscribe(); }
    if (this.saveDeliverySub) { this.saveDeliverySub.unsubscribe(); }
  };

  loadDeliveryForm() {
    const deliveryId = +this.route.snapshot.paramMap.get('id');
    if (deliveryId && deliveryId != 0) {
      this.loadDelivery(deliveryId);
    } else {
      this.setDelivery(undefined);
    }
  };

  setDelivery(delivery: any) {
    this.delivery = delivery ? new Delivery(delivery) : new Delivery();
    this.provinceField.nativeElement.focus();
  };

  loadDelivery(id: number) {
    this.getDeliverySub = this.deliveryService
      .getDeliveryById(id)
      .subscribe(delivery => this.setDelivery(delivery));
  };

  saveDelivery(deliveryForm: NgForm) {
    if (deliveryForm.valid) {
      this.isSaving = true;
      this.saveDeliverySub = this.deliveryService
        .saveDelivery(this.delivery)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    this.loadDeliveryForm();
    this.util.showSuccessMessage('Delivery saved successfully.');
  };

  saveFailed = (error) => {
    this.isSaving = false;
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  customerSelected() {
    if (this.selectedCustomer && this.selectedCustomer.id != 0 && !this.customerIdExists(this.selectedCustomer.id)) {
      this.fetchCustomerOrderItems(this.selectedCustomer);
      this.customerField.nativeElement.value = '';
      this.customerField.nativeElement.focus();
    }
  };

  customerIdExists(id: number) {
    return this.delivery.lineItems.find(x => x.customer.id == id) != null;
  };

  fetchCustomerOrderItems(customer: Customer) {
    this.getOrderLineItemSub = this.orderService
      .getOrderLineItemLookup(customer.id, 'delivery')
      .subscribe(data => {
        var newItems = data.map(orderLineItem =>
        {
          var quantityNotDelivered = orderLineItem.quantity - orderLineItem.quantityDelivered;
          var deliveryItem = new DeliveryLineItem(orderLineItem);
          deliveryItem.id = 0; // Set id to 0
          deliveryItem.quantity = quantityNotDelivered;
          deliveryItem.quantityNotDelivered = quantityNotDelivered;
          return deliveryItem;
        });
        this.delivery.lineItems = this.delivery.lineItems.concat(newItems);
      });
  };

  removeCustomer(customerId: number) {
    if (confirm('Are you sure you want to remove this customer? Items related to this customer will also be removed.')) {
      this.delivery.lineItems = this.delivery.lineItems.filter(val => val.customer.id != customerId);
    }
  };

  quantityUpdated(event: any, lineItem: DeliveryLineItem) {
    this.delivery.updateQuantity(lineItem.orderLineItemId, +event.target.value);
  };

  removeLineItem(index: number) {
    if (confirm('Are you sure you want to remove this item?')) {
      this.delivery.lineItems.splice(index, 1);
    }
  };

  // Autocomplete
  searchProvince = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? [] :
        this.provinceService.getProvinceLookup(term)
          .pipe(
            map(provinces => provinces.splice(0, 10))
          )
      )
    );

  searchCity = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? [] : this.filterCities(term))
    );

  searchCustomer = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? [] :
        this.customerService.getCustomerLookup(this.delivery.provinceId, this.delivery.cityId, term)
          .pipe(
            map(customers => customers.splice(0, 10))
          )
      )
    );

  provinceFormatter = (x: { name: string }) => x.name;
  cityFormatter = (x: { name: string }) => x.name;
  customerFormatter = (x: { name: string }) => x.name;

  private filterCities(value: string): City[] {
    const filterValue = value.toLowerCase();

    return this.delivery.province
      .cities
      .filter(city => city.name.toLowerCase().startsWith(filterValue))
      .splice(0, 10);
  }
}
