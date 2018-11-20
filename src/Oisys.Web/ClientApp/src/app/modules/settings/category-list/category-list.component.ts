import { Component, ViewChild, AfterViewInit, ElementRef } from '@angular/core';

import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { CategoryService } from '../../../shared/services/category.service';

import { Observable, of, merge, fromEvent } from 'rxjs';
import { catchError, map, startWith, switchMap, debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { SummaryItem } from '../../../shared/models/summary-item';
import { Category } from '../../../shared/models/category';
import { UtilitiesService } from '../../../shared/services/utilities.service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements AfterViewInit {
  displayedColumns: string[] = ['name', 'buttons'];
  dataSource = new MatTableDataSource();
  categories: Observable<SummaryItem<Category>>;

  resultsLength = 0;
  isLoadingResults = false;

  selectedCategory: Category;

  constructor(private categoryService: CategoryService, private util: UtilitiesService) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('searchBox') input: ElementRef;

  ngAfterViewInit() {
    loadCategories();
  };

  loadCategories() {
    // If the user changes the sort order, reset back to the first page.
    sort.sortChange.subscribe(() => paginator.pageIndex = 0);

    merge(
      sort.sortChange,
      paginator.page,
      fromEvent(input.nativeElement, 'keyup')
        .pipe(
          debounceTime(350),
          distinctUntilChanged(),
          tap(() => {
            paginator.pageIndex = 0;
          })
        )
    )
      .pipe(
        startWith({}),
        switchMap(() => {
          isLoadingResults = true;
          return fetchCategories();
        }),
        map(data => {
          // Flip flag to show that loading has finished.
          isLoadingResults = false;
          resultsLength = data.total_count;

          return data.items;
        }),
        catchError(() => {
          isLoadingResults = false;

          return of([]);
        })
      )
      .subscribe(data => dataSource.data = data);
  }

  fetchCategories() {
    return categoryService.getCategories(
      paginator.pageIndex + 1,
      paginator.pageSize,
      sort.active,
      sort.direction,
      input.nativeElement.value);
  };

  onEditCategory(categoryToEdit: Category): void {
    selectedCategory = categoryToEdit;
  };

  onDeleteCategory(id: number): void {
    if (confirm("Are you sure you want to delete this category?")) {
      categoryService.deleteCategory(id).subscribe(() => {
        loadCategories();
        util.openSnackBar("Category deleted successfully.");
      });
    }
  };

  onCategorySaved(category: Category): void {
    loadCategories();
    util.openSnackBar("Category saved successfully.");
  };
}
