import { Item } from "./item";
import { PriceList } from "./price-list";

export class OrderLineItem {
  public id: number;
  public quantity: number;
  public itemId: number;

  public priceList: PriceList;

  private _selectedItem: Item;
  get selectedItem() {
    return this._selectedItem;
  }
  set selectedItem(item: Item) {
    if (item) {
      this._selectedItem = item;
      this.itemId = item.id;
    }
  }

  get unit() {
    return this._selectedItem && this._selectedItem.unit || '';
  }

  get categoryName() {
    return this._selectedItem && this._selectedItem.categoryName || '';
  }

  get unitPrice() {
    var unitPrice = 0;
    if (this._selectedItem) {
      switch (this.priceList) {
        case PriceList["Main Price"]:
          unitPrice = this._selectedItem.mainPrice;
          break;
        case PriceList["Walk-In Price"]:
          unitPrice = this._selectedItem.walkInPrice;
          break;
        case PriceList["N.E. Price"]:
          unitPrice = this._selectedItem.nePrice;
          break;
      }
    }
    return unitPrice;
  }

  get totalPrice() {
    return this.quantity * this.unitPrice;
  }

  constructor();
  constructor(orderLineItem: OrderLineItem);
  constructor(orderLineItem?: any) {
    this.id = orderLineItem && orderLineItem.id || 0;
    this.quantity = orderLineItem && orderLineItem.quantity || 0;
    this.itemId = orderLineItem && orderLineItem.itemId || 0;

    this.priceList = orderLineItem && orderLineItem.priceList || PriceList["Main Price"];
  }

  updatePriceList(priceList?: PriceList) {
    this.priceList = priceList || PriceList["Main Price"];
  };
}
