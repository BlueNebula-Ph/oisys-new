import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import { UtilitiesService } from './utilities.service';
import { ItemCount } from '../models/item-count';
import { OrderReport } from '../models/order-report';
import { ProductSales } from '../models/product-sales';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private url = `${environment.apiHost}api/report`;

  constructor(
    private http: HttpClient,
    private util: UtilitiesService
  ) { }

  getCountSheet(): Observable<ItemCount[]> {
    var url = `${this.url}/count-sheet`;
    return this.http.get<ItemCount[]>(url);
  };

  getOrders(dateFrom?: Date, dateTo?: Date): Observable<OrderReport[]> {
    var url = `${this.url}/orders`;

    if (dateFrom && dateTo) {
      url += `/${dateFrom}/${dateTo}`;
    }

    return this.http.get<OrderReport[]>(url);
  };

  getProductSales(dateFrom?: Date, dateTo?: Date): Observable<ProductSales[]> {
    var url = `${this.url}/product-sales`;

    if (dateFrom && dateTo) {
      url += `/${dateFrom}/${dateTo}`;
    }

    return this.http.get<ProductSales[]>(url);
  };
}
