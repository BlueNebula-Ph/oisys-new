import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../../shared/modules/shared.module';

import { QuotationListComponent } from './quotation-list/quotation-list.component';
import { QuotationDetailComponent } from './quotation-detail/quotation-detail.component';
import { QuotationFormComponent } from './quotation-form/quotation-form.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    QuotationListComponent,
    QuotationDetailComponent,
    QuotationFormComponent
  ]
})
export class QuotationModule { }
