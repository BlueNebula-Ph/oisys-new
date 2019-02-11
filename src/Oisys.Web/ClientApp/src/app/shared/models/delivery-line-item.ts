import { JsonModelBase } from "./json-model-base";

export class DeliveryLineItem extends JsonModelBase {
  public id: number;
  public orderLineItemId: number;
  public quantity: number;
  public customerId: number;
  public customer: string;
  public orderNumber: string;
  public orderDate: string;
  public itemCode: string;
  public itemName: string;
  public category: string;

  get item() {
    return `${this.itemCode} - ${this.itemName} in Order # ${this.orderNumber} @ ${this.orderDate}`;
  }

  constructor();
  constructor(deliverLineItem: {
    orderLineItemId: number,
    quantity: number,
    customerId: number,
    customer: string,
    orderNumber: string,
    orderDate: string,
    itemCode: string,
    itemName: string,
    category: string
  });
  constructor(deliveryLineItem?: any) {
    super();

    this.id = deliveryLineItem && deliveryLineItem.id || 0;
    this.orderLineItemId = deliveryLineItem && deliveryLineItem.orderLineItemId || 0;
    this.quantity = deliveryLineItem && deliveryLineItem.quantity || 0;

    this.customerId = deliveryLineItem && deliveryLineItem.customerId || 0;
    this.customer = deliveryLineItem && deliveryLineItem.customer || '';
    this.orderNumber = deliveryLineItem && deliveryLineItem.orderNumber || '';
    this.orderDate = deliveryLineItem && deliveryLineItem.orderDate || '';
    this.itemCode = deliveryLineItem && deliveryLineItem.itemCode || '';
    this.itemName = deliveryLineItem && deliveryLineItem.itemName || '';
    this.category = deliveryLineItem && deliveryLineItem.category || '';
  }
}
