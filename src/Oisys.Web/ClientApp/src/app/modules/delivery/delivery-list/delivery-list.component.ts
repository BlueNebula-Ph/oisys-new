import { Component, AfterContentInit, OnDestroy } from '@angular/core';

import { of, Observable, Subscription } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { DeliveryService } from '../../../shared/services/delivery.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Search } from '../../../shared/models/search';
import { Customer } from '../../../shared/models/customer';
import { Delivery } from '../../../shared/models/delivery';
import { Item } from '../../../shared/models/item';
import { Province } from '../../../shared/models/province';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-delivery-list',
  templateUrl: './delivery-list.component.html',
  styleUrls: ['./delivery-list.component.css']
})
export class DeliveryListComponent extends PageBase implements AfterContentInit, OnDestroy {
  page: Page = new Page();
  sort: Sort = new Sort();
  search: Search = new Search();

  rows$: Observable<Delivery[]>;
  customers$: Observable<Customer[]>;
  items$: Observable<Item[]>;
  provinces$: Observable<Province[]>;
  deleteDeliverySub: Subscription;

  isLoading: boolean = false;

  constructor(
    private deliveryService: DeliveryService,
    private customerService: CustomerService,
    private provinceService: ProvinceService,
    private inventoryService: InventoryService,
    private authService: AuthenticationService,
    private util: UtilitiesService
  ) {
    super(authService);

    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'date';
    this.sort.dir = 'desc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.fetchLists();
    this.loadDeliveries();
  };

  ngOnDestroy() {
    if (this.deleteDeliverySub) { this.deleteDeliverySub.unsubscribe(); }
  };

  loadDeliveries() {
    this.isLoading = true;
    this.rows$ = this.deliveryService.getDeliveries(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.search.searchTerm,
      this.search.customerId,
      this.search.provinceId,
      this.search.itemId,
      this.search.dateFrom,
      this.search.dateTo)
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
      );
  }

  fetchLists() {
    this.customers$ = this.customerService.getCustomerLookup();
    this.items$ = this.inventoryService.getItemLookup();
    this.provinces$ = this.provinceService.getProvinceLookup();
  };

  onDeleteDelivery(id: number): void {
    if (confirm('Are you sure you want to delete this delivery?')) {
      this.deleteDeliverySub = this.deliveryService.deleteDelivery(id)
        .subscribe(() => {
          this.loadDeliveries();
          this.util.showSuccessMessage('Delivery deleted successfully.');
        });
    }
  }

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadDeliveries();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadDeliveries();
    }
  }

  onSearch(event) {
    if (event) {
      // Reset page number on search.
      this.page.pageNumber = 0;
      this.search = event;
      this.loadDeliveries();
    }
  }
}
