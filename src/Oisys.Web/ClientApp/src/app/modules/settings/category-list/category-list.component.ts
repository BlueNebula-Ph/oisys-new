import { Component, ViewChild, AfterViewInit, ElementRef } from '@angular/core';

import { MatPaginator, MatTableDataSource, MatSort, MatInput } from '@angular/material';
import { CategoryService } from '../../../shared/services/category.service';

import { Observable, of, merge, fromEvent } from 'rxjs';
import { catchError, map, startWith, switchMap, debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { SummaryItem } from '../../../shared/models/summary-item';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements AfterViewInit {
  displayedColumns: string[] = ['name', 'buttons'];
  dataSource = new MatTableDataSource();
  categories: Observable<SummaryItem>;

  resultsLength = 0;
  isLoadingResults = false;

  constructor(private categoryService: CategoryService) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('searchBox') input: ElementRef;

  ngAfterViewInit() {
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
          return this.loadCategories();
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
  };

  loadCategories() {
    return this.categoryService.getCategories(
      this.paginator.pageIndex + 1, 
      this.paginator.pageSize, 
      this.sort.active, 
      this.sort.direction,
      this.input.nativeElement.value);
  };
}
