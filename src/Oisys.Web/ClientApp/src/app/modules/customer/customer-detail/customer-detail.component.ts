import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CustomerService } from '../../../shared/services/customer.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { Customer } from '../../../shared/models/customer';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent extends PageBase implements AfterContentInit {
  customer$: Observable<Customer>;

  constructor(
    private customerService: CustomerService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);
  }

  ngAfterContentInit() {
    this.loadCustomerDetails();
  };

  loadCustomerDetails() {
    const customerId = +this.route.snapshot.paramMap.get('id');
    this.customer$ = this.customerService
      .getCustomerById(customerId)
      .pipe(
        map(customer => new Customer(customer))
      );
  };
}
