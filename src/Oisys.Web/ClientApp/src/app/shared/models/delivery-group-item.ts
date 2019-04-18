import { DeliveryLineItem } from "./delivery-line-item";

export class DeliveryGroupItem {
  constructor(
    public customerId: number,
    public customerName: string,
    public customerAddress: string,
    public lineItems: DeliveryLineItem[]
  ) {
    this.customerId = customerId || 0;
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
