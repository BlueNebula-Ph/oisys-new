export class City {
  constructor(public id?: number, public name?: string, public rowVersion?: string) {
    this.id = id || 0;
    this.name = name || '';
    this.rowVersion = rowVersion || '';
  };
}
