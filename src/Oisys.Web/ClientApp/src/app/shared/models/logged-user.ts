export class LoggedUser {
  public username: string;
  public fullname: string;
  public canWrite: boolean;
  public canView: boolean;
  public canDelete: boolean;
  public admin: boolean;
  public token: string;
  public accessRights: string;

  constructor();
  constructor(user: LoggedUser);
  constructor(user?: any) {
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
