import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CashVoucherService } from '../../../shared/services/cash-voucher.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { CashVoucher } from '../../../shared/models/cash-voucher';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-voucher-detail',
  templateUrl: './voucher-detail.component.html',
  styleUrls: ['./voucher-detail.component.css']
})
export class VoucherDetailComponent extends PageBase implements AfterContentInit {
  voucher$: Observable<CashVoucher>;

  constructor(
    private cashVoucherService: CashVoucherService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);
  }

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
