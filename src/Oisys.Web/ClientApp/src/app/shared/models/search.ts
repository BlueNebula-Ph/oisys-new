import { Province } from "./province";
import { City } from "./city";
import { Item } from "./item";
import { Customer } from "./customer";
import { Category } from "./category";

export class Search {
  public searchTerm: string = '';
  public selectedProvince: Province = null;
  public selectedCity: City = null;
  public selectedItem: Item = null;
  public selectedCustomer: Customer = null;
  public selectedCategory: Category = null;
  public dateFrom: Date = null;
  public dateTo: Date = null;

  get provinceId(): number {
    if (this.selectedProvince) {
      return this.selectedProvince.id;
    }
    return 0;
  };

  get cityId(): number {
    if (this.selectedCity) {
      return this.selectedCity.id;
    }
    return 0;
  };

  get categoryId(): number {
    if (this.selectedCategory) {
      return this.selectedCategory.id;
    }
    return 0;
  }

  get customerId(): number {
    if (this.selectedCustomer) {
      return this.selectedCustomer.id;
    }
    return 0;
  }

  get itemId(): number {
    if (this.selectedItem) {
      return this.selectedItem.id;
    }
    return 0;
  }
}
