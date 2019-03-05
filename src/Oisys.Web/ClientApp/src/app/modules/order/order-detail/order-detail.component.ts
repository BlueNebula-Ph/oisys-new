import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { Order } from '../../../shared/models/order';
import { OrderService } from '../../../shared/services/order.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent implements AfterContentInit {
  order$: Observable<Order>;

  constructor(
    private orderService: OrderService,
    private route: ActivatedRoute
  ) { }

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
