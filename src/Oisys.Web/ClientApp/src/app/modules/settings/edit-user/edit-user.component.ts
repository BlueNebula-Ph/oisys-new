import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Subscription } from 'rxjs';

import { UserService } from '../../../shared/services/user.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';
import { User } from '../../../shared/models/user';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {
  private _user: User;
  @Input()
  get user() {
    return this._user;
  };
  set user(value) {
    if (value) {
      this._user = value;
      this.userNameField.nativeElement.focus();
    }
  };

  @Output() onUserSaved: EventEmitter<User> = new EventEmitter<User>();

  saveUserSub: Subscription;
  isSaving: boolean = false;

  @ViewChild('username') userNameField: ElementRef;
  @ViewChild('pword') passwordField: ElementRef;

  constructor(
    private userService: UserService,
    private util: UtilitiesService
  ) { }

  ngOnInit() {
    this.clearUser();
  };

  ngOnDestroy() {
    if (this.saveUserSub) { this.saveUserSub.unsubscribe(); }
  };

  saveUser(userForm: NgForm) {
    if (userForm.valid) {
      this.isSaving = true;
      this.saveUserSub = this.userService
        .saveUser(this.user)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = (result) => {
    this.clearUser();
    this.util.showSuccessMessage('User saved successfully.');
    this.userNameField.nativeElement.focus();
    this.onUserSaved.emit(result);
  };

  saveFailed = (error) => {
    this.util.showErrorMessage('An error occurred while saving. Please try again.');
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  clearUser() {
    this.user = new User();
  };

  togglePassword(event) {
    if (event) {
      setTimeout(() => {
        this.passwordField.nativeElement.focus();
      }, 50);
    }
  };
}
