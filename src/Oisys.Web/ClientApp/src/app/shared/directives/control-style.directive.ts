import { Directive } from '@angular/core';

@Directive({
  selector: '[appControlStyle]',
  host: {
    'class': 'form-control form-control-sm'
  }
})
export class ControlStyleDirective {
}
