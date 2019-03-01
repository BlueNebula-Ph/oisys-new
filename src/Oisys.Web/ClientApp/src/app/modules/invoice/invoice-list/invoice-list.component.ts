import { Component, AfterContentInit, OnDestroy } from '@angular/core';

import { of, Observable, Subscription } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { InvoiceService } from '../../../shared/services/invoice.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Search } from '../../../shared/models/search';
import { Customer } from '../../../shared/models/customer';
import { Invoice } from '../../../shared/models/invoice';

@Component({
  selector: 'app-invoice-list',
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.css']
})
export class InvoiceListComponent implements AfterContentInit, OnDestroy {
  page: Page = new Page();
  sort: Sort = new Sort();
  search: Search = new Search();

  rows$: Observable<Invoice[]>;
  customers$: Observable<Customer[]>;
  deleteInvoiceSub: Subscription;

  isLoading: boolean = false;

  constructor(
    private invoiceService: InvoiceService,
    private customerService: CustomerService,
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

  ngOnDestroy() {
    if (this.deleteInvoiceSub) { this.deleteInvoiceSub.unsubscribe(); }
  };

  loadInvoices() {
    this.isLoading = true;
    this.rows$ = this.invoiceService.getInvoices(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.search.searchTerm,
      this.search.customerId,
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
  };

  onDeleteInvoice(id: number): void {
    if (confirm('Are you sure you want to delete this invoice?')) {
      this.deleteInvoiceSub = this.invoiceService.deleteInvoice(id)
        .subscribe(() => {
          this.loadInvoices();
          this.util.showSuccessMessage('Invoice deleted successfully.');
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

  onSearch(event) {
    if (event) {
      // Reset page number on search.
      this.page.pageNumber = 0;
      this.search = event;
      this.loadInvoices();
    }
  }
}
