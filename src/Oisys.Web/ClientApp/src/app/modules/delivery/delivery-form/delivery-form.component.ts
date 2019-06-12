import { Component, AfterContentInit, ElementRef, ViewChild, OnDestroy } from '@angular/core';

import { ActivatedRoute, Event } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { DeliveryService } from '../../../shared/services/delivery.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { OrderService } from '../../../shared/services/order.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Delivery } from '../../../shared/models/delivery';
import { DeliveryLineItem } from '../../../shared/models/delivery-line-item';
import { City } from '../../../shared/models/city';
import { Province } from '../../../shared/models/province';

@Component({
  selector: 'app-delivery-form',
  templateUrl: './delivery-form.component.html',
  styleUrls: ['./delivery-form.component.css']
})
export class DeliveryFormComponent implements AfterContentInit, OnDestroy {
  delivery: Delivery = new Delivery();

  getDeliverySub: Subscription;
  getOrderLineItemSub: Subscription;
  saveDeliverySub: Subscription;

  selectedProvince: Province;
  selectedCity: City;

  isSaving = false;

  @ViewChild('province') provinceField: ElementRef;
  @ViewChild('deliveryDate') dateField: ElementRef;

  constructor(
    private deliveryService: DeliveryService,
    private provinceService: ProvinceService,
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
    this.dateField.nativeElement.focus();
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

  citySelected() {
    if (!this.selectedProvince && !this.selectedCity) {
      return;
    }

    const area = `${this.selectedCity.name}, ${this.selectedProvince.name}`;
    const areaExists = this.delivery.deliveryAreaList.find(a => a == area);
    if (this.selectedCity.id && this.selectedProvince.id && !areaExists) {
      this.fetchItemsForDelivery(this.selectedProvince.id, this.selectedCity.id);

      this.selectedProvince = undefined;
      this.selectedCity = undefined;
      this.provinceField.nativeElement.focus();
    }
  };

  fetchItemsForDelivery(provinceId: number, cityId: number) {
    this.getOrderLineItemSub = this.orderService
      .getOrderLineItemsForDelivery(provinceId, cityId)
      .subscribe(data => {
        const newItems = data.map(item => {
          item.id = 0;
          item.quantity = item.quantity - item.quantityDelivered;
          return new DeliveryLineItem(item);
        });
        this.delivery.lineItems = this.delivery.lineItems.concat(newItems);
        this.delivery.groupLineItems();
      });
  };

  quantityUpdated(event: any, lineItem: DeliveryLineItem) {
    this.delivery.updateQuantity(lineItem.orderLineItemId, event.target.valueAsNumber);
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

  provinceFormatter = (x: { name: string }) => x.name;
  cityFormatter = (x: { name: string }) => x.name;

  private filterCities(value: string): City[] {
    const filterValue = value.toLowerCase();

    return this.selectedProvince
      .cities
      .filter(city => city.name.toLowerCase().startsWith(filterValue))
      .splice(0, 10);
  }
}
