import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CustomerService } from '../../../shared/services/customer.service';
import { Customer } from '../../../shared/models/customer';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements AfterContentInit {
  customer$: Observable<Customer>;

  constructor(
    private customerService: CustomerService,
    private route: ActivatedRoute
  ) { }

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
