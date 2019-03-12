import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { LoggedUser } from '../models/logged-user';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private url = `${environment.apiHost}api/auth/token`;
  private readonly currentUserKey = 'currentUser';

  private currentUserSubject: BehaviorSubject<LoggedUser>;
  public currentUser: Observable<LoggedUser>;

  get currentUserValue(): LoggedUser {
    return this.currentUserSubject.value;
  }

  constructor(
    private http: HttpClient
  ) {
    this.currentUserSubject = new BehaviorSubject<LoggedUser>(JSON.parse(localStorage.getItem(this.currentUserKey)));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  login(username: string, password: string) {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded'
      })
    };

    let body = `username=${username}&password=${password}`;

    return this.http.post<any>(this.url, body, httpOptions)
      .pipe(
        map(user => {
          if (user && user.token) {
            localStorage.setItem(this.currentUserKey, JSON.stringify(user));
            this.currentUserSubject.next(user);
          }

          return user;
        })
      );
  }

  logout() {
    localStorage.removeItem(this.currentUserKey);
    this.currentUserSubject.next(null);
  }
}
