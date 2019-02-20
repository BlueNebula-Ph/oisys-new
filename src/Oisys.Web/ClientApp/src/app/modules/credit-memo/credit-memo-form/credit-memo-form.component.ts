import { Component, AfterContentInit, ViewChild, OnDestroy, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

import { CreditMemoService } from '../../../shared/services/credit-memo.service';
import { OrderService } from '../../../shared/services/order.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { CreditMemo } from '../../../shared/models/credit-memo';
import { CreditMemoLineItem } from '../../../shared/models/credit-memo-line-item';

@Component({
  selector: 'app-credit-memo-form',
  templateUrl: './credit-memo-form.component.html',
  styleUrls: ['./credit-memo-form.component.css']
})
export class CreditMemoFormComponent implements AfterContentInit, OnDestroy {
  creditMemo: CreditMemo = new CreditMemo();
  getCreditMemoSub: Subscription;
  saveCreditMemoSub: Subscription;
  isSaving = false;

  @ViewChild('customer') customerField: ElementRef;

  constructor(
    private creditMemoService: CreditMemoService,
    private orderService: OrderService,
    private customerService: CustomerService,
    private util: UtilitiesService,
    private route: ActivatedRoute,
    private config: NgbTypeaheadConfig
  ) {
    this.config.showHint = true;
  }

  ngAfterContentInit() {
    this.loadCreditMemoForm();
  };

  ngOnDestroy() {
    if (this.getCreditMemoSub) { this.getCreditMemoSub.unsubscribe(); }
    if (this.saveCreditMemoSub) { this.saveCreditMemoSub.unsubscribe(); }
  };

  loadCreditMemoForm() {
    const creditMemoId = +this.route.snapshot.paramMap.get('id');
    if (creditMemoId && creditMemoId != 0) {
      this.loadCreditMemo(creditMemoId);
    } else {
      this.setCreditMemo(undefined);
    }
  };

  setCreditMemo(creditMemo: any) {
    this.creditMemo = creditMemo ? new CreditMemo(creditMemo) : new CreditMemo();
    this.customerField.nativeElement.focus();
  };

  loadCreditMemo(id: number) {
    this.getCreditMemoSub = this.creditMemoService
      .getCreditMemoById(id)
      .subscribe(creditMemo => this.setCreditMemo(creditMemo));
  }

  saveCreditMemo(creditMemoForm: NgForm) {
    if (creditMemoForm.valid) {
      this.isSaving = true;
      this.creditMemoService
        .saveCreditMemo(this.creditMemo)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = () => {
    if (this.creditMemo.id == 0) {
      this.setCreditMemo(undefined);
    }
    this.util.showSuccessMessage('Credit memo saved successfully.');
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
    if (this.creditMemo && this.creditMemo.lineItems) {
      this.creditMemo.lineItems.push(new CreditMemoLineItem());
    }
  };

  removeLineItem(index: number) {
    if (confirm('Are you sure you want to remove this item?')) {
      this.creditMemo.lineItems.splice(index, 1);
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
        this.orderService.getOrderLineItemLookup(this.creditMemo.customerId, false, term)
          .pipe(
            map(orders => orders.splice(0, 10))
          )
      )
    );

  customerFormatter = (x: { name: string }) => x.name;
  itemFormatter = (x: { itemName: string, orderCode: string }) => `${x.itemName} - Order # ${x.orderCode}`;
}
