export class LoggedUser {
    constructor(user) {
        this.username = user && user.username || '';
        this.fullname = user && user.fullname || '';
        this.admin = user && user.admin || false;
        this.canDelete = user && user.canDelete || false;
        this.canView = user && user.canView || false;
        this.canWrite = user && user.canWrite || false;
        this.accessRights = user && user.accessRights || '';
        this.token = user && user.token || '';
    }
}
//# sourceMappingURL=logged-user.js.map