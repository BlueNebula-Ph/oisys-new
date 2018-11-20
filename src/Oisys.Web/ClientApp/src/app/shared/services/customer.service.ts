import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Customer } from '../models/customer';
import { SummaryItem } from '../models/summary-item';
import { UtilitiesService } from './utilities.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private url = environment.apiHost + 'api/customer';

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getCustomers(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string): Observable<SummaryItem<Customer>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
    var searchUrl = url + '/search';

    return http.post<any>(searchUrl, filter)
      .pipe(
        catchError(util.handleError('getCustomers', []))
      );
  };

  getCustomerById(id: number): Observable<Customer> {
    var getUrl = url + "/" + id;
    return http.get<Customer>(getUrl);
  };

  getCustomerLookup(): Observable<Customer[]> {
    var lookupUrl = url + "/lookup";
    return http.get<Customer[]>(lookupUrl);
  };

  saveCustomer(customer: Customer): Observable<Customer> {
    if (customer.id == 0) {
      return http.post<Customer>(url, customer, util.httpOptions);
    } else {
      var editUrl = url + "/" + customer.id;
      return http.put<Customer>(editUrl, customer, util.httpOptions);
    }
  };

  deleteCustomer(id: number): Observable<any> {
    var deleteUrl = url + "/" + id;
    return http.delete(deleteUrl, util.httpOptions);
  };
}
