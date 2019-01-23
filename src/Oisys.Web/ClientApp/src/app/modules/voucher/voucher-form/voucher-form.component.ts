import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { CashVoucherService } from '../../../shared/services/cash-voucher.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { CashVoucher } from '../../../shared/models/cash-voucher';
import { VoucherCategory } from '../../../shared/models/voucher-category';

@Component({
  selector: 'app-voucher-form',
  templateUrl: './voucher-form.component.html',
  styleUrls: ['./voucher-form.component.css']
})
export class VoucherFormComponent implements AfterContentInit {
  voucher: CashVoucher = new CashVoucher();
  voucherCategories = VoucherCategory;

  constructor(private cashVoucherService: CashVoucherService, private util: UtilitiesService, private route: ActivatedRoute) {
  }

  ngAfterContentInit() {
    this.loadVoucher();
  }

  saveCashVoucher(cashVoucherForm: NgForm) {
    if (!cashVoucherForm.valid) {
      console.log(cashVoucherForm.errors);
    }

    this.cashVoucherService
      .saveCashVoucher(this.voucher)
      .subscribe(() => {
        this.loadVoucher();
        this.util.showSuccessMessage('Cash voucher saved successfully.');
      }, () => {
        this.util.showErrorMessage('An error has occurred.');
      });
  };

  loadVoucher() {
    this.route.paramMap.subscribe(params => {
      var routeParam = params.get("id");
      var id = parseInt(routeParam);

      if (id == 0) {
        this.voucher = new CashVoucher();
      } else {
        this.cashVoucherService
          .getCashVoucherById(id)
          .subscribe(voucher => {
            voucher.category = VoucherCategory[voucher.category];
            this.voucher = new CashVoucher(voucher);
          });
      }
    });
  };
}
