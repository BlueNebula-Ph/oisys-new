import { Component, ViewChild, AfterViewInit, ElementRef } from '@angular/core';

import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { ProvinceService } from '../../../shared/services/province.service';

import { Observable, of, merge, fromEvent } from 'rxjs';
import { catchError, map, startWith, switchMap, debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { SummaryItem } from '../../../shared/models/summary-item';
import { Province } from '../../../shared/models/province';
import { UtilitiesService } from '../../../shared/services/utilities.service';

@Component({
  selector: 'app-province-list',
  templateUrl: './province-list.component.html',
  styleUrls: ['./province-list.component.css']
})
export class ProvinceListComponent implements AfterViewInit {
  displayedColumns: string[] = ['name', 'cities', 'buttons'];
  dataSource = new MatTableDataSource();
  categories: Observable<SummaryItem<Province>>;

  resultsLength = 0;
  isLoadingResults = false;

  selectedProvince: Province;

  constructor(private provinceService: ProvinceService, private util: UtilitiesService) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('searchBox') input: ElementRef;

  ngAfterViewInit() {
    loadProvinces();
  };

  loadProvinces() {
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
          return fetchProvinces();
        }),
        map(data => {
          // Flip flag to show that loading has finished.
          isLoadingResults = false;
          resultsLength = data.total_count;

          data.items.map((item) => {
            item.cityNames = item.cities.map((sc) => sc.name).join(', ');
          });

          return data.items;
        }),
        catchError(() => {
          isLoadingResults = false;

          return of([]);
        })
      )
      .subscribe(data => dataSource.data = data);
  }

  fetchProvinces() {
    return provinceService.getProvinces(
      paginator.pageIndex + 1,
      paginator.pageSize,
      sort.active,
      sort.direction,
      input.nativeElement.value);
  };

  onEditProvince(provinceToEdit: Province): void {
    selectedProvince = provinceToEdit;
  };

  onDeleteProvince(id: number): void {
    if (confirm("Are you sure you want to delete this province?")) {
      provinceService.deleteProvince(id).subscribe(() => {
        loadProvinces();
        util.openSnackBar("Province deleted successfully.");
      });
    }
  };

  onProvinceSaved(province: Province): void {
    loadProvinces();
    util.openSnackBar("Province saved successfully.");
  };
}
