import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../../shared/modules/shared.module';

import { InventoryListComponent } from './inventory-list/inventory-list.component';
import { InventoryFormComponent } from './inventory-form/inventory-form.component';
import { InventoryDetailComponent } from './inventory-detail/inventory-detail.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    InventoryListComponent,
    InventoryFormComponent,
    InventoryDetailComponent
  ]
})
export class InventoryModule { }
