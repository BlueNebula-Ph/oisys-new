import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

import { CashVoucher } from '../models/cash-voucher';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class CashVoucherService {
  private url = `${environment.apiHost}api/cashVoucher`;

  constructor(
    private http: HttpClient,
    private util: UtilitiesService
  ) { }

  getCashVouchers(
    pageNumber: number,
    pageSize: number,
    sortBy: string,
    sortDirection: string,
    searchTerm: string,
    dateFrom?: Date,
    dateTo?: Date
  ): Observable<PagedData<CashVoucher>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, dateFrom, dateTo };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getCashVouchers', []))
      );
  };

  getCashVoucherById(id: number): Observable<any> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<any>(getUrl);
  };

  saveCashVoucher(order: CashVoucher): Observable<CashVoucher> {
    if (order.id == 0) {
      return this.http.post<CashVoucher>(this.url, order, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${order.id}`;
      return this.http.put<CashVoucher>(editUrl, order, this.util.httpOptions);
    }
  };

  deleteCashVoucher(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
