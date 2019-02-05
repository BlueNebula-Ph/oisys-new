import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

import { Delivery } from '../models/delivery';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {
  private url = `${environment.apiHost}api/delivery`;

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getDeliveries(
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
  ): Observable<PagedData<Delivery>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, customerId, provinceId, itemId, dateFrom, dateTo };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getDeliveries', []))
      );
  };

  getDeliveryById(id: number): Observable<any> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<any>(getUrl);
  };

  saveDelivery(delivery: Delivery): Observable<Delivery> {
    if (delivery.id == 0) {
      return this.http.post<Delivery>(this.url, delivery, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${delivery.id}`;
      return this.http.put<Delivery>(editUrl, delivery, this.util.httpOptions);
    }
  };

  deleteDelivery(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
