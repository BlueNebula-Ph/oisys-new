import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { InventoryService } from '../../../shared/services/inventory.service';
import { CategoryService } from '../../../shared/services/category.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Item } from '../../../shared/models/item';
import { Category } from '../../../shared/models/category';

@Component({
  selector: 'app-inventory-form',
  templateUrl: './inventory-form.component.html',
  styleUrls: ['./inventory-form.component.css']
})
export class InventoryFormComponent implements AfterContentInit {
  item: Item = new Item();
  categories: Category[];

  constructor(private inventoryService: InventoryService, private categoryService: CategoryService, private util: UtilitiesService, private route: ActivatedRoute, private config: NgbTypeaheadConfig) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.fetchCategories();
    this.loadItem();
  };

  saveItem(itemForm: NgForm) {
    if (itemForm.valid) {
      this.inventoryService
        .saveItem(this.item)
        .subscribe(() => {
          if (this.item.id == 0) {
            this.loadItem();
          }
          this.util.showSuccessMessage("Item saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  loadItem() {
    this.route.paramMap.subscribe(params => {
      var routeParam = params.get("id");
      var id = parseInt(routeParam);

      if (id == 0) {
        this.item = new Item();
      } else {
        this.inventoryService
          .getItemById(id)
          .subscribe(item => {
            this.item = new Item(item);
            this.item.selectedCategory = this.filterCategories(item.categoryName)[0];
          });
      }
    });
  };

  fetchCategories() {
    this.categoryService.getCategoryLookup()
      .subscribe(results => {
        this.categories = results;
      });
  };

  // Autocomplete
  searchCategory = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? [] : this.filterCategories(term))
    );

  categoryFormatter = (x: { name: string }) => x.name;

  private filterCategories(value: string): Category[] {
    const filterValue = value.toLowerCase();

    return this.categories.filter(category => category.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }
}
