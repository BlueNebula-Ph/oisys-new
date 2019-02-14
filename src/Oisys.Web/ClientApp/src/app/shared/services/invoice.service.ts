import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

import { Invoice } from '../models/invoice';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private url = `${environment.apiHost}api/invoice`;

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getInvoices(
    pageNumber: number,
    pageSize: number,
    sortBy: string,
    sortDirection: string,
    searchTerm: string,
    customerId: number = 0,
    dateFrom?: Date,
    dateTo?: Date
  ): Observable<PagedData<Invoice>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, customerId, dateFrom, dateTo };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getInvoices', []))
      );
  };

  getInvoiceById(id: number): Observable<any> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<any>(getUrl);
  };

  saveInvoice(invoice: Invoice): Observable<Invoice> {
    if (invoice.id == 0) {
      return this.http.post<Invoice>(this.url, invoice, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${invoice.id}`;
      return this.http.put<Invoice>(editUrl, invoice, this.util.httpOptions);
    }
  };

  deleteInvoice(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
