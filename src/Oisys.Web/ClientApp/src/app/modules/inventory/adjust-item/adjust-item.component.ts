import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Adjustment } from '../../../shared/models/adjustment';
import { Item } from '../../../shared/models/item';
import { AdjustmentType } from '../../../shared/models/adjustment-type';

@Component({
  selector: 'app-adjust-item',
  templateUrl: './adjust-item.component.html',
  styleUrls: ['./adjust-item.component.css']
})
export class AdjustItemComponent implements AfterContentInit {
  adjustment: Adjustment = new Adjustment();
  label: string = 'adjustment';
  adjustmentTypes = AdjustmentType;
  items: Item[];

  get isAdjustment() {
    return this.label == 'adjustment';
  }

  get isManufacturing() {
    return this.label == 'manufacturing';
  }

  constructor(private inventoryService: InventoryService, private util: UtilitiesService, private route: ActivatedRoute) {
  }

  ngAfterContentInit() {
    this.getRouteData();
    this.fetchItems();
  }

  getRouteData() {
    this.route.data.subscribe(data => this.label = data.type);
  };

  fetchItems() {
    this.inventoryService.getItemLookup()
      .subscribe(items => this.items = items);
  };

  saveAdjustment(adjustmentForm: NgForm) {
    if (!adjustmentForm.valid) {
      console.log(adjustmentForm.errors);
    }

    this.inventoryService
      .saveItemAdjustment(this.adjustment)
      .subscribe(() => {
        this.clearAdjustment();
        this.util.showSuccessMessage('Adjustment saved successfully.');
      }, () => {
        this.util.showErrorMessage('An error has occurred.');
      });
  };

  clearAdjustment() {
    this.adjustment = new Adjustment();
  };

  // Autocomplete
  searchItem = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => this.filterItems(term))
    );

  itemFormatter = (x: { name: string }) => x.name;

  private filterItems(value: string): Item[] {
    const filterValue = value.toLowerCase();

    return this.items.filter(item => item.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }
}
