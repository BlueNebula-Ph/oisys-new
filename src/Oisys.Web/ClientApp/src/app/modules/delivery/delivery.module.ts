import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeliveryDetailComponent } from './delivery-detail/delivery-detail.component';
import { DeliveryListComponent } from './delivery-list/delivery-list.component';
import { DeliveryFormComponent } from './delivery-form/delivery-form.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [DeliveryDetailComponent, DeliveryListComponent, DeliveryFormComponent]
})
export class DeliveryModule { }
