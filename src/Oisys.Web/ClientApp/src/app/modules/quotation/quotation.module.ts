import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuotationListComponent } from './quotation-list/quotation-list.component';
import { QuotationDetailComponent } from './quotation-detail/quotation-detail.component';
import { QuotationFormComponent } from './quotation-form/quotation-form.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [QuotationListComponent, QuotationDetailComponent, QuotationFormComponent]
})
export class QuotationModule { }
