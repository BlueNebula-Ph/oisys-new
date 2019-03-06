import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'noValue'
})
export class NoValuePipe implements PipeTransform {
  transform(value: string): string {
    if (!value || value == '') {
      return '-';
    }
    return value;
  }
}
