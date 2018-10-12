import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Category } from '../models/category';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { catchError, map, tap } from 'rxjs/operators';
import { SummaryItem } from '../models/summary-item';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private url = environment.apiHost + 'api/category';

  constructor(private http: HttpClient) { }

  getCategories(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string):
    Observable<SummaryItem> {
      var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
      var searchUrl = this.url + '/search';

      return this.http.post<any>(searchUrl, filter)
        .pipe(
          catchError(this.handleError('getCategories', []))
        );
    };

  getCategoryById(id: number):
    Category {
      return new Category(0, '', '');
    };

  getCategoryLookup():
    Observable<Category[]> {
      return null;
    };
  
  saveCategory(category: Category):
    void {
      this.http.post<any>(this.url, category, httpOptions)
        .subscribe((result) => { console.log(result); });
    };

  deleteCategory(id: number):
    void {
      alert('category deleted!');
    }; 

  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
private handleError<T> (operation = 'operation', result?: T) {
  return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  };
}