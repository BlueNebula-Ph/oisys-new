import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../../shared/modules/shared.module';

import { DeliveryDetailComponent } from './delivery-detail/delivery-detail.component';
import { DeliveryListComponent } from './delivery-list/delivery-list.component';
import { DeliveryFormComponent } from './delivery-form/delivery-form.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    DeliveryDetailComponent,
    DeliveryListComponent,
    DeliveryFormComponent
  ]
})
export class DeliveryModule { }
