export class Category {
  public id: number;
  public name: string;
  public rowVersion: string;

  constructor()
  constructor(category: Category)
  constructor(category?: any) {
    this.id = category && category.id || 0;
    this.name = category && category.name || '';
    this.rowVersion = category && category.rowVersion || '';
  };
}
