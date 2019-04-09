import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";

import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

import { AuthenticationService } from "../services/authentication.service";
import { UtilitiesService } from "../services/utilities.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    private authService: AuthenticationService,
    private util: UtilitiesService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        catchError(err => {
          if ([401, 403].indexOf(err.status) !== -1) {
            // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
            this.authService.logout();
            location.reload(true);
          }

          if (err.status === 400 || err.status == 409 || err.status == 500) {
            this.util.showErrorMessage(err.error);
          }

          return throwError(err);
        }));
  }
}
