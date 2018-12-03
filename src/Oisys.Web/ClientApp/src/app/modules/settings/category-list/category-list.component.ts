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
    this.loadCategories();
  };

  loadCategories() {
    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(
      this.sort.sortChange,
      this.paginator.page,
      fromEvent(this.input.nativeElement, 'keyup')
        .pipe(
          debounceTime(350),
          distinctUntilChanged(),
          tap(() => {
            this.paginator.pageIndex = 0;
          })
        )
    )
      .pipe(
        startWith({}),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.fetchCategories();
        }),
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoadingResults = false;
          this.resultsLength = data.total_count;

          return data.items;
        }),
        catchError(() => {
          this.isLoadingResults = false;

          return of([]);
        })
      )
      .subscribe(data => this.dataSource.data = data);
  }

  fetchCategories() {
    return this.categoryService.getCategories(
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction,
      this.input.nativeElement.value);
  };

  onEditCategory(categoryToEdit: Category): void {
    this.selectedCategory = categoryToEdit;
  };

  onDeleteCategory(id: number): void {
    if (confirm("Are you sure you want to delete this category?")) {
      this.categoryService.deleteCategory(id).subscribe(() => {
        this.loadCategories();
        this.util.openSnackBar("Category deleted successfully.");
      });
    }
  };

  onCategorySaved(category: Category): void {
    this.loadCategories();
    this.util.openSnackBar("Category saved successfully.");
  };
}
