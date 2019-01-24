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

  private _selectedCustomer: Customer;
  get selectedCustomer() {
    return this._selectedCustomer;
  }
  set selectedCustomer(customer: Customer) {
    if (customer) {
      this._selectedCustomer = customer;
      this.customerId = customer.id;
      this.updateLineItems();
    }
  }

  private _discountPercent: number;
  get discountPercent() {
    if (this._discountPercent) {
      return this._discountPercent;
    }
    if (this.selectedCustomer && this.customerId && this.customerId != 0) {
      return this.selectedCustomer.discount;
    }
    return 0;
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

    this.date = order && order.date || new Date();
    this.dueDate = order && order.dueDate || new Date();

    this.discountPercent = order && order.discountPercent || 0;
    this.lineItems = order && order.lineItems || new Array<LineItem>();
  }

  updateLineItems() {
    if (this.customerId && this.customerId != 0) {
      this.lineItems.forEach((lineItem) => {
        lineItem.updatePriceList(this._selectedCustomer.priceListId);
      });
    }
  };
}
