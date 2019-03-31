import { AfterContentInit, Component } from '@angular/core';
import { Observable } from 'rxjs';

import { DashboardService } from '../../services/dashboard.service';
import { DashboardOrder } from '../../models/dashboard-order';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements AfterContentInit {
  ordersCount$: Observable<number>;
  orderCountDays = 7;

  salesValue$: Observable<number>;
  salesDays = 7;

  ordersDue$: Observable<number>;
  lowQuantityItems$: Observable<number>;

  ordersBoard$: Observable<DashboardOrder>;

  constructor(
    private dashboardService: DashboardService
  ) { }

  ngAfterContentInit() {
    this.loadOrdersCount();
    this.loadSales();
    this.loadOrdersDue();
    this.loadLowQuantity();
    this.loadOrdersBoard();
  };

  loadOrdersCount() {
    this.ordersCount$ = this.dashboardService.getRecentOrders(this.orderCountDays);
  };

  orderCountUpdated(newOrderCountDays) {
    this.orderCountDays = newOrderCountDays;
    this.loadOrdersCount();
  }

  loadSales() {
    this.salesValue$ = this.dashboardService.getRecentSales(this.salesDays);
  };

  salesDaysUpdated(newSalesDays) {
    this.salesDays = newSalesDays;
    this.loadSales();
  };

  loadOrdersDue() {
    this.ordersDue$ = this.dashboardService.getOrdersDue();
  };

  loadLowQuantity() {
    this.lowQuantityItems$ = this.dashboardService.getLowQuantityItems();
  };

  loadOrdersBoard() {
    this.ordersBoard$ = this.dashboardService.getOrdersBoard();
  };
}
