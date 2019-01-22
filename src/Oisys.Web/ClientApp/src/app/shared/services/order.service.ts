import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

import { Order } from '../models/order';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private url = `${environment.apiHost}api/order`;

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getOrders(
    pageNumber: number,
    pageSize: number,
    sortBy: string,
    sortDirection: string,
    searchTerm: string,
    customerId: number = 0,
    provinceId: number = 0,
    itemId: number = 0,
    dateFrom?: Date,
    dateTo?: Date
  ): Observable<PagedData<Order>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, customerId, provinceId, itemId, dateFrom, dateTo };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getOrders', []))
      );
  };

  getOrderById(id: number): Observable<any> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<any>(getUrl);
  };

  getOrderLookup(): Observable<Order[]> {
    var lookupUrl = `${this.url}/lookup`;
    return this.http.get<Order[]>(lookupUrl);
  };

  saveOrder(order: Order): Observable<Order> {
    if (order.id == 0) {
      return this.http.post<Order>(this.url, order, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${order.id}`;
      return this.http.put<Order>(editUrl, order, this.util.httpOptions);
    }
  };

  deleteOrder(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
