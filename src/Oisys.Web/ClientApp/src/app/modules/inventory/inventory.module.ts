import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../../shared/modules/shared.module';

import { InventoryListComponent } from './inventory-list/inventory-list.component';
import { InventoryFormComponent } from './inventory-form/inventory-form.component';
import { InventoryDetailComponent } from './inventory-detail/inventory-detail.component';
import { AdjustItemComponent } from './adjust-item/adjust-item.component';
import { ManufactureListComponent } from './manufacture-list/manufacture-list.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    InventoryListComponent,
    InventoryFormComponent,
    InventoryDetailComponent,
    AdjustItemComponent,
    ManufactureListComponent,
  ]
})
export class InventoryModule { }
