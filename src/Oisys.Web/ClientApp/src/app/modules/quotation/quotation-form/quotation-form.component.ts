import { Component, AfterContentInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, forkJoin, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
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
export class QuotationFormComponent implements AfterContentInit, OnDestroy {
  salesQuotation: SalesQuotation = new SalesQuotation();
  getQuotationSub: Subscription;
  saveQuotationSub: Subscription;
  isSaving = false;

  @ViewChild('customer') customerField: ElementRef;

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
    this.loadQuotationForm();
  };

  ngOnDestroy() {
    if (this.getQuotationSub) { this.getQuotationSub.unsubscribe(); }
    if (this.saveQuotationSub) { this.saveQuotationSub.unsubscribe(); }
  };

  loadQuotationForm() {
    const quotationId = +this.route.snapshot.paramMap.get('id');
    if (quotationId && quotationId != 0) {
      this.loadQuotation(quotationId);
    } else {
      this.setQuotation(undefined);
    }
  };

  setQuotation(quotation: any) {
    this.salesQuotation = quotation ? new SalesQuotation(quotation) : new SalesQuotation();
    this.customerField.nativeElement.focus();
  };

  loadQuotation(id: number) {
    this.getQuotationSub = this.salesQuotationService
      .getSalesQuotationById(id)
      .subscribe(quotation => this.setQuotation(quotation));
  };

  saveSalesQuotation(salesQuotationForm: NgForm) {
    if (salesQuotationForm.valid) {
      this.isSaving = true;
      this.salesQuotationService
        .saveSalesQuotation(this.salesQuotation)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    if (this.salesQuotation.id == 0) {
      this.setQuotation(undefined);
    }
    this.util.showSuccessMessage('Sales quotation saved successfully.');
  };

  saveFailed = (error) => {
    this.util.showErrorMessage('An error occurred while saving. Please try again.');
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  // Line items
  addLineItem() {
    if (this.salesQuotation && this.salesQuotation.lineItems) {
      this.salesQuotation.lineItems.push(new LineItem());
      this.salesQuotation.updateLineItems();
    }
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
      switchMap(term => term.length < 2 ? [] :
        this.customerService.getCustomerLookup(0, 0, term)
          .pipe(
            map(customers => customers.splice(0, 10))
          )
      )
    );

  searchItem = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? [] :
        this.inventoryService.getItemLookup(term)
          .pipe(
            map(items => items.splice(0, 10))
          )
      )
    );

  customerFormatter = (x: { name: string }) => x.name;
  itemFormatter = (x: { name: string }) => x.name;
}
