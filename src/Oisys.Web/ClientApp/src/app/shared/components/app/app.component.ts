import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { navItems } from '../../../nav';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnDestroy {
  public navItems = navItems;
  public sidebarMinimized = true;
  private changes: MutationObserver;
  public element: HTMLElement = document.body;

  get isLoggedIn() {
    return this.authService.currentUserValue;
  }

  get fullname() {
    return this.authService.currentUserValue.fullname;
  }

  get username() {
    return this.authService.currentUserValue.username;
  }

  constructor(
    private authService: AuthenticationService,
    private router: Router
  ) {
    this.changes = new MutationObserver((mutations) => {
      this.sidebarMinimized = document.body.classList.contains('sidebar-minimized');
    });

    this.changes.observe(<Element>this.element, {
      attributes: true,
      attributeFilter: ['class']
    });
  }

  ngOnDestroy(): void {
    this.changes.disconnect();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  };
}
