import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

import { SalesQuotation } from '../models/sales-quotation';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class SalesQuotationService {
  private url = `${environment.apiHost}api/salesQuote`;

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getSalesQuotations(
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
  ): Observable<PagedData<SalesQuotation>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, customerId, provinceId, itemId, dateFrom, dateTo };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getSalesQuotations', []))
      );
  };

  getSalesQuotationById(id: number): Observable<any> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<any>(getUrl);
  };

  saveSalesQuotation(salesQuotation: SalesQuotation): Observable<SalesQuotation> {
    if (salesQuotation.id == 0) {
      return this.http.post<SalesQuotation>(this.url, salesQuotation, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${salesQuotation.id}`;
      return this.http.put<SalesQuotation>(editUrl, salesQuotation, this.util.httpOptions);
    }
  };

  deleteSalesQuotation(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
