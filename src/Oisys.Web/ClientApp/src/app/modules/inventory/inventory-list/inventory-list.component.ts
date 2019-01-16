import { Component, AfterContentInit } from '@angular/core';
import { Router } from '@angular/router';

import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { InventoryService } from '../../../shared/services/inventory.service';
import { CategoryService } from '../../../shared/services/category.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Item } from '../../../shared/models/item';
import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { Category } from '../../../shared/models/category';

@Component({
  selector: 'app-inventory-list',
  templateUrl: './inventory-list.component.html',
  styleUrls: ['./inventory-list.component.css']
})
export class InventoryListComponent implements AfterContentInit {
  page: Page = new Page();
  sort: Sort = new Sort();
  rows = new Array<Item>();

  searchTerm: string = '';
  selectedCategory: Category = new Category();
  categories: Category[];

  isLoading: boolean = false;

  constructor(private inventoryService: InventoryService, private categoryService: CategoryService, private util: UtilitiesService, private router: Router) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'name';
    this.sort.dir = 'asc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.fetchCategories();
    this.loadItems();
  };

  loadItems() {
    this.isLoading = true;
    this.inventoryService.getItems(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.searchTerm,
      this.selectedCategory.id)
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

  fetchCategories() {
    this.categoryService.getCategoryLookup()
      .subscribe(results => {
        this.categories = results;
      });
  };

  addItem(id: number): void {
    var url = "/items/edit/" + id;
    this.router.navigateByUrl(url);
  };

  onDeleteItem(id: number): void {
    if (confirm("Are you sure you want to delete this item?")) {
      this.inventoryService.deleteItem(id).subscribe(() => {
        this.loadItems();
        this.util.showSuccessMessage("Item deleted successfully.");
      });
    }
  }

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadItems();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadItems();
    }
  }

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadItems();
  }

  clear() {
    this.searchTerm = '';
    this.selectedCategory = new Category();
  }
}
