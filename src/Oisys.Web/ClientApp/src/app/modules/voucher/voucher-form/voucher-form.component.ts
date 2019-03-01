import { Component, AfterContentInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Subscription } from 'rxjs';

import { CashVoucherService } from '../../../shared/services/cash-voucher.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { CashVoucher } from '../../../shared/models/cash-voucher';
import { VoucherCategory } from '../../../shared/models/voucher-category';

@Component({
  selector: 'app-voucher-form',
  templateUrl: './voucher-form.component.html',
  styleUrls: ['./voucher-form.component.css']
})
export class VoucherFormComponent implements AfterContentInit, OnDestroy {
  voucher: CashVoucher = new CashVoucher();
  voucherCategories = VoucherCategory;
  getVoucherSub: Subscription;
  saveVoucherSub: Subscription;
  isSaving = false;

  @ViewChild('payTo') payToField: ElementRef;

  constructor(
    private cashVoucherService: CashVoucherService,
    private util: UtilitiesService,
    private route: ActivatedRoute
  ) { }

  ngAfterContentInit() {
    this.loadVoucherForm();
  };

  ngOnDestroy() {
    if (this.getVoucherSub) { this.getVoucherSub.unsubscribe(); }
    if (this.saveVoucherSub) { this.saveVoucherSub.unsubscribe(); }
  };

  loadVoucherForm() {
    const voucherId = +this.route.snapshot.paramMap.get('id');
    if (voucherId && voucherId != 0) {
      this.loadVoucher(voucherId);
    } else {
      this.setVoucher(undefined);
    }
  };

  setVoucher(voucher: any) {
    this.voucher = voucher ? new CashVoucher(voucher) : new CashVoucher();
    this.payToField.nativeElement.focus();
  };

  loadVoucher(id: number) {
    this.getVoucherSub = this.cashVoucherService
      .getCashVoucherById(id)
      .subscribe(voucher => this.setVoucher(voucher));
  };

  saveCashVoucher(cashVoucherForm: NgForm) {
    if (cashVoucherForm.valid) {
      this.isSaving = true;
      this.cashVoucherService
        .saveCashVoucher(this.voucher)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    if (this.voucher.id == 0) {
      this.setVoucher(undefined);
    }
    this.util.showSuccessMessage('Cash voucher saved successfully.');
  };

  saveFailed = (error) => {
    this.util.showErrorMessage('An error occurred while saving. Please try again.');
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };
}
