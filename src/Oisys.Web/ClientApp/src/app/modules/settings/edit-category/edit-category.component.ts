import { Component, OnInit, Input, Output, ViewChild, EventEmitter, ElementRef, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Subscription } from 'rxjs';

import { CategoryService } from '../../../shared/services/category.service'
import { UtilitiesService } from '../../../shared/services/utilities.service';
import { Category } from '../../../shared/models/category';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy {
  private _category: Category;
  @Input()
  get category() {
    return this._category;
  };
  set category(value) {
    if (value) {
      this._category = value;
      this.categoryNameField.nativeElement.focus();
    }
  };

  @Output() onCategorySaved: EventEmitter<Category> = new EventEmitter<Category>();

  saveCategorySub: Subscription;
  isSaving: boolean = false;

  @ViewChild('categoryName') categoryNameField: ElementRef;

  constructor(
    private categoryService: CategoryService,
    private util: UtilitiesService
  ) { }

  ngOnInit() {
    this.clearCategory();
  };

  ngOnDestroy() {
    if (this.saveCategorySub) { this.saveCategorySub.unsubscribe(); }
  };

  saveCategory(categoryForm: NgForm) {
    if (categoryForm.valid) {
      this.isSaving = true;
      this.saveCategorySub = this.categoryService
        .saveCategory(this.category)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = (result) => {
    this.clearCategory();
    this.util.showSuccessMessage("Category saved successfully.");
    this.categoryNameField.nativeElement.focus();
    this.onCategorySaved.emit(result);
  };

  saveFailed = (error) => {
    this.util.showErrorMessage("An error occurred while saving. Please try again.");
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  clearCategory() {
    this.category = new Category();
  };
}
