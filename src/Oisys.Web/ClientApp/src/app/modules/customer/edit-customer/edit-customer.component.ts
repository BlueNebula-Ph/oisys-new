import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private customerService: CustomerService, private provinceService: ProvinceService, private util: UtilitiesService, private route: ActivatedRoute, private config: NgbTypeaheadConfig) {
    this.config.showHint = true;
  }

  ngOnInit() {
    this.fetchProvinces();
    this.loadCustomer();
  };

  saveCustomer(customerForm: NgForm) {
    if (customerForm.valid) {
      this.customerService
        .saveCustomer(this.customer)
        .subscribe(() => {
          if (this.customer.id == 0) {
            this.loadCustomer();
          }
          this.util.showSuccessMessage("Customer saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  loadCustomer() {
    this.route.paramMap.subscribe(params => {
      var routeParam = params.get("id");
      var id = parseInt(routeParam);

      if (id == 0) {
        this.customer = new Customer();
      } else {
        this.customerService
          .getCustomerById(id)
          .subscribe(c => {
            this.customer = new Customer(c.id,
              c.name,
              c.email,
              c.contactNumber,
              c.contactPerson,
              c.address,
              c.cityId,
              c.provinceId,
              c.terms,
              c.discount,
              c.priceListId);
            this.customer.selectedProvince = this.filterProvinces(c.provinceName)[0];
            this.customer.selectedCity = this.filterCities(c.cityName)[0];
          });
      }
    });
  };

  fetchProvinces() {
    this.provinceService.getProvinceLookup()
      .subscribe(results => {
        this.provinces = results;
      });
  };

  provinceUpdated() {
    this.customer.selectedCity = new City(); 
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
