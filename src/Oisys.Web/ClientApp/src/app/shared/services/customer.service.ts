import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Customer } from '../models/customer';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private url = environment.apiHost + 'api/customer';

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getCustomers(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string, provinceId: number = 0, cityId: number = 0): Observable<PagedData<Customer>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, provinceId, cityId };
    var searchUrl = this.url + '/search';

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getCustomers', []))
      );
  };

  getCustomerById(id: number): Observable<any> {
    var getUrl = this.url + "/" + id;
    return this.http.get<any>(getUrl);
  };

  getCustomerLookup(): Observable<Customer[]> {
    var lookupUrl = this.url + "/lookup";
    return this.http.get<Customer[]>(lookupUrl);
  };

  saveCustomer(customer: Customer): Observable<Customer> {
    if (customer.id == 0) {
      return this.http.post<Customer>(this.url, customer, this.util.httpOptions);
    } else {
      var editUrl = this.url + "/" + customer.id;
      return this.http.put<Customer>(editUrl, customer, this.util.httpOptions);
    }
  };

  deleteCustomer(id: number): Observable<any> {
    var deleteUrl = this.url + "/" + id;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
