import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { OrderService } from '../../../shared/services/order.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { Order } from '../../../shared/models/order';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent extends PageBase implements AfterContentInit {
  order$: Observable<Order>;

  constructor(
    private orderService: OrderService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);
  }

  ngAfterContentInit() {
    this.loadOrderDetails();
  };

  loadOrderDetails() {
    const orderId = +this.route.snapshot.paramMap.get('id');
    if (orderId && orderId != 0) {
      this.order$ = this.orderService
        .getOrderById(orderId)
        .pipe(
          map(order => new Order(order))
        );
    }
  };
}
