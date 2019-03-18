import { Component, OnDestroy, OnInit, ViewChild, ElementRef, AfterContentInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { Subscription } from 'rxjs';

import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterContentInit, OnDestroy {
  username: string;
  password: string;
  error: string;

  loginSubscription: Subscription;

  isLoading = false;
  defaultUrl = '/'
  returnUrl: string;

  @ViewChild('usernameBox') usernameField: ElementRef;

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    if (this.authService.currentUserValue) {
      this.router.navigate([this.defaultUrl]);
      location.reload();
    }
  }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  ngAfterContentInit() {
    this.focusDefault();
  }

  ngOnDestroy() {
    if (this.loginSubscription) { this.loginSubscription.unsubscribe(); }
  }

  onLogin(loginForm: NgForm) {
    if (loginForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.loginSubscription = this.authService
      .login(this.username, this.password)
      .subscribe(
        data => {
          this.router.navigate([this.returnUrl]);
        },
        error => {
          this.isLoading = false;
          this.focusDefault();
        }
      );
  };

  private focusDefault() {
    setTimeout(() => { this.usernameField.nativeElement.focus(); }, 100);
  };
}
