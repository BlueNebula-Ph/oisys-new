import { ModelBase } from "./model-base";
import { Item } from "./item";
import { PriceList } from "./price-list";

export class OrderLineItem extends ModelBase {
  public id: number;
  public quantity: number;
  public itemId: number;
  public itemName: string;

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

  private _unitPrice: number;
  get unitPrice() {
    if (this._unitPrice && this._unitPrice != 0) {
      return this._unitPrice;
    }

    var unitPrice = 0;
    if (this._selectedItem && this.itemId != 0) {
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
  set unitPrice(value: number) {
    this._unitPrice = value;
  }

  get totalPrice() {
    return this.quantity * this.unitPrice;
  }

  constructor();
  constructor(orderLineItem: OrderLineItem);
  constructor(orderLineItem?: any) {
    super();

    this.id = orderLineItem && orderLineItem.id || 0;
    this.quantity = orderLineItem && orderLineItem.quantity || 0;
    this.itemId = orderLineItem && orderLineItem.itemId || 0;
    this.itemName = orderLineItem && orderLineItem.itemName || '';

    this.priceList = orderLineItem && orderLineItem.priceList || PriceList["Main Price"];

    this.unitPrice = orderLineItem && orderLineItem.unitPrice || 0;
  }

  updatePriceList(priceList?: PriceList) {
    this.priceList = priceList || PriceList["Main Price"];
  };
}
