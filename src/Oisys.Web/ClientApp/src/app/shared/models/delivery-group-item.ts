import { DeliveryLineItem } from "./delivery-line-item";

export class DeliveryGroupItem {
  get totalQuantity() {
    return this.lineItems.reduce((total, item) => { return total + item.quantity }, 0);
  }

  constructor(
    public itemCode: string,
    public itemName: string,
    public categoryName: string,
    public lineItems: DeliveryLineItem[]
  ) {
    this.itemCode = itemCode || '';
    this.itemName = itemName || '';
    this.categoryName = categoryName || '';
    this.lineItems = lineItems || new Array<DeliveryLineItem>();
  }

  addLineItem(lineItem: DeliveryLineItem) {
    if (lineItem) {
      this.lineItems.push(lineItem);
    }
  };
}
