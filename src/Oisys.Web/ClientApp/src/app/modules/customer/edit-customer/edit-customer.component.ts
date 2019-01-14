import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { fromEvent, Observable } from 'rxjs';

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
  filteredProvinces: Province[];
  cities: City[];
  filteredCities: City[];

  @ViewChild('provinceBox') provinceBox: ElementRef;
  @ViewChild('cityBox') cityBox: ElementRef;

  constructor(private customerService: CustomerService, private provinceService: ProvinceService, private util: UtilitiesService, private router: Router) {}

  ngOnInit() {
    this.fetchProvinces();
    this.loadCustomer();

    //fromEvent(this.provinceBox.nativeElement, 'keyup')
    //  .subscribe(() => {
    //    const searchVal = this.provinceBox.nativeElement.value;
    //    this.filteredProvinces = this.filterProvinces(searchVal);
    //  });

    //fromEvent(this.cityBox.nativeElement, 'keyup')
    //  .subscribe(() => {
    //    const searchVal = this.cityBox.nativeElement.value;
    //    this.filteredCities = this.filterCities(searchVal);
    //  });
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

  displayProvince(prov: Province) {
    if (prov) {
      return prov.name;
    }
  };

  displayCity(city: City) {
    if (city) {
      return city.name;
    }
  };

  onProvinceSelected(prov: Province) {
    if (prov) {
      this.customer.selectedCity = undefined;
      this.cities = prov.cities;
    }
  };

  private filterProvinces(value: string): Province[] {
    const filterValue = value.toLowerCase();

    return this.provinces.filter(province => province.name.toLowerCase().indexOf(filterValue) === 0);
  }

  private filterCities(value: string): City[] {
    const filterValue = value.toLowerCase();

    return this.cities.filter(city => city.name.toLowerCase().indexOf(filterValue) === 0);
  }
}
