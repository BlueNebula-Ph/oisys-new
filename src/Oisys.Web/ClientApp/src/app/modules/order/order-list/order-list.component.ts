import { Component, AfterContentInit } from '@angular/core';

import { of, forkJoin } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { OrderService } from '../../../shared/services/order.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Customer } from '../../../shared/models/customer';
import { Order } from '../../../shared/models/order';
import { Item } from '../../../shared/models/item';
import { Province } from '../../../shared/models/province';
import { LineItem } from '../../../shared/models/line-item';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  rows = new Array<Order>();

  searchTerm: string = '';
  dateFrom: Date;
  dateTo: Date;
  selectedCustomer: Customer = new Customer();
  customers: Customer[];
  selectedItem: Item = new Item();
  items: Item[];
  provinces: Province[];
  selectedProvince: Province = new Province();

  isLoading: boolean = false;

  constructor(
    private orderService: OrderService,
    private customerService: CustomerService,
    private provinceService: ProvinceService,
    private inventoryService: InventoryService,
    private util: UtilitiesService) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'date';
    this.sort.dir = 'desc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.fetchLists();
    this.loadOrders();
  };

  loadOrders() {
    this.isLoading = true;
    this.orderService.getOrders(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.searchTerm,
      this.selectedCustomer.id,
      this.selectedProvince.id,
      this.selectedItem.id,
      this.dateFrom,
      this.dateTo)
      .pipe(
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoading = false;
          this.page = data.pageInfo;

          return data.items.map(order => {
            order.lineItems = order.lineItems.map(lineItem => new LineItem(lineItem));
            return new Order(order);
          });
        }),
        catchError(() => {
          this.isLoading = false;

          return of([]);
        })
      )
      .subscribe(data => this.rows = data);
  }

  fetchLists() {
    forkJoin(
      this.customerService.getCustomerLookup(),
      this.inventoryService.getItemLookup(),
      this.provinceService.getProvinceLookup()
    ).subscribe(([customerResponse, inventoryResponse, provinceResponse]) => {
      this.customers = customerResponse;
      this.items = inventoryResponse;
      this.provinces = provinceResponse;
    });
  };

  onDeleteOrder(id: number): void {
    if (confirm("Are you sure you want to delete this order?")) {
      this.orderService.deleteOrder(id).subscribe(() => {
        this.loadOrders();
        this.util.showSuccessMessage("Order deleted successfully.");
      });
    }
  }

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadOrders();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadOrders();
    }
  }

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadOrders();
  }

  clear() {
    this.searchTerm = '';
    this.selectedCustomer = new Customer();
    this.selectedItem = new Item();
    this.selectedProvince = new Province();
    this.dateFrom = null;
    this.dateTo = null;
  }
}
