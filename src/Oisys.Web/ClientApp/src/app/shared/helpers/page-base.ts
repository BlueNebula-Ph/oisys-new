import { AuthenticationService } from "../services/authentication.service";

export class PageBase {

  get isAdmin() {
    console.log(this.authenticationService.currentUserValue.admin);
    return this.authenticationService.currentUserValue &&
      this.authenticationService.currentUserValue.admin;
  }

  get canWrite() {
    return this.authenticationService.currentUserValue &&
      this.authenticationService.currentUserValue.canWrite;
  }

  get canDelete() {
    return this.authenticationService.currentUserValue &&
      this.authenticationService.currentUserValue.canDelete;
  }

  constructor(
    private authenticationService: AuthenticationService
  ) { }
}
