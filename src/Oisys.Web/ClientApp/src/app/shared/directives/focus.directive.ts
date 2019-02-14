import { Directive, AfterContentInit, ElementRef, Input, OnChanges, SimpleChanges } from '@angular/core';

@Directive({
  selector: '[appFocus]',
  inputs: [
    "shouldFocusElement: appFocus",
  ]
})
export class FocusDirective implements AfterContentInit, OnChanges {
  private _shouldFocusElement: boolean;
  set shouldFocusElement(val: boolean) {
    console.log(val);
    this._shouldFocusElement = val;
    if (this._shouldFocusElement) {
      this.setFocus();
    }
  };

  public constructor(private el: ElementRef) {}

  public ngAfterContentInit() {
    this.setFocus();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    //console.log(changes);
    //if (changes) {
    //  if (changes.shouldFocusElement) {
    //    this.setFocus();
    //  }
    //}
  }

  private setFocus() {
    setTimeout(() => {
      this.el.nativeElement.focus();
      this._shouldFocusElement = false;
      //console.log(this.shouldFocusElement);
    }, 500);
  };
}
