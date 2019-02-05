import { Component, AfterContentInit } from '@angular/core';

import { of, forkJoin } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { InvoiceService } from '../../../shared/services/invoice.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { ProvinceService } from '../../../shared/services/province.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Customer } from '../../../shared/models/customer';
import { Invoice } from '../../../shared/models/invoice';
import { Item } from '../../../shared/models/item';
import { Province } from '../../../shared/models/province';
import { InvoiceLineItem } from '../../../shared/models/invoice-line-item';

@Component({
  selector: 'app-invoice-list',
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.css']
})
export class InvoiceListComponent implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  rows = new Array<Invoice>();

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
    private invoiceService: InvoiceService,
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
    this.loadInvoices();
  };

  loadInvoices() {
    this.isLoading = true;
    this.invoiceService.getInvoices(
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

          //return data.items.map(invoice => {
          //  invoice.lineItems = invoice.lineItems.map(lineItem => new LineItem(lineItem));
          //  return new Invoice(invoice);
          //});
          return data.items;
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

  onDeleteInvoice(id: number): void {
    if (confirm("Are you sure you want to delete this invoice?")) {
      this.invoiceService.deleteInvoice(id).subscribe(() => {
        this.loadInvoices();
        this.util.showSuccessMessage("Invoice deleted successfully.");
      });
    }
  }

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadInvoices();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadInvoices();
    }
  }

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadInvoices();
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
