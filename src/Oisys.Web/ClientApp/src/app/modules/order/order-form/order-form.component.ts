import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, forkJoin } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { OrderService } from '../../../shared/services/order.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Order } from '../../../shared/models/order';
import { Customer } from '../../../shared/models/customer';
import { Item } from '../../../shared/models/item';
import { LineItem } from '../../../shared/models/line-item';

@Component({
  selector: 'app-order-form',
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.css']
})
export class OrderFormComponent implements AfterContentInit {
  order: Order = new Order();
  customers: Customer[];
  items: Item[];

  constructor(
    private orderService: OrderService,
    private customerService: CustomerService,
    private inventoryService: InventoryService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.fetchLists();
    setTimeout(() => this.loadOrder(), 3000);
  };

  saveOrder(orderForm: NgForm) {
    if (orderForm.valid) {
      this.orderService
        .saveOrder(this.order)
        .subscribe(() => {
          if (this.order.id == 0) {
            this.loadOrder();
          }
          this.util.showSuccessMessage("Order saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  loadOrder() {
    this.route.paramMap.subscribe(params => {
      var routeParam = params.get("id");
      var id = parseInt(routeParam);

      if (id == 0) {
        this.order = new Order();
      } else {
        this.orderService
          .getOrderById(id)
          .subscribe(order => {
            this.order = new Order(order);
            this.order.lineItems = order.lineItems.map(lineItem => {
              var orderLineItem = new LineItem(lineItem);
              orderLineItem.selectedItem = this.filterItems(lineItem.itemName)[0];
              return orderLineItem;
            });
            this.order.selectedCustomer = this.filterCustomers(order.customerName)[0];
          });
      }
    });
  };

  fetchLists() {
    forkJoin(
      this.customerService.getCustomerLookup(),
      this.inventoryService.getItemLookup()
    ).subscribe(([customerResponse, inventoryResponse]) => {
      this.customers = customerResponse;
      this.items = inventoryResponse;
    });
  };

  // Line items
  addLineItem() {
    this.order.lineItems.push(new LineItem());
    this.order.updateLineItems();
  };

  removeLineItem(index: number) {
    if(confirm('Are you sure you want to remove this item?')) {
      this.order.lineItems.splice(index, 1);
    }
  };

  // Autocomplete
  searchCustomer = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? [] : this.filterCustomers(term))
    );

  searchItem = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => this.filterItems(term))
    );

  customerFormatter = (x: { name: string }) => x.name;
  itemFormatter = (x: { name: string }) => x.name;

  private filterCustomers(value: string): Customer[] {
    const filterValue = value.toLowerCase();

    return this.customers.filter(customer => customer.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }

  private filterItems(value: string): Item[] {
    const filterValue = value.toLowerCase();

    return this.items.filter(item => item.name.toLowerCase().startsWith(filterValue)).splice(0, 10);
  }
}
