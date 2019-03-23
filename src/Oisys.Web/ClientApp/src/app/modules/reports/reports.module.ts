import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../../shared/modules/shared.module';

import { CountSheetComponent } from './count-sheet/count-sheet.component';
import { ProductSalesComponent } from './product-sales/product-sales.component';
import { OrderSummaryComponent } from './order-summary/order-summary.component';
import { ReportSelectorComponent } from './report-selector/report-selector.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FormsModule
  ],
  declarations: [
    CountSheetComponent,
    ProductSalesComponent,
    OrderSummaryComponent,
    ReportSelectorComponent
  ]
})
export class ReportsModule { }
