import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { CategoryService } from '../../../shared/services/category.service'
import { UtilitiesService } from '../../../shared/services/utilities.service';
import { Category } from '../../../shared/models/category';
import { isUndefined } from 'util';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {
  @Input() category: Category;
  @Output() onCategorySaved: EventEmitter<Category> = new EventEmitter<Category>();

  isSaving: boolean = false;

  constructor(private categoryService: CategoryService, private util: UtilitiesService) { }

  ngOnInit() {
    this.clearCategory();
  }

  saveCategory(categoryForm: NgForm) {
    if (categoryForm.valid) {
      this.isSaving = true;
      this.categoryService
        .saveCategory(this.category)
        .pipe(
          catchError(error => {
            this.isSaving = false;
            this.util.handleError('saveCategory', error);
            this.util.showErrorMessage("An error occurred while saving category. Please try again.");
            return of(undefined);
          })
        )
        .subscribe(result => {
          if (!isUndefined(result)) {
            this.isSaving = false;
            this.clearCategory();
            this.onCategorySaved.emit(result);

            categoryForm.resetForm();
          }
        });
    }
  }

  clearCategory() {
    this.category = new Category();
  }
}
