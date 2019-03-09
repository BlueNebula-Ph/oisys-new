import { Component, AfterContentInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';

import { Observable, Subscription, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { UtilitiesService } from '../../../shared/services/utilities.service';
import { UserService } from '../../../shared/services/user.service';

import { Page } from '../../../shared/models/page';
import { Sort } from '../../../shared/models/sort';
import { User } from '../../../shared/models/user';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements AfterContentInit, OnDestroy {
  page: Page = new Page();
  sort: Sort = new Sort();
  selectedUser: User;

  rows$: Observable<User[]>;
  deleteUserSub: Subscription;

  isLoading: boolean = false;

  @ViewChild('searchBox') input: ElementRef;

  constructor(
    private userService: UserService,
    private util: UtilitiesService
  ) {
    this.page.pageNumber = 0;
    this.page.size = 20;
    this.sort.prop = 'username';
    this.sort.dir = 'asc';
  }

  ngAfterContentInit() {
    this.setPage({ offset: 0 });
    this.loadUsers();
  };

  ngOnDestroy() {
    if (this.deleteUserSub) { this.deleteUserSub.unsubscribe(); }
  };

  loadUsers() {
    this.isLoading = true;
    this.rows$ = this.userService.getUsers(
      this.page.pageNumber,
      this.page.size,
      this.sort.prop,
      this.sort.dir,
      this.input.nativeElement.value)
      .pipe(
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoading = false;
          this.page = data.pageInfo;

          return data.items.map(user => new User(user));
        }),
        catchError(() => {
          this.isLoading = false;

          return of([]);
        })
      );
  }

  onEditUser(userToEdit: User): void {
    this.selectedUser = new User(userToEdit);
  };

  onDeleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.deleteUserSub = this.userService.deleteUser(id).subscribe(() => {
        this.loadUsers();
        this.util.showSuccessMessage('User deleted successfully.');
      });
    }
  };

  onUserSaved(user: User): void {
    this.loadUsers();
  };

  setPage(pageInfo): void {
    this.page.pageNumber = pageInfo.offset;
    this.loadUsers();
  }

  onSort(event) {
    if (event) {
      // On sort change, update to 1st page.
      this.page.pageNumber = 0;
      this.sort = event.sorts[0];
      this.loadUsers();
    }
  }

  search() {
    // Reset page number on search.
    this.page.pageNumber = 0;
    this.loadUsers();
  }

  onKeyup(event) {
    if (event && event.keyCode == 13) {
      this.search();
    }
  };
}
