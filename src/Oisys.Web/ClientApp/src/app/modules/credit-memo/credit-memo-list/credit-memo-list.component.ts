import { Component, AfterContentInit } from '@angular/core';

import { of, forkJoin } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { CreditMemoService } from '../../../shared/services/credit-memo.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Customer } from '../../../shared/models/customer';
import { CreditMemo } from '../../../shared/models/credit-memo';
import { Item } from '../../../shared/models/item';

@Component({
  selector: 'app-credit-memo-list',
  templateUrl: './credit-memo-list.component.html',
  styleUrls: ['./credit-memo-list.component.css']
})
export class CreditMemoListComponent implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  rows = new Array<CreditMemo>();

  searchTerm: string = '';
  dateFrom: Date;
  dateTo: Date;
  selectedCustomer: Customer = new Customer();
  customers: Customer[];
  selectedItem: Item = new Item();
  items: Item[];

  isLoading: boolean = false;

  constructor(
    private creditMemoService: CreditMemoService,
    private customerService: CustomerService,
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
    this.loadCreditMemos();
  };

  loadCreditMemos() {
    this.isLoading = true;
    this.creditMemoService.getCreditMemos(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.searchTerm,
      this.selectedCustomer.id,
      this.selectedItem.id,
      this.dateFrom,
      this.dateTo)
      .pipe(
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoading = false;
          this.page = data.pageInfo;

          return data.items.map(creditMemo => {
            //creditMemo.lineItems = creditMemo.lineItems.map(lineItem => new LineItem(lineItem));
            return new CreditMemo(creditMemo);
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
    ).subscribe(([customerResponse, inventoryResponse]) => {
      this.customers = customerResponse;
      this.items = inventoryResponse;
    });
  };

  onDeleteCreditMemo(id: number): void {
    if (confirm("Are you sure you want to delete this credit memo?")) {
      this.creditMemoService.deleteCreditMemo(id).subscribe(() => {
        this.loadCreditMemos();
        this.util.showSuccessMessage("Credit memo deleted successfully.");
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

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadCreditMemos();
  }

  clear() {
    this.searchTerm = '';
    this.selectedCustomer = new Customer();
    this.selectedItem = new Item();
    this.dateFrom = null;
    this.dateTo = null;
  }
}
