import { Component, AfterContentInit, ViewChild, OnDestroy, ElementRef  } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';

import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Adjustment } from '../../../shared/models/adjustment';
import { AdjustmentType } from '../../../shared/models/adjustment-type';
import { AdjustmentCategory } from '../../../shared/models/adjustment-category';

@Component({
  selector: 'app-adjust-item',
  templateUrl: './adjust-item.component.html',
  styleUrls: ['./adjust-item.component.css']
})
export class AdjustItemComponent implements AfterContentInit, OnDestroy {
  adjustment: Adjustment = new Adjustment();
  label: string = 'adjustment';
  adjustmentTypes = AdjustmentType;
  isSaving = false;

  saveAdjustmentSub: Subscription;

  @ViewChild('itemName') itemNameField: ElementRef;

  get isAdjustment() {
    return this.label == 'adjustment';
  }

  get isManufacturing() {
    return this.label == 'manufacturing';
  }

  constructor(
    private inventoryService: InventoryService,
    private util: UtilitiesService,
    private route: ActivatedRoute
  ) { }

  ngAfterContentInit() {
    this.label = this.route.snapshot.data.type;
    this.clearAdjustment();
  };

  ngOnDestroy() {
    if (this.saveAdjustmentSub) { this.saveAdjustmentSub.unsubscribe(); }
  };

  saveAdjustment(adjustmentForm: NgForm) {
    if (adjustmentForm.valid) {
      this.isSaving = true;
      this.saveAdjustmentSub = this.inventoryService
        .saveItemAdjustment(this.adjustment)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    this.clearAdjustment();
    this.util.showSuccessMessage('Adjustment saved successfully.');
  };

  saveFailed = (error) => {
    this.util.showErrorMessage('An error occurred while saving. Please try again.');
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  clearAdjustment() {
    this.adjustment = new Adjustment();
    this.adjustment.category = this.isAdjustment ? AdjustmentCategory.Adjustment : AdjustmentCategory.Manufacture;
    this.itemNameField.nativeElement.focus();
  };

  // Autocomplete
  searchItem = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? [] :
        this.inventoryService.getItemLookup(term)
          .pipe(
            map(provinces => provinces.splice(0, 10))
        )
      )
    );

  itemFormatter = (x: { code: string, name: string }) => `${x.code} - ${x.name}`;
}
