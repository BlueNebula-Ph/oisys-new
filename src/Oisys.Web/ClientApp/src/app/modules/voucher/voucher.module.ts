import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/modules/shared.module';

import { VoucherDetailComponent } from './voucher-detail/voucher-detail.component';
import { VoucherListComponent } from './voucher-list/voucher-list.component';
import { VoucherFormComponent } from './voucher-form/voucher-form.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    VoucherDetailComponent,
    VoucherListComponent,
    VoucherFormComponent
  ]
})
export class VoucherModule { }
