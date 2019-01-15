import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { CustomerService } from '../../../shared/services/customer.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Customer } from '../../../shared/models/customer';
import { Province } from '../../../shared/models/Province';
import { City } from '../../../shared/models/city';
import { PriceList } from '../../../shared/models/price-list';

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements OnInit {
  customer: Customer = new Customer();
  priceList = PriceList;
  provinces: Province[];

  constructor(private customerService: CustomerService, private provinceService: ProvinceService, private util: UtilitiesService, private router: Router, private config: NgbTypeaheadConfig) {
    this.config.showHint = true;
  }

  ngOnInit() {
    this.fetchProvinces();
    this.loadCustomer();
  };

  backToSummary() {
    this.router.navigateByUrl('/customers');
  };

  saveCustomer(customerForm: NgForm) {
    if (customerForm.valid) {
      this.customerService.saveCustomer(this.customer)
        .subscribe(result => {
          this.loadCustomer();
          customerForm.resetForm();

          this.util.showSuccessMessage("Customer saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  loadCustomer() {
    this.customer = new Customer();
  };

  fetchProvinces() {
    this.provinceService.getProvinceLookup()
      .subscribe(results => {
        this.provinces = results;
      });
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

  provinceFormatter = (x: { name: string }) => x.name;
  cityFormatter = (x: { name: string }) => x.name;

  private filterProvinces(value: string): Province[] {
    const filterValue = value.toLowerCase();

    return this.provinces.filter(province => province.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }

  private filterCities(value: string): City[] {
    const filterValue = value.toLowerCase();

    return this.customer.selectedProvince.cities.filter(city => city.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }
}
