export class Item {
  constructor(
    public id?: number,
    public code?: string,
    public name?: string
  ) {
    this.id = this.id || 0;
  }
}
