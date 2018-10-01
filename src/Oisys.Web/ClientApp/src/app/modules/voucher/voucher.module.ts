import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VoucherDetailComponent } from './voucher-detail/voucher-detail.component';
import { VoucherListComponent } from './voucher-list/voucher-list.component';
import { VoucherFormComponent } from './voucher-form/voucher-form.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [VoucherDetailComponent, VoucherListComponent, VoucherFormComponent]
})
export class VoucherModule { }
