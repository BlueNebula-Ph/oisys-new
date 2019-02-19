import { Component, AfterContentInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, forkJoin } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { DeliveryService } from '../../../shared/services/delivery.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { OrderService } from '../../../shared/services/order.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Delivery } from '../../../shared/models/delivery';
import { DeliveryLineItem } from '../../../shared/models/delivery-line-item';
import { Customer } from '../../../shared/models/customer';
import { Province } from '../../../shared/models/province';
import { City } from '../../../shared/models/city';

@Component({
  selector: 'app-delivery-form',
  templateUrl: './delivery-form.component.html',
  styleUrls: ['./delivery-form.component.css']
})
export class DeliveryFormComponent implements AfterContentInit {
  delivery: Delivery = new Delivery();
  provinces: Province[];
  customers: Customer[];
  selectedCustomer: Customer;
  selectedCustomers: Customer[] = new Array<Customer>();

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
    this.fetchLists();
    //setTimeout(() => this.loadDelivery(), 3000);
  };

  saveDelivery(deliveryForm: NgForm) {
    if (deliveryForm.valid) {
      this.deliveryService
        .saveDelivery(this.delivery)
        .subscribe(() => {
          if (this.delivery.id == 0) {
            //this.loadDelivery();
          }
          this.util.showSuccessMessage("Delivery saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  //loadDelivery() {
  //  this.route.paramMap.subscribe(params => {
  //    var routeParam = params.get("id");
  //    var id = parseInt(routeParam);

  //    if (id == 0) {
  //      this.delivery = new Delivery();
  //    } else {
  //      this.deliveryService
  //        .getDeliveryById(id)
  //        .subscribe(delivery => {
  //          this.delivery = new Delivery(delivery);
  //          this.delivery.lineItems = delivery.lineItems.map(lineItem => {
  //            var deliveryLineItem = new LineItem(lineItem);
  //            deliveryLineItem.selectedItem = this.filterItems(lineItem.itemName)[0];
  //            return deliveryLineItem;
  //          });
  //          this.delivery.selectedCustomer = this.filterCustomers(delivery.customerName)[0];
  //        });
  //    }
  //  });
  //};

  fetchLists() {
    forkJoin(
      this.provinceService.getProvinceLookup()
    ).subscribe(([provinceResponse]) => {
      this.provinces = provinceResponse;
    });
  };

  provinceUpdated() {
    this.delivery.selectedCity = new City();
  };

  cityUpdated() {
    if (this.delivery.provinceId && this.delivery.cityId) {
      this.customerService
        .getCustomerLookup(this.delivery.provinceId, this.delivery.cityId)
        .subscribe(data => this.customers = data);
    }
  };

  customerSelected() {
    if (this.selectedCustomer && this.selectedCustomer.id != 0 && !this.customerIdExists(this.selectedCustomer.id)) {
      this.selectedCustomers.push(this.selectedCustomer);
      this.fetchCustomerOrderItems(this.selectedCustomer);
    }
  };

  customerIdExists(id: number) {
    return this.selectedCustomers.find(x => x.id == id) != null;
  };

  removeCustomer(index: number) {
    if (confirm('Are you sure you want to remove this customer? Items related to this customer will also be removed.')) {
      var customers = this.selectedCustomers.splice(index, 1);
      var customer = customers[0];
      if (customer) {
        this.delivery.lineItems = this.delivery.lineItems.filter(val => val.customerId != customer.id);
      }
    }
  };

  fetchCustomerOrderItems(customer: Customer) {
    this.orderService.getOrderLineItemLookup(customer.id, false)
      .subscribe(data => {
        this.delivery.lineItems = data.map(lineItem => new DeliveryLineItem({
          customerId: customer.id,
          customer: customer.name,
          orderLineItemId: lineItem.id,
          orderNumber: lineItem.orderCode,
          orderDate: lineItem.orderDate,
          quantity: lineItem.quantity,
          itemCode: lineItem.itemCode,
          itemName: lineItem.itemName,
          category: lineItem.categoryName
        }));

        this.selectedCustomer = new Customer();
      });
  };

  // Line items
  addLineItem() {
    //this.delivery.lineItems.push(new LineItem());
    //this.delivery.updateLineItems();
  };

  removeLineItem(index: number) {
    if (confirm('Are you sure you want to remove this item?')) {
      //this.delivery.lineItems.splice(index, 1);
    }
  };

  // Autocomplete
  searchProvince = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? [] : this.filterProvinces(term))
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
      map(term => term.length < 2 ? [] : this.filterCustomers(term))
    );

  provinceFormatter = (x: { name: string }) => x.name;
  cityFormatter = (x: { name: string }) => x.name;
  customerFormatter = (x: { name: string }) => x.name;

  private filterProvinces(value: string): Province[] {
    const filterValue = value.toLowerCase();

    return this.provinces.filter(province => province.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }

  private filterCities(value: string): City[] {
    const filterValue = value.toLowerCase();

    return this.delivery.selectedProvince.cities.filter(city => city.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }

  private filterCustomers(value: string): Customer[] {
    const filterValue = value.toLowerCase();

    return this.customers.filter(customer => customer.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }
}
