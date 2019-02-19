import { Component, Input, Output, EventEmitter, ViewChild, ElementRef, AfterContentInit } from '@angular/core';

import { Search } from '../../models/search';
import { Province } from '../../models/province';
import { Item } from '../../models/item';
import { Customer } from '../../models/customer';
import { Category } from '../../models/category';

@Component({
  selector: 'app-search-panel',
  templateUrl: './search-panel.component.html',
  styleUrls: ['./search-panel.component.css']
})
export class SearchPanelComponent implements AfterContentInit {
  @Input() showProvinces: boolean = false;
  @Input() provinces: Province[] = new Array<Province>();

  @Input() showCities: boolean = false;

  @Input() showDates: boolean = false;

  @Input() showCategories: boolean = false;
  @Input() categories: Category[] = new Array<Category>();

  @Input() showItems: boolean = false;
  @Input() items: Item[] = new Array<Item>();

  @Input() showCustomers: boolean = false;
  @Input() customers: Customer[] = new Array<Customer>();

  @Output() searched = new EventEmitter<Search>();

  search: Search = new Search();

  @ViewChild("keywords") keywordsField: ElementRef;

  constructor() { }

  ngAfterContentInit() {
    this.keywordsField.nativeElement.focus();
  }

  performSearch(): void {
    this.searched.emit(this.search);
    this.keywordsField.nativeElement.focus();
  };

  clearSearch(): void {
    this.search = new Search();
    this.performSearch();
  };

  onKeyup(event) {
    if (event && event.keyCode == 13) {
      this.performSearch();
    }
  };
}
