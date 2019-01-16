import { Component, OnInit } from '@angular/core';
import { Customer } from '../../../shared/models/customer';
import { ActivatedRoute } from '@angular/router';
import { CustomerService } from '../../../shared/services/customer.service';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements OnInit {
  customer: Customer = new Customer();

  constructor(private route: ActivatedRoute, private customerService: CustomerService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      var id = params.get("id");
      this.customerService
        .getCustomerById(parseInt(id))
        .subscribe(result => {
          this.customer = result;
        });
    });
  }
}
