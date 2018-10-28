import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EnumToArrayPipe } from '../pipes/enum-to-array.pipe';
import { FocusMeDirective } from '../directives/focus-me.directive';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [EnumToArrayPipe, FocusMeDirective],
  exports: [EnumToArrayPipe, FocusMeDirective]
})
export class SharedModule {}
