import { JsonModelBase } from "./json-model-base";
import { OrderLineItem } from "./order-line-item";
import { Customer } from "./customer";

export class DeliveryLineItem extends JsonModelBase {
  public id: number;
  public orderLineItemId: number;
  public quantity: number;
  public orderCode: string;
  public orderDate: string;
  public itemCode: string;
  public itemName: string;
  public unit: string;
  public categoryName: string;
  public quantityNotDelivered: number = 0;

  get item() {
    return `${this.itemCode} - ${this.itemName}`;
  };

  private _customer: Customer;
  get customer() {
    return this._customer;
  }
  set customer(value: Customer) {
    if (value) {
      this._customer = value;
    }
  }

  constructor();
  constructor(deliveryLineItem: DeliveryLineItem);
  constructor(deliveryLineItem: OrderLineItem);
  constructor(deliveryLineItem?: any) {
    super();

    this.id = deliveryLineItem && deliveryLineItem.id || 0;
    this.orderLineItemId = deliveryLineItem && deliveryLineItem.orderLineItemId || 0;
    this.quantity = deliveryLineItem && deliveryLineItem.quantity || 0;

    this.orderCode = deliveryLineItem && deliveryLineItem.orderCode || '';
    this.orderDate = deliveryLineItem && deliveryLineItem.orderDate || '';
    this.itemCode = deliveryLineItem && deliveryLineItem.itemCode || '';
    this.itemName = deliveryLineItem && deliveryLineItem.itemName || '';
    this.unit = deliveryLineItem && deliveryLineItem.unit || '';
    this.categoryName = deliveryLineItem && deliveryLineItem.categoryName || '';

    this.customer = (deliveryLineItem && deliveryLineItem.customer) ?
      new Customer(deliveryLineItem.customer) : undefined;
  }
}
