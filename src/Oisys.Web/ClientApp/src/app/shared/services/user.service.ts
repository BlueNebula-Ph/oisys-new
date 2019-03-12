import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';

import { UtilitiesService } from './utilities.service';
import { User } from '../models/user';
import { PagedData } from '../models/paged-data';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private url = `${environment.apiHost}api/user`;

  constructor(
    private http: HttpClient,
    private util: UtilitiesService
  ) { }

  getUsers(pageNumber: number, pageSize: number, sortBy: string, sortDirection: string, searchTerm: string): Observable<PagedData<User>> {
    var filter = { pageNumber, pageSize, sortBy, sortDirection, searchTerm };
    var searchUrl = `${this.url}/search`;

    return this.http.post<any>(searchUrl, filter)
      .pipe(
        catchError(this.util.handleError('getUsers', []))
      );
  };

  getUserById(id: number): Observable<User> {
    var getUrl = `${this.url}/${id}`;
    return this.http.get<User>(getUrl);
  };

  saveUser(user: User): Observable<User> {
    if (user.id == 0) {
      return this.http.post<User>(this.url, user, this.util.httpOptions);
    } else {
      var editUrl = `${this.url}/${user.id}`;
      return this.http.put<User>(editUrl, user, this.util.httpOptions);
    }
  };

  deleteUser(id: number): Observable<any> {
    var deleteUrl = `${this.url}/${id}`;
    return this.http.delete(deleteUrl, this.util.httpOptions);
  };
}
