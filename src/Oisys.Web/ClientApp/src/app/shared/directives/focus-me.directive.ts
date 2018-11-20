import { Directive, AfterViewInit, ElementRef } from '@angular/core';

@Directive({
  selector: '[appFocusMe]'
})
export class FocusMeDirective implements AfterViewInit {

  constructor(private el: ElementRef) {}

  ngAfterViewInit() {
    setTimeout(() => { el.nativeElement.focus(); }, 100);
  };
}
