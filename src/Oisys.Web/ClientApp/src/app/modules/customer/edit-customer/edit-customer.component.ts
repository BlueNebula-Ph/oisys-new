import { Component, AfterContentInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { CustomerService } from '../../../shared/services/customer.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Customer } from '../../../shared/models/customer';
import { City } from '../../../shared/models/city';
import { PriceList } from '../../../shared/models/price-list';

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements AfterContentInit, OnDestroy {
  customer = new Customer();
  priceList = PriceList;
  getCustomerSub: Subscription;
  saveCustomerSub: Subscription;
  isSaving = false;

  @ViewChild('name') nameField: ElementRef;

  constructor(
    private customerService: CustomerService,
    private provinceService: ProvinceService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.loadCustomerForm();
  };

  ngOnDestroy() {
    if (this.getCustomerSub) { this.getCustomerSub.unsubscribe(); }
    if (this.saveCustomerSub) { this.saveCustomerSub.unsubscribe(); }
  };

  loadCustomerForm() {
    const customerId = +this.route.snapshot.paramMap.get('id');
    if (customerId && customerId != 0) {
      this.loadCustomer(customerId);
    } else {
      this.setCustomer(undefined);
    }
  };

  setCustomer(customer: any) {
    this.customer = customer ? new Customer(customer) : new Customer();
    this.nameField.nativeElement.focus();
  };

  loadCustomer(id: number) {
    this.getCustomerSub = this.customerService
      .getCustomerById(id)
      .subscribe(customer => this.setCustomer(customer));
  }

  saveCustomer(customerForm: NgForm) {
    if (customerForm.valid) {
      this.isSaving = true;
      this.customerService
        .saveCustomer(this.customer)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    this.loadCustomerForm();
    this.util.showSuccessMessage('Customer saved successfully.');
  };

  saveFailed = (error) => {
    this.isSaving = false;
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  provinceUpdated() {
    this.customer.city = new City();
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

    return this.customer.province.cities.filter(city => city.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }
}
