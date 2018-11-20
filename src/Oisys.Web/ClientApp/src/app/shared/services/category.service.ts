import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { SummaryItem } from '../models/summary-item';
import { UtilitiesService } from './utilities.service';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private url = environment.apiHost + 'api/category';

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getCategories(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string): Observable<SummaryItem<Category>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
    var searchUrl = url + '/search';

    return http.post<any>(searchUrl, filter)
      .pipe(
        catchError(util.handleError('getCategories', []))
      );
  };

  getCategoryById(id: number): Observable<Category> {
    var getUrl = url + "/" + id;
    return http.get(getUrl);
  };

  getCategoryLookup(): Observable<Category[]> {
    var lookupUrl = url + "/lookup";
    return http.get<Category[]>(lookupUrl);
  };

  saveCategory(category: Category): Observable<Category> {
    if (category.id == 0) {
      return http.post(url, category, util.httpOptions);
    } else {
      var editUrl = url + "/" + category.id;
      return http.put(editUrl, category, util.httpOptions);
    }
  };

  deleteCategory(id: number): Observable<any> {
    var deleteUrl = url + "/" + id;
    return http.delete(deleteUrl, util.httpOptions);
  };
}
