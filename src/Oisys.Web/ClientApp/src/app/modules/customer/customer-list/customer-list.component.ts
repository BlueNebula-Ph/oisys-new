import { Component, AfterContentInit } from '@angular/core';

import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { CustomerService } from '../../../shared/services/customer.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Customer } from '../../../shared/models/customer';
import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Search } from '../../../shared/models/search';
import { Province } from '../../../shared/models/province';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  search: Search = new Search();
  rows = new Array<Customer>();
  
  provinces: Province[];

  isLoading: boolean = false;

  constructor(private customerService: CustomerService, private provinceService: ProvinceService, private util: UtilitiesService) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'name';
    this.sort.dir = 'asc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.fetchProvinces();
    this.loadCustomers();
  };

  loadCustomers() {
    this.isLoading = true;
    this.customerService.getCustomers(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.search.searchTerm,
      this.search.provinceId,
      this.search.cityId)
      .pipe(
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoading = false;

          this.page = data.pageInfo;

          return data.items;
        }),
        catchError(() => {
          this.isLoading = false;

          return of([]);
        })
      )
      .subscribe(data => this.rows = data);
  }

  fetchProvinces() {
    this.provinceService.getProvinceLookup()
      .subscribe(results => {
        this.provinces = results;
      });
  };

  onDeleteCustomer(id: number): void {
    if (confirm("Are you sure you want to delete this customer?")) {
      this.customerService.deleteCustomer(id).subscribe(() => {
        this.loadCustomers();
        this.util.showSuccessMessage("Customer deleted successfully.");
      });
    }
  }

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadCustomers();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadCustomers();
    }
  }

  onSearch(event) {
    if (event) {
      // Reset page number on search.
      this.page.pageNumber = 0;
      this.search = event;
      this.loadCustomers();
    }
  }
}
