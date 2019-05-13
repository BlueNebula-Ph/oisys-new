import { Category } from "./category";

export class Item {
  public id: number;
  public code: string;
  public name: string;
  public description: string;
  public categoryId: number;
  public categoryName: string;
  public quantity: number;
  public stockQuantity: number;
  public unit: string;
  public weight: string;
  public thickness: string;
  public mainPrice: number;
  public walkInPrice: number;
  public nePrice: number;
  public rowVersion: string;

  private _category: Category;
  get category() {
    return this._category;
  }
  set category(category: Category) {
    if (category) {
      this._category = category;
      this.categoryId = category.id;
    }
  }

  constructor();
  constructor(item: Item);
  constructor(item?: any) {
    this.id = item && item.id || 0;
    this.code = item && item.code || '';
    this.name = item && item.name || '';
    this.description = item && item.description || '';
    this.rowVersion = item && item.rowVersion || '';

    this.categoryId = item && item.categoryId || 0;
    this.categoryName = item && item.categoryName || '';

    this.quantity = item && item.quantity || 0;
    this.stockQuantity = item && item.stockQuantity || 0;
    this.unit = item && item.unit || '';
    this.weight = item && item.weight || '';
    this.thickness = item && item.thickness || '';

    this.mainPrice = item && item.mainPrice || 0;
    this.walkInPrice = item && item.walkInPrice || 0;
    this.nePrice = item && item.nePrice || 0;

    this.category = (item && item.category) ? new Category(item.category) : undefined;
  }
}
