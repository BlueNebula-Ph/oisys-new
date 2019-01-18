import { Item } from "./item";

export class OrderLineItem {
  public id: number;
  public quantity: number;
  public itemId: number;

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

  constructor();
  constructor(orderLineItem: OrderLineItem);
  constructor(orderLineItem?: any) {
    this.id = orderLineItem && orderLineItem.id || 0;
    this.quantity = orderLineItem && orderLineItem.quantity || 0;
    this.itemId = orderLineItem && orderLineItem.itemId || 0;
  }
}
