import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceFormComponent } from './invoice-form/invoice-form.component';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { InvoiceDetailComponent } from './invoice-detail/invoice-detail.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [InvoiceFormComponent, InvoiceListComponent, InvoiceDetailComponent]
})
export class InvoiceModule { }
