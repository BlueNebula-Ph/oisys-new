import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../../../shared/services/customer.service';
import { Customer } from '../../../shared/models/customer';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements OnInit {
  customer: Customer = new Customer();

  constructor(private customerService: CustomerService) { }

  ngOnInit() {
    this.loadCustomer();
  }

  saveCustomer(customerForm: NgForm) {
    if (customerForm.valid) {
      this.customerService.saveCustomer(this.customer)
        .subscribe(result => {
          this.loadCustomer();
          customerForm.resetForm();
        });
    }
  };

  loadCustomer() {
    this.customer = new Customer();
  };

}
