import { Component, ViewChild, AfterViewInit, ElementRef, AfterContentInit } from '@angular/core';

import { of } from 'rxjs';
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
export class CategoryListComponent implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  rows = new Array<Category>();

  isLoading: boolean = false;

  selectedCategory: Category;

  constructor(private categoryService: CategoryService, private util: UtilitiesService) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'name';
    this.sort.dir = 'asc';
  }

  @ViewChild('searchBox') input: ElementRef;

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.loadCategories();
  };

  loadCategories() {
    this.isLoading = true;
    this.categoryService.getCategories(
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
      )
      .subscribe(data => this.rows = data);
  }

  onEditCategory(categoryToEdit: Category): void {
    this.selectedCategory = categoryToEdit;
  };

  onDeleteCategory(id: number): void {
    if (confirm("Are you sure you want to delete this category?")) {
      this.categoryService.deleteCategory(id).subscribe(() => {
        this.loadCategories();
        this.util.showSuccessMessage("Category deleted successfully.");
      });
    }
  };

  onCategorySaved(category: Category): void {
    this.loadCategories();
    this.util.showSuccessMessage("Category saved successfully.");
  };

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadCategories();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadCategories();
    }
  }

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadCategories();
  }
}
