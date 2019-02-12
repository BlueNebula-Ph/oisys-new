import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

import { CreditMemo } from '../models/credit-memo';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class CreditMemoService {
  private url = `${environment.apiHost}api/creditmemo`;

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getCreditMemos(
    pageNumber: number,
    pageSize: number,
    sortBy: string,
    sortDirection: string,
    searchTerm: string,
    customerId: number = 0,
    itemId: number = 0,
    dateFrom?: Date,
    dateTo?: Date
  ): Observable<PagedData<CreditMemo>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, customerId, itemId, dateFrom, dateTo };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getCreditMemos', []))
      );
  };

  getCreditMemoById(id: number): Observable<any> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<any>(getUrl);
  };

  getCreditMemoLookup(customerId: number, isInvoiced: boolean = false): Observable<CreditMemo[]> {
    var lookupUrl = `${this.url}/${customerId}/lookup/${isInvoiced}`;
    return this.http.get<CreditMemo[]>(lookupUrl);
  };

  saveCreditMemo(creditMemo: CreditMemo): Observable<CreditMemo> {
    if (creditMemo.id == 0) {
      return this.http.post<CreditMemo>(this.url, creditMemo, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${creditMemo.id}`;
      return this.http.put<CreditMemo>(editUrl, creditMemo, this.util.httpOptions);
    }
  };

  deleteCreditMemo(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
