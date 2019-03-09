import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';

import { UtilitiesService } from './utilities.service';
import { Province } from '../models/province';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class ProvinceService {
  private url = `${environment.apiHost}api/province`;

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getProvinces(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string): Observable<PagedData<Province>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getProvinces', []))
      );
  };

  getProvinceById(id: number): Observable<Province> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<Province>(getUrl);
  };

  getProvinceLookup(name: string = ''): Observable<Province[]> {
    var lookupUrl = `${this.url}/lookup`;

    if (name !== '') {
      lookupUrl = lookupUrl.concat(`/${name}`);
    }

    return this.http.get<Province[]>(lookupUrl);
  };

  saveProvince(province: Province): Observable<Province> {
    if (province.id == 0) {
      return this.http.post<Province>(this.url, province, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${province.id}`;
      return this.http.put<Province>(editUrl, province, this.util.httpOptions);
    }
  };

  deleteProvince(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
