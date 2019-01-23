import { Component, AfterContentInit } from '@angular/core';

import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { CashVoucherService } from '../../../shared/services/cash-voucher.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { CashVoucher } from '../../../shared/models/cash-voucher';

@Component({
  selector: 'app-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.css']
})
export class VoucherListComponent implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  rows = new Array<CashVoucher>();

  searchTerm: string = '';
  dateFrom: Date;
  dateTo: Date;

  isLoading: boolean = false;

  constructor(
    private cashVoucherService: CashVoucherService,
    private util: UtilitiesService) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'date';
    this.sort.dir = 'desc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.loadCashVouchers();
  };

  loadCashVouchers() {
    this.isLoading = true;
    this.cashVoucherService.getCashVouchers(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.searchTerm,
      this.dateFrom,
      this.dateTo)
      .pipe(
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoading = false;
          this.page = data.pageInfo;

          return data.items.map(cashVoucher => new CashVoucher(cashVoucher));
        }),
        catchError(() => {
          this.isLoading = false;

          return of([]);
        })
      )
      .subscribe(data => this.rows = data);
  }

  onDeleteCashVoucher(id: number): void {
    if (confirm("Are you sure you want to delete this voucher?")) {
      this.cashVoucherService.deleteCashVoucher(id).subscribe(() => {
        this.loadCashVouchers();
        this.util.showSuccessMessage("Voucher deleted successfully.");
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

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadCashVouchers();
  }

  clear() {
    this.searchTerm = '';
    this.dateFrom = null;
    this.dateTo = null;
  }
}
