import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumToArray'
})
export class EnumToArrayPipe implements PipeTransform {
  transform(data: Object) {
    const keys = Object.keys(data);
    var result = new Array();
    keys.forEach((key) => {
      result.push({ key: key, value: data[key] });
    });
    return result.splice(keys.length / 2);
  }
}
