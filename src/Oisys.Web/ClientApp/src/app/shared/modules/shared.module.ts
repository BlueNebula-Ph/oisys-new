import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';

import { EnumToArrayPipe } from '../pipes/enum-to-array.pipe';
import { FocusDirective } from '../directives/focus.directive';
import { ControlStyleDirective } from '../directives/control-style.directive';
import { InputGroupStyleDirective } from '../directives/input-group-style.directive';
import { SearchPanelComponent } from '../components/search-panel/search-panel.component';


@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    RouterModule,
    FormsModule,
    NgxDatatableModule,
    NgbTypeaheadModule,
    CurrencyMaskModule
  ],
  declarations: [
    EnumToArrayPipe,
    FocusDirective,
    ControlStyleDirective,
    InputGroupStyleDirective,
    SearchPanelComponent
  ],
  exports: [
    EnumToArrayPipe,
    FocusDirective,
    ControlStyleDirective,
    InputGroupStyleDirective,
    SearchPanelComponent,
    BrowserAnimationsModule,
    RouterModule,
    NgxDatatableModule,
    NgbTypeaheadModule,
    CurrencyMaskModule
  ]
})
export class SharedModule {}
