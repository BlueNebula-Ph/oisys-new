import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Item } from '../models/item';
import { UtilitiesService } from './utilities.service';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private url = environment.apiHost + 'api/item';

  constructor(private http: HttpClient, private util: UtilitiesService) { }

  getItems(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string, categoryId: number = 0): Observable<PagedData<Item>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm, categoryId };
    var searchUrl = this.url + '/search';

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getItems', []))
      );
  };

  getItemById(id: number): Observable<Item> {
    var getUrl = this.url + "/" + id;
    return this.http.get<Item>(getUrl);
  };

  getItemLookup(): Observable<Item[]> {
    var lookupUrl = this.url + "/lookup";
    return this.http.get<Item[]>(lookupUrl);
  };

  saveItem(item: Item): Observable<any> {
    if (item.id == 0) {
      return this.http.post<Item>(this.url, item, this.util.httpOptions);
    } else {
      var editUrl = this.url + "/" + item.id;
      return this.http.put<Item>(editUrl, item, this.util.httpOptions);
    }
  };

  deleteItem(id: number): Observable<any> {
    var deleteUrl = this.url + "/" + id;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}