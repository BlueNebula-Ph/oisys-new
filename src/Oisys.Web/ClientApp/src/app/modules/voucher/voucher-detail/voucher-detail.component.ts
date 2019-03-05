import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CashVoucher } from '../../../shared/models/cash-voucher';
import { CashVoucherService } from '../../../shared/services/cash-voucher.service';

@Component({
  selector: 'app-voucher-detail',
  templateUrl: './voucher-detail.component.html',
  styleUrls: ['./voucher-detail.component.css']
})
export class VoucherDetailComponent implements AfterContentInit {
  voucher$: Observable<CashVoucher>;

  constructor(
    private cashVoucherService: CashVoucherService,
    private route: ActivatedRoute
  ) { }

  ngAfterContentInit() {
    this.loadVoucherDetails();
  };

  loadVoucherDetails() {
    const cashVoucherId = +this.route.snapshot.paramMap.get('id');
    if (cashVoucherId && cashVoucherId != 0) {
      this.voucher$ = this.cashVoucherService
        .getCashVoucherById(cashVoucherId)
        .pipe(
          map(voucher => new CashVoucher(voucher))
        );
    }
  };
}
