import { DeliveryLineItem } from "./delivery-line-item";

export class DeliveryGroupCategory {
  get totalQuantity() {
    return this.lineItems.reduce((total, item) => { return total + item.quantity }, 0);
  }

  constructor(
    public categoryName: string,
    public lineItems: DeliveryLineItem[]
  ) {
    this.categoryName = categoryName || '';
    this.lineItems = lineItems || new Array<DeliveryLineItem>();
  }

  addLineItem(lineItem: DeliveryLineItem) {
    if (lineItem) {
      this.lineItems.push(lineItem);
    }
  };
}
