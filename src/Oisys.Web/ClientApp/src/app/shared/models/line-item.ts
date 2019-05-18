import { PriceList } from "./price-list";
import { Item } from "./item";
import { JsonModelBase } from "./json-model-base";

export class LineItem extends JsonModelBase {
  public id: number;
  public quantity: number;
  public itemId: number;
  public itemName: string;
  public discountPercent: number;

  public priceList: PriceList;

  private _item: Item;
  get item() {
    return this._item;
  }
  set item(item: Item) {
    if (item) {
      this._item = item;
      this.itemId = item.id;
    }
  }

  get unit() {
    return this._item && this._item.unit || '';
  }

  get categoryName() {
    return this._item && this._item.categoryName || '';
  }

  private _unitPrice: number;
  get unitPrice() {
    if (this._unitPrice && this._unitPrice != 0) {
      return this._unitPrice;
    }

    var unitPrice = 0;
    if (this._item && this.itemId != 0) {
      switch (this.priceList) {
        case PriceList["Main Price"]:
          unitPrice = this._item.mainPrice;
          break;
        case PriceList["Walk-In Price"]:
          unitPrice = this._item.walkInPrice;
          break;
        case PriceList["N.E. Price"]:
          unitPrice = this._item.nePrice;
          break;
      }
    }
    return unitPrice;
  }
  set unitPrice(value: number) {
    this._unitPrice = value;
  }

  get discountedUnitPrice() {
    return parseFloat((this.unitPrice - (this.unitPrice * this.discountPercent / 100)).toFixed(2));
  }

  get totalPrice() {
    return this.quantity * this.unitPrice;
  }

  constructor();
  constructor(lineItem: LineItem);
  constructor(lineItem?: any) {
    super();

    this.id = lineItem && lineItem.id || 0;
    this.quantity = lineItem && lineItem.quantity || 0;
    this.itemId = lineItem && lineItem.itemId || 0;
    this.itemName = lineItem && lineItem.itemName || '';
    this.discountPercent = lineItem && lineItem.discountPercent || 0;

    this.priceList = lineItem && lineItem.priceList || PriceList["Main Price"];

    this.unitPrice = lineItem && lineItem.unitPrice || 0;

    this.item = (lineItem && lineItem.item) ? new Item(lineItem.item) : undefined;
  }

  updatePriceList(priceList?: PriceList) {
    this.priceList = priceList || PriceList["Main Price"];
  };
} 
