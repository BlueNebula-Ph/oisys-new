import { Directive } from '@angular/core';

@Directive({
  selector: '[appInputGroupStyle]',
  host: {
    'class': 'input-group input-group-sm'
  }
})
export class InputGroupStyleDirective {
}
