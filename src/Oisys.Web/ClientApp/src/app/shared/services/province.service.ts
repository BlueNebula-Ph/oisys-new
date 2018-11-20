import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { SummaryItem } from '../models/summary-item';
import { UtilitiesService } from './utilities.service';
import { Province } from '../models/Province';

@Injectable({
  providedIn: 'root'
})
export class ProvinceService {
  private url = environment.apiHost + 'api/province';

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getProvinces(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string): Observable<SummaryItem<Province>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
    var searchUrl = url + '/search';

    return http.post<any>(searchUrl, filter)
      .pipe(
        catchError(util.handleError('getProvinces', []))
      );
  };

  getProvinceById(id: number): Observable<Province> {
    var getUrl = url + "/" + id;
    return http.get<Province>(getUrl);
  };

  getProvinceLookup(): Observable<Province[]> {
    var lookupUrl = url + "/lookup";
    return http.get<Province[]>(lookupUrl);
  };

  saveProvince(province: Province): Observable<Province> {
    if (province.id == 0) {
      return http.post<Province>(url, province, util.httpOptions);
    } else {
      var editUrl = url + "/" + province.id;
      return http.put<Province>(editUrl, province, util.httpOptions);
    }
  };

  deleteProvince(id: number): Observable<any> {
    var deleteUrl = url + "/" + id;
    return http.delete(deleteUrl, util.httpOptions);
  };
}
