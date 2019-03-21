import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { InventoryService } from '../../../shared/services/inventory.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { Item } from '../../../shared/models/item';
import { ItemHistory } from '../../../shared/models/item-history';
import { ItemOrderHistory } from '../../../shared/models/item-order-history';

import { PageBase } from '../../../shared/helpers/page-base';
import { Page } from '../../../shared/models/page';

@Component({
  selector: 'app-inventory-detail',
  templateUrl: './inventory-detail.component.html',
  styleUrls: ['./inventory-detail.component.css']
})
export class InventoryDetailComponent extends PageBase implements AfterContentInit {
  item$: Observable<Item>;

  itemHistory$: Observable<ItemHistory[]>;
  itemHistoryPage: Page = new Page();
  itemHistoryLoading = false;

  itemOrders$: Observable<ItemOrderHistory[]>;
  itemOrdersPage: Page = new Page();
  itemOrdersLoading = false;

  itemId: number;

  constructor(
    private inventoryService: InventoryService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);

    this.itemHistoryPage.pageNumber = 0;
    this.itemHistoryPage.size = 15;
    this.itemOrdersPage.pageNumber = 0;
    this.itemOrdersPage.size = 15;
  }

  ngAfterContentInit() {
    this.itemId = +this.route.snapshot.paramMap.get('id');
    this.loadItemDetails();
    this.loadOrderHistory();
    this.loadItemHistory();
  };

  loadItemDetails() {
    if (this.itemId && this.itemId != 0) {
      this.item$ = this.inventoryService
        .getItemById(this.itemId)
        .pipe(
          map(item => new Item(item))
        );
    }
  };

  loadItemHistory() {
    this.itemHistoryLoading = true;
    this.itemHistory$ = this.inventoryService
      .getItemHistory(this.itemId, this.itemHistoryPage.pageNumber, this.itemHistoryPage.size)
      .pipe(
        map(data => {
          this.itemHistoryPage = data.pageInfo;
          this.itemHistoryLoading = false;
          return data.items;
        })
      );
  };

  loadOrderHistory() {
    this.itemOrdersLoading = true;
    this.itemOrders$ = this.inventoryService
      .getItemOrderHistory(this.itemId, this.itemOrdersPage.pageNumber, this.itemOrdersPage.size)
      .pipe(
        map(data => {
          this.itemOrdersPage = data.pageInfo;
          this.itemOrdersLoading = false;
          return data.items;
        })
      );
  };

  setPage(pageInfo, type): void {
    if (type == "itemOrder") {
      this.itemOrdersPage.pageNumber = pageInfo.offset;
      this.loadOrderHistory();
    } else if (type == "itemHistory") {
      this.itemHistoryPage.pageNumber = pageInfo.offset;
      this.loadItemHistory();
    }
  };
}
