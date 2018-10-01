import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreditMemoDetailComponent } from './credit-memo-detail/credit-memo-detail.component';
import { CreditMemoListComponent } from './credit-memo-list/credit-memo-list.component';
import { CreditMemoFormComponent } from './credit-memo-form/credit-memo-form.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [CreditMemoDetailComponent, CreditMemoListComponent, CreditMemoFormComponent]
})
export class CreditMemoModule { }
