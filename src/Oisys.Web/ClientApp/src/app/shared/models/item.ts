import { Category } from "./category";

export class Item {
  public id: number;
  public code: string;
  public name: string;
  public description: string;
  public categoryId: number;
  public categoryName: string;
  public quantity: number;
  public unit: string;
  public weight: string;
  public thickness: string;
  public mainPrice: number;
  public walkInPrice: number;
  public nePrice: number;

  private _selectedCategory: Category;
  get selectedCategory() {
    return this._selectedCategory;
  }
  set selectedCategory(category: Category) {
    if (category) {
      this._selectedCategory = category;
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

    this.categoryId = item && item.categoryId || 0;
    this.categoryName = item && item.categoryName || '';

    this.quantity = item && item.quantity || 0;
    this.unit = item && item.unit || '';
    this.weight = item && item.weight || '';
    this.thickness = item && item.thickness || '';

    this.mainPrice = item && item.mainPrice || 0;
    this.walkInPrice = item && item.walkInPrice || 0;
    this.nePrice = item && item.nePrice || 0;
  }
}
