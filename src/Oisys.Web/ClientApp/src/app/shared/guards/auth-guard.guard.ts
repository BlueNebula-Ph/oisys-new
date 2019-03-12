import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { AuthenticationService } from '../services/authentication.service';
import { UtilitiesService } from '../services/utilities.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthenticationService,
    private util: UtilitiesService
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ) {
    const currentUser = this.authService.currentUserValue;
    if (currentUser) {
      if (route.data.roles) {
        let userAccessRights = currentUser.accessRights.split(',');

        route.data.roles.forEach(role => {
          if (!userAccessRights.includes(role)) {
            this.util.showErrorMessage('Sorry, you do not have permissions to access this page.');
            this.router.navigate(['/']);
            return false;
          }
        });
      }

      // authorised so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
