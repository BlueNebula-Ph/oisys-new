import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { catchError, map, tap } from 'rxjs/operators';
import { SummaryItem } from '../models/summary-item';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private url = environment.apiHost + 'api/category/search';

  constructor(private http: HttpClient) { }

  getCategories(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string):
    Observable<SummaryItem> {
      var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
      return this.http.post<any>(this.url, filter)
        .pipe(
          catchError(this.handleError('getCategories', []))
        );
    };

  getCategoryById(id: number):
    Category {
      return { id: 0, name: "", rowVersion: "" };
    };
  
  saveCategory():
    void {
      alert('category saved!');
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