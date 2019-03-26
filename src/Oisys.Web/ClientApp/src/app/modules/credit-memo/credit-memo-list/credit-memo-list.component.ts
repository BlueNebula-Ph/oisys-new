import { Component, AfterContentInit, OnDestroy } from '@angular/core';

import { of, Observable, Subscription } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { CreditMemoService } from '../../../shared/services/credit-memo.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Search } from '../../../shared/models/search';
import { Customer } from '../../../shared/models/customer';
import { CreditMemo } from '../../../shared/models/credit-memo';
import { Item } from '../../../shared/models/item';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-credit-memo-list',
  templateUrl: './credit-memo-list.component.html',
  styleUrls: ['./credit-memo-list.component.css']
})
export class CreditMemoListComponent extends PageBase implements AfterContentInit, OnDestroy {
  page: Page = new Page();
  sort: Sort = new Sort();
  search: Search = new Search();

  rows$: Observable<CreditMemo[]>;
  customers$: Observable<Customer[]>;
  items$: Observable<Item[]>;
  deleteCreditMemoSub: Subscription;

  isLoading: boolean = false;

  constructor(
    private creditMemoService: CreditMemoService,
    private customerService: CustomerService,
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
    this.loadCreditMemos();
  };

  ngOnDestroy() {
    if (this.deleteCreditMemoSub) { this.deleteCreditMemoSub.unsubscribe(); }
  };

  loadCreditMemos() {
    this.isLoading = true;
    this.rows$ = this.creditMemoService.getCreditMemos(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.search.searchTerm,
      this.search.customerId,
      this.search.itemId,
      this.search.dateFrom,
      this.search.dateTo,
      this.search.isInvoiced)
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
  };

  onDeleteCreditMemo(id: number): void {
    if (confirm('Are you sure you want to delete this credit memo?')) {
      this.deleteCreditMemoSub = this.creditMemoService.deleteCreditMemo(id)
        .subscribe(() => {
          this.loadCreditMemos();
          this.util.showSuccessMessage('Credit memo deleted successfully.');
        });
    }
  }

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadCreditMemos();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadCreditMemos();
    }
  }

  onSearch(event) {
    if (event) {
      // Reset page number on search.
      this.page.pageNumber = 0;
      this.search = event;
      this.loadCreditMemos();
    }
  }
}
