import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr'

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {
  public httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private toastrService: ToastrService) { }

  showSuccessMessage(message: string): void {
    this.toastrService.success(message, 'Success!');
  };

  showErrorMessage(message: string): void {
    if (!message || message == '') {
      message = "An error has occurred. Please contact your administrator.";
    }
    this.toastrService.error(message, 'Error!', { timeOut: 8000, enableHtml: true });
  };

  /**
  * Handle Http operation that failed.
  * Let the app continue.
  * @param operation - name of the operation that failed
  * @param result - optional value to return as the observable result
  */
  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  };
}
