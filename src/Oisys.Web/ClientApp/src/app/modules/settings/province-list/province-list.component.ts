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
    this.loadProvinces();
  };

  loadProvinces() {
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
          return this.fetchProvinces();
        }),
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoadingResults = false;
          this.resultsLength = data.total_count;

          data.items.map((item) => {
            item.cityNames = item.cities.map((sc) => sc.name).join(', ');
          });

          return data.items;
        }),
        catchError(() => {
          this.isLoadingResults = false;

          return of([]);
        })
      )
      .subscribe(data => this.dataSource.data = data);
  }

  fetchProvinces() {
    return this.provinceService.getProvinces(
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction,
      this.input.nativeElement.value);
  };

  onEditProvince(provinceToEdit: Province): void {
    this.selectedProvince = provinceToEdit;
  };

  onDeleteProvince(id: number): void {
    if (confirm("Are you sure you want to delete this province?")) {
      this.provinceService.deleteProvince(id).subscribe(() => {
        this.loadProvinces();
        this.util.showSuccessMessage("Province deleted successfully.");
      });
    }
  };

  onProvinceSaved(province: Province): void {
    this.loadProvinces();
    this.util.showSuccessMessage("Province saved successfully.");
  };
}
