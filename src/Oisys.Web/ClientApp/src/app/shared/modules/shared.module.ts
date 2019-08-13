import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxPrintModule } from 'ngx-print';

import { EnumToArrayPipe } from '../pipes/enum-to-array.pipe';
import { NoValuePipe } from '../pipes/no-value.pipe';

import { FocusDirective } from '../directives/focus.directive';
import { ControlStyleDirective } from '../directives/control-style.directive';
import { InputGroupStyleDirective } from '../directives/input-group-style.directive';
import { SearchPanelComponent } from '../components/search-panel/search-panel.component';
import { LogoComponent } from '../components/logo/logo.component';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    RouterModule,
    FormsModule,
    NgxDatatableModule,
    NgbTypeaheadModule,
    CurrencyMaskModule,
    NgxPrintModule
  ],
  declarations: [
    EnumToArrayPipe,
    NoValuePipe,
    FocusDirective,
    ControlStyleDirective,
    InputGroupStyleDirective,
    SearchPanelComponent,
    LogoComponent
  ],
  exports: [
    EnumToArrayPipe,
    NoValuePipe,
    FocusDirective,
    ControlStyleDirective,
    InputGroupStyleDirective,
    SearchPanelComponent,
    LogoComponent,
    BrowserAnimationsModule,
    RouterModule,
    NgxDatatableModule,
    NgbTypeaheadModule,
    CurrencyMaskModule,
    NgxPrintModule
  ]
})
export class SharedModule {}
