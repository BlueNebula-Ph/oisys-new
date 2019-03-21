import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CustomerService } from '../../../shared/services/customer.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { Customer } from '../../../shared/models/customer';
import { CustomerTransaction } from '../../../shared/models/customer-transaction';

import { PageBase } from '../../../shared/helpers/page-base';
import { Page } from '../../../shared/models/page';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent extends PageBase implements AfterContentInit {
  customer$: Observable<Customer>;

  transactions$: Observable<CustomerTransaction[]>;
  transactionsPage: Page = new Page();
  transactionsLoading = false;

  customerId: number;

  constructor(
    private customerService: CustomerService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);

    this.transactionsPage.pageNumber = 0;
    this.transactionsPage.size = 20;
  }

  ngAfterContentInit() {
    this.customerId = +this.route.snapshot.paramMap.get('id');
    this.loadCustomerDetails();
    this.loadCustomerTransactions();
  };

  loadCustomerDetails() {
    this.customer$ = this.customerService
      .getCustomerById(this.customerId)
      .pipe(
        map(customer => new Customer(customer))
      );
  };

  loadCustomerTransactions() {
    this.transactionsLoading = true;
    this.transactions$ = this.customerService
      .getTransactions(this.customerId, this.transactionsPage.pageNumber, this.transactionsPage.size)
      .pipe(
        map(data => {
          this.transactionsPage = data.pageInfo;
          this.transactionsLoading = false;
          return data.items;
        })
      );
  };

  setPage(pageInfo): void {
    this.transactionsPage.pageNumber = pageInfo.offset;
    this.loadCustomerTransactions();
  };
}
