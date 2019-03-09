import { JsonModelBase } from "./json-model-base";

export class User extends JsonModelBase {
  public id: number;
  public username: string;
  public password: string;
  public firstname: string;
  public lastname: string;
  public canWrite: boolean;
  public canView: boolean;
  public canDelete: boolean;

  private _admin: boolean;
  get admin() {
    return this._admin;
  }
  set admin(value: boolean) {
    if (value) {
      this.canView = value;
      this.canWrite = value;
      this.canDelete = value;
    }
    this._admin = value;
  }

  private _updatePassword: boolean;
  get updatePassword() {
    if (this.id == 0) {
      return true;
    }
    return this._updatePassword;
  }
  set updatePassword(value: boolean) {
    this._updatePassword = value;
  }

  get accessRights() {
    let access = new Array<string>();
    if (this.admin) {
      access.push('admin')
    }
    if (this.canView) {
      access.push('canView');
    }
    if (this.canWrite) {
      access.push('canWrite');
    }
    if (this.canDelete) {
      access.push('canDelete');
    }
    return access.join(',');
  }

  constructor();
  constructor(user: User);
  constructor(user ?: any) {
    super();

    this.id = user && user.id || 0;
    this.username = user && user.username || '';
    this.password = user && user.password || '';
    this.firstname = user && user.firstname || '';
    this.lastname = user && user.lastname || '';
    this.admin = user && user.admin || false;
    this.canWrite = user && user.canWrite || false;
    this.canView = user && user.canView || false;
    this.canDelete = user && user.canDelete || false;
  };
}
