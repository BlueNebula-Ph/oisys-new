import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';

import { UtilitiesService } from './utilities.service';
import { Category } from '../models/category';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private url = environment.apiHost + 'api/category';

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getCategories(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string): Observable<PagedData<Category>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
    var searchUrl = this.url + '/search';

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getCategories', []))
      );
  };

  getCategoryById(id: number): Observable<Category> {
    var getUrl = this.url + "/" + id;
    return this.http.get(getUrl);
  };

  getCategoryLookup(): Observable<Category[]> {
    var lookupUrl = this.url + "/lookup";
    return this.http.get<Category[]>(lookupUrl);
  };

  saveCategory(category: Category): Observable<Category> {
    if (category.id == 0) {
      return this.http.post(this.url, category, this.util.httpOptions);
    } else {
      var editUrl = this.url + '/' + category.id;
      return this.http.put(editUrl, category, this.util.httpOptions);
    }
  };

  deleteCategory(id: number): Observable<any> {
    var deleteUrl = this.url + "/" + id;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
