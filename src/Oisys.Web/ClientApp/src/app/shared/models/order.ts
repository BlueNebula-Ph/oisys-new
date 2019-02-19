import { Customer } from "./customer";
import { JsonModelBase } from "./json-model-base";
import { LineItem } from "./line-item";

export class Order extends JsonModelBase {
  public id: number;
  public code: number;
  public customerId: number;
  public customerName: string;
  public customerAddress: string;
  public date: Date;
  public dueDate: Date;
  public lineItems: LineItem[];

  private _customer: Customer;
  get customer() {
    return this._customer;
  }
  set customer(customer: Customer) {
    if (customer) {
      this._customer = customer;
      this.customerId = customer.id;
      this.updateLineItems();
    }
  }

  private _discountPercent: number;
  get discountPercent() {
    if (this._discountPercent) {
      return this._discountPercent;
    } else {
      if (this._customer && this.customerId && this.customerId != 0) {
        return this._customer.discount;
      }
      return 0;
    }
  }
  set discountPercent(value: number) {
    this._discountPercent = value;
  }

  get grossAmount() {
    var totalGrossAmount = 0;
    if (this.lineItems) {
      this.lineItems.forEach(val => {
        if (val && val.totalPrice) {
          totalGrossAmount += val.totalPrice;
        }
      });
    }
    return totalGrossAmount;
  }

  get discountAmount() {
    return parseFloat((this.grossAmount * this.discountPercent / 100).toFixed(2));
  }

  get totalAmount() {
    return this.grossAmount - this.discountAmount;
  }

  get lineItemsValid() {
    return this.lineItems.length > 0 && this.lineItems.every(lineItem => lineItem.itemId != 0);
  }

  constructor();
  constructor(order: Order);
  constructor(order?: any) {
    super();

    this.id = order && order.id || 0;
    this.code = order && order.code || 0;
    this.customerId = order && order.customerId || 0;
    this.customerName = order && order.customerName || '';
    this.customerAddress = order && order.customerAddress || '';
    this.discountPercent = order && order.discountPercent || 0;

    this.date = order && order.date || new Date();
    this.dueDate = order && order.dueDate || new Date();

    this.lineItems = (order && order.lineItems) ? order.lineItems.map(lineItem => new LineItem(lineItem)) : new Array<LineItem>();
    this.customer = (order && order.customer) ? new Customer(order.customer) : undefined;
  }

  updateLineItems() {
    if (this.customerId && this.customerId != 0 && this.lineItems) {
      this.lineItems.forEach((lineItem) => {
        lineItem.updatePriceList(this._customer.priceListId);
      });
    }
  };
}
