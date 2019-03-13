export class PageBase {
    constructor(authenticationService) {
        this.authenticationService = authenticationService;
    }
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
}
//# sourceMappingURL=page-base.js.map