import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';

import { EnumToArrayPipe } from '../pipes/enum-to-array.pipe';
import { FocusDirective } from '../directives/focus.directive';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    RouterModule,
    NgxDatatableModule,
    NgbTypeaheadModule,
    CurrencyMaskModule
  ],
  declarations: [
    EnumToArrayPipe,
    FocusDirective
  ],
  exports: [
    EnumToArrayPipe,
    FocusDirective,
    BrowserAnimationsModule,
    RouterModule,
    NgxDatatableModule,
    NgbTypeaheadModule,
    CurrencyMaskModule
  ]
})
export class SharedModule {}
