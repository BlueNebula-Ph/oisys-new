import { Component, AfterContentInit } from '@angular/core';
import { Observable } from 'rxjs';

import { ReportService } from '../../../shared/services/report.service';

import { OrderReport } from '../../../shared/models/order-report';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.css']
})
export class OrderSummaryComponent implements AfterContentInit {
  orders$: Observable<OrderReport[]>;

  dateFrom: Date;
  dateTo: Date;
  totalAmount: number = 0;

  constructor(
    private reportService: ReportService
  ) { }

  ngAfterContentInit() {
    this.loadReport();
  }

  loadReport() {
    this.totalAmount = 0;
    this.orders$ = this.reportService
      .getOrders(this.dateFrom, this.dateTo)
      .pipe(
        tap(orders => {
          orders.map(order => {
            this.totalAmount += order.totalAmount;
            return order;
          })
        })
      );
  };
}
