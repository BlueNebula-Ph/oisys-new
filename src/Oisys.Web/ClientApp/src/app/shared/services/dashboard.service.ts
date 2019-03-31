import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import { UtilitiesService } from './utilities.service';
import { DashboardOrder } from '../models/dashboard-order';


@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private url = `${environment.apiHost}api/dashboard`;

  constructor(
    private http: HttpClient,
    private util: UtilitiesService
  ) { }

  getRecentOrders(days: number): Observable<number> {
    const url = `${this.url}/orders/${days}`;
    return this.http.get<number>(url);
  };

  getRecentSales(days: number): Observable<number> {
    const url = `${this.url}/sales/${days}`;
    return this.http.get<number>(url);
  };

  getOrdersDue(): Observable<number> {
    const url = `${this.url}/orders-due`;
    return this.http.get<number>(url);
  };

  getLowQuantityItems(): Observable<number> {
    const url = `${this.url}/low-quantity`;
    return this.http.get<number>(url);
  };

  getOrdersBoard(): Observable<DashboardOrder> {
    const url = `${this.url}/order-list`;
    return this.http.get<DashboardOrder>(url);
  };
}
