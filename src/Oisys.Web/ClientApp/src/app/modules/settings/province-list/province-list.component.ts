import { Component, ViewChild, AfterContentInit, ElementRef, OnDestroy } from '@angular/core';

import { of, Observable, Subscription } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { ProvinceService } from '../../../shared/services/province.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Province } from '../../../shared/models/province';
import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';

@Component({
  selector: 'app-province-list',
  templateUrl: './province-list.component.html',
  styleUrls: ['./province-list.component.css']
})
export class ProvinceListComponent implements AfterContentInit, OnDestroy {
  page: Page = new Page();
  sort: Sort = new Sort();
  selectedProvince: Province;

  rows$: Observable<Province[]>;
  deleteProvinceSub: Subscription;

  isLoading: boolean = false;

  @ViewChild('searchBox') input: ElementRef;

  constructor(private provinceService: ProvinceService, private util: UtilitiesService) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'name';
    this.sort.dir = 'asc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.loadProvinces();
  };

  ngOnDestroy() {
    if (this.deleteProvinceSub) { this.deleteProvinceSub.unsubscribe(); }
  };

  loadProvinces() {
    this.isLoading = true;
    this.rows$ = this.provinceService.getProvinces(
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

          return data.items.map(prov => new Province(prov));
        }),
        catchError(() => {
          this.isLoading = false;

          return of([]);
        })
      );
  }

  onEditProvince(provinceToEdit: Province): void {
    this.selectedProvince = new Province(provinceToEdit);
  };

  onDeleteProvince(id: number): void {
    if (confirm("Are you sure you want to delete this province?")) {
      this.deleteProvinceSub = this.provinceService.deleteProvince(id).subscribe(() => {
        this.loadProvinces();
        this.util.showSuccessMessage("Province deleted successfully.");
      });
    }
  };

  onProvinceSaved(province: Province): void {
    this.loadProvinces();
  };

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadProvinces();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadProvinces();
    }
  }

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadProvinces();
  }

  onKeyup(event) {
    if (event && event.keyCode == 13) {
      this.search();
    }
  };
}
