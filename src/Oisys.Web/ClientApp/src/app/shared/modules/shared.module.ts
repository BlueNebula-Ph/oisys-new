import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { TypeaheadModule } from 'ngx-bootstrap';

import { EnumToArrayPipe } from '../pipes/enum-to-array.pipe';
import { FocusMeDirective } from '../directives/focus-me.directive';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    NgxDatatableModule,
    TypeaheadModule
  ],
  declarations: [
    EnumToArrayPipe,
    FocusMeDirective
  ],
  exports: [
    EnumToArrayPipe,
    FocusMeDirective,
    BrowserAnimationsModule,
    NgxDatatableModule,
    TypeaheadModule
  ]
})
export class SharedModule {}
