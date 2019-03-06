import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { InventoryService } from '../../../shared/services/inventory.service';
import { Item } from '../../../shared/models/item';

@Component({
  selector: 'app-inventory-detail',
  templateUrl: './inventory-detail.component.html',
  styleUrls: ['./inventory-detail.component.css']
})
export class InventoryDetailComponent implements AfterContentInit {
  item$: Observable<Item>;

  constructor(
    private inventoryService: InventoryService,
    private route: ActivatedRoute
  ) { }

  ngAfterContentInit() {
    this.loadItemDetails();
  };

  loadItemDetails() {
    const itemId = +this.route.snapshot.paramMap.get('id');
    if (itemId && itemId != 0) {
      this.item$ = this.inventoryService
        .getItemById(itemId)
        .pipe(
          map(item => new Item(item))
        );
    }
  };
}
