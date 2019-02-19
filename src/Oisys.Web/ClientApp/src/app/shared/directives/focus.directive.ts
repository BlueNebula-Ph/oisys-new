import { Directive, AfterContentInit, Input, OnChanges, SimpleChanges, ElementRef } from '@angular/core';

@Directive({
  selector: '[appFocus]'
})
export class FocusDirective implements AfterContentInit, OnChanges {
  @Input() appFocus: boolean = false;

  public constructor(private el: ElementRef) { }

  public ngAfterContentInit() {
    this.setFocus();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes && changes.appFocus.currentValue) {
      this.setFocus();
    }
  }

  private setFocus() {
    setTimeout(() => {
      if (this.appFocus) {
        this.el.nativeElement.focus();
        this.el.nativeElement.select();
      }
    }, 20);
  };
}
