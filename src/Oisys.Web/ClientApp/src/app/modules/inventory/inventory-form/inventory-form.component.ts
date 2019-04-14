import { Component, AfterContentInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { InventoryService } from '../../../shared/services/inventory.service';
import { CategoryService } from '../../../shared/services/category.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Item } from '../../../shared/models/item';

@Component({
  selector: 'app-inventory-form',
  templateUrl: './inventory-form.component.html',
  styleUrls: ['./inventory-form.component.css']
})
export class InventoryFormComponent implements AfterContentInit, OnDestroy {
  item = new Item();
  getItemSub: Subscription;
  saveItemSub: Subscription;
  isSaving = false;
  isQuantityDisabled = false;

  @ViewChild('itemCode') codeField: ElementRef;

  constructor(
    private inventoryService: InventoryService,
    private categoryService: CategoryService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.loadItemForm();
  };

  ngOnDestroy() {
    if (this.getItemSub) { this.getItemSub.unsubscribe(); }
    if (this.saveItemSub) { this.saveItemSub.unsubscribe(); }
  };

  loadItemForm() {
    const itemId = +this.route.snapshot.paramMap.get('id');
    if (itemId && itemId != 0) {
      this.loadItem(itemId);
      this.isQuantityDisabled = true;
    } else {
      this.setItem(undefined);
    }
  };

  setItem(item: any) {
    this.item = item ? new Item(item) : new Item();
    this.codeField.nativeElement.focus();
  };

  loadItem(id: number) {
    this.getItemSub = this.inventoryService
      .getItemById(id)
      .subscribe(item => this.setItem(item));
  }

  saveItem(itemForm: NgForm) {
    if (itemForm.valid) {
      this.isSaving = true;
      this.inventoryService
        .saveItem(this.item)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    this.loadItemForm();
    this.util.showSuccessMessage('Item saved successfully.');
  };

  saveFailed = (error) => {
    this.isSaving = false;
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  // Autocomplete
  searchCategory = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? [] :
        this.categoryService.getCategoryLookup(term)
          .pipe(
            map(categories => categories.splice(0, 10))
          )
      )
    );

  categoryFormatter = (x: { name: string }) => x.name;
}
