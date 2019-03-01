import { DeliveryLineItem } from "./delivery-line-item";

export class DeliveryGroupItem {
  constructor(
    public customerName: string,
    public customerAddress: string,
    public lineItems: DeliveryLineItem[]
  ) {
    this.customerName = customerName || '';
    this.customerAddress = customerAddress || '';
    this.lineItems = lineItems || new Array<DeliveryLineItem>();
  }

  addLineItem(lineItem: DeliveryLineItem) {
    if (lineItem) {
      this.lineItems.push(lineItem);
    }
  };
}
