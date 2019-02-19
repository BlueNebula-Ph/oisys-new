import { Component, ViewChild, AfterContentInit, OnDestroy, ElementRef } from '@angular/core';

import { of, Observable, Subscription } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { CategoryService } from '../../../shared/services/category.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Category } from '../../../shared/models/category';
import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements AfterContentInit, OnDestroy {
  page: Page = new Page();
  sort: Sort = new Sort();
  selectedCategory: Category;

  rows$: Observable<Category[]>;
  deleteCategorySub: Subscription;

  isLoading: boolean = false;

  @ViewChild('searchBox') input: ElementRef;

  constructor(private categoryService: CategoryService, private util: UtilitiesService) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'name';
    this.sort.dir = 'asc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.loadCategories();
    this.input.nativeElement.focus();
  };

  ngOnDestroy() {
    if (this.deleteCategorySub) { this.deleteCategorySub.unsubscribe(); }
  };

  loadCategories() {
    this.isLoading = true;
    this.rows$ = this.categoryService.getCategories(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.input.nativeElement.value)
      .pipe(
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoading = false;

          this.page = data.pageInfo;

          return data.items;
        }),
        catchError(() => {
          this.isLoading = false;

          return of([]);
        })
      );
  }

  onEditCategory(categoryToEdit: Category): void {
    this.selectedCategory = categoryToEdit;
  };

  onDeleteCategory(id: number): void {
    if (confirm("Are you sure you want to delete this category?")) {
      this.deleteCategorySub = this.categoryService.deleteCategory(id).subscribe(() => {
        this.loadCategories();
        this.util.showSuccessMessage("Category deleted successfully.");
      });
    }
  };

  onCategorySaved(category: Category): void {
    this.loadCategories();
  };

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadCategories();
  };

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadCategories();
    }
  };

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadCategories();
  };

  onKeyup(event) {
    if (event && event.keyCode == 13) {
      this.search();
    }
  };
}
