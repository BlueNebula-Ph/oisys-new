export class Category {
  constructor(public id?: number, public name?: string, public rowVersion?: string) {
    this.id = this.id || 0;
    this.name = this.name || '';
    this.rowVersion = this.rowVersion || '';
  };
}
