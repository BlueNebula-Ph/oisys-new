import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderListComponent } from './order-list/order-list.component';
import { OrderDetailComponent } from './order-detail/order-detail.component';
import { OrderFormComponent } from './order-form/order-form.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [OrderListComponent, OrderDetailComponent, OrderFormComponent]
})
export class OrderModule { }
