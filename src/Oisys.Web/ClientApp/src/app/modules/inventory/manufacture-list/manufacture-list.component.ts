import { AfterContentInit, Component } from '@angular/core';

import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AuthenticationService } from '../../../shared/services/authentication.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Adjustment } from '../../../shared/models/adjustment';
import { Item } from '../../../shared/models/item';
import { Page } from '../../../shared/models/page';
import { Search } from '../../../shared/models/search';
import { Sort } from '../../../shared/models/sort';
import { AdjustmentCategory } from '../../../shared/models/adjustment-category';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-manufacture-list',
  templateUrl: './manufacture-list.component.html',
  styleUrls: ['./manufacture-list.component.css']
})
export class ManufactureListComponent extends PageBase implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  search: Search = new Search();

  rows$: Observable<Adjustment[]>;
  items$: Observable<Item[]>;

  isLoading: boolean = false;

  constructor(
    private inventoryService: InventoryService,
    private authService: AuthenticationService,
    private util: UtilitiesService
  ) {
    super(authService);

    this.page.pageNumber = 0;
    this.page.size = 50;
    this.sort.prop = 'id';
    this.sort.dir = 'asc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.fetchItems();

    this.loadAdjustments();
  };

  loadAdjustments() {
    this.isLoading = true;
    this.rows$ = this.inventoryService.getItemAdjustments(
      AdjustmentCategory.Manufacture,
      this.page.pageNumber,
      this.page.size,
      this.search.itemId)
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
  };

  fetchItems() {
    this.items$ = this.inventoryService.getItemLookup();
  };

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadAdjustments();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadAdjustments();
    }
  }

  onSearch(event) {
    if (event) {
      // Reset page number on search.
      this.page.pageNumber = 0;
      this.search = event;
      this.loadAdjustments();
    }
  }
}
