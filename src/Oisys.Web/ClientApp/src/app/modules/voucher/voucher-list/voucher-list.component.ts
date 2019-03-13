import { Component, AfterContentInit, OnDestroy } from '@angular/core';

import { of, Observable, Subscription } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { CashVoucherService } from '../../../shared/services/cash-voucher.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Search } from '../../../shared/models/search';
import { CashVoucher } from '../../../shared/models/cash-voucher';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.css']
})
export class VoucherListComponent extends PageBase implements AfterContentInit, OnDestroy {
  page: Page = new Page();
  sort: Sort = new Sort();
  search: Search = new Search();

  rows$: Observable<CashVoucher[]>;
  deleteVoucherSub: Subscription;

  isLoading: boolean = false;

  constructor(
    private cashVoucherService: CashVoucherService,
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
    this.loadCashVouchers();
  };

  ngOnDestroy() {
    if (this.deleteVoucherSub) { this.deleteVoucherSub.unsubscribe(); }
  };

  loadCashVouchers() {
    this.isLoading = true;
    this.rows$ = this.cashVoucherService.getCashVouchers(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.search.searchTerm,
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

  onDeleteCashVoucher(id: number): void {
    if (confirm('Are you sure you want to delete this voucher?')) {
      this.deleteVoucherSub = this.cashVoucherService.deleteCashVoucher(id).subscribe(() => {
        this.loadCashVouchers();
        this.util.showSuccessMessage('Voucher deleted successfully.');
      });
    }
  }

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadCashVouchers();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadCashVouchers();
    }
  }

  onSearch(event) {
    if (event) {
      // Reset page number on search.
      this.page.pageNumber = 0;
      this.search = event;
      this.loadCashVouchers();
    }
  }
}
