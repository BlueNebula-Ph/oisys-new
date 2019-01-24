import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, forkJoin } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { SalesQuotationService } from '../../../shared/services/sales-quotation.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { InventoryService } from '../../../shared/services/inventory.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { SalesQuotation } from '../../../shared/models/sales-quotation';
import { Customer } from '../../../shared/models/customer';
import { Item } from '../../../shared/models/item';
import { LineItem } from '../../../shared/models/line-item';

@Component({
  selector: 'app-quotation-form',
  templateUrl: './quotation-form.component.html',
  styleUrls: ['./quotation-form.component.css']
})
export class QuotationFormComponent implements AfterContentInit {
  salesQuotation: SalesQuotation = new SalesQuotation();
  customers: Customer[];
  items: Item[];

  constructor(
    private salesQuotationService: SalesQuotationService,
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
    setTimeout(() => this.loadSalesQuotation(), 3000);
  };

  saveSalesQuotation(salesQuotationForm: NgForm) {
    if (salesQuotationForm.valid) {
      this.salesQuotationService
        .saveSalesQuotation(this.salesQuotation)
        .subscribe(() => {
          if (this.salesQuotation.id == 0) {
            this.loadSalesQuotation();
          }
          this.util.showSuccessMessage("Sales quotation saved successfully.");
        }, error => {
          console.error(error);
          this.util.showErrorMessage("An error occurred.");
        });
    }
  };

  loadSalesQuotation() {
    this.route.paramMap.subscribe(params => {
      var routeParam = params.get("id");
      var id = parseInt(routeParam);

      if (id == 0) {
        this.salesQuotation = new SalesQuotation();
      } else {
        this.salesQuotationService
          .getSalesQuotationById(id)
          .subscribe(salesQuotation => {
            this.salesQuotation = new SalesQuotation(salesQuotation);
            this.salesQuotation.lineItems = salesQuotation.lineItems.map(lineItem => {
              var salesQuotationLineItem = new LineItem(lineItem);
              salesQuotationLineItem.selectedItem = this.filterItems(lineItem.itemName)[0];
              return salesQuotationLineItem;
            });
            this.salesQuotation.selectedCustomer = this.filterCustomers(salesQuotation.customerName)[0];
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
    this.salesQuotation.lineItems.push(new LineItem());
    this.salesQuotation.updateLineItems();
  };

  removeLineItem(index: number) {
    if (confirm('Are you sure you want to remove this item?')) {
      this.salesQuotation.lineItems.splice(index, 1);
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
