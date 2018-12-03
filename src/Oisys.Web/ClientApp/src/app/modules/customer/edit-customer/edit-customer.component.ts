import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Location } from '@angular/common'
import { CustomerService } from '../../../shared/services/customer.service';
import { Customer } from '../../../shared/models/customer';
import { NgForm } from '@angular/forms';
import { PriceList } from '../../../shared/models/price-list';
import { fromEvent, Observable } from 'rxjs';
import { Province } from '../../../shared/models/Province';
import { ProvinceService } from '../../../shared/services/province.service';
import { City } from '../../../shared/models/city';
import { UtilitiesService } from '../../../shared/services/utilities.service';

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

  constructor(private customerService: CustomerService, private provinceService: ProvinceService, private util: UtilitiesService, private location: Location) {}

  ngOnInit() {
    this.fetchProvinces();
    this.loadCustomer();

    fromEvent(this.provinceBox.nativeElement, 'keyup')
      .subscribe(() => {
        const searchVal = this.provinceBox.nativeElement.value;
        this.filteredProvinces = this.filterProvinces(searchVal);
      });

    fromEvent(this.cityBox.nativeElement, 'keyup')
      .subscribe(() => {
        const searchVal = this.cityBox.nativeElement.value;
        this.filteredCities = this.filterCities(searchVal);
      });
  };

  backToSummary() {
    this.location.back();
  };

  saveCustomer(customerForm: NgForm) {
    if (customerForm.valid) {
      this.customerService.saveCustomer(this.customer)
        .subscribe(result => {
          this.loadCustomer();
          customerForm.resetForm();

          this.util.openSnackBar("Customer saved successfully.");
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
