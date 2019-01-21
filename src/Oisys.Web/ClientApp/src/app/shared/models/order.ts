import { ModelBase } from "./model-base";
import { Customer } from "./customer";
import { OrderLineItem } from "./order-line-item";

export class Order extends ModelBase {
  public id: number;
  public code: string;
  public customerId: number;
  public customer: Customer;
  public customerName: string;
  public date: Date;
  public dueDate: Date;
  public discountPercent: number;
  public lineItems: OrderLineItem[];

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

  private _grossAmount: number;
  get grossAmount() {
    if (this._grossAmount) {
      return this._grossAmount;
    };

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

  private _discountAmount: number;
  get discountAmount() {
    if (this._discountAmount) {
      return this._discountAmount;
    }

    return this.grossAmount * this.discountPercent / 100;
  }

  private _totalAmount: number;
  get totalAmount() {
    if (this._totalAmount && this._totalAmount != 0) {
      return this._totalAmount;
    }
    return this.grossAmount - this.discountAmount;
  }

  get lineItemsValid() {
    return this.lineItems.every(lineItem => lineItem.itemId != 0);
  }

  constructor();
  constructor(order: Order);
  constructor(order?: any) {
    super();

    this.id = order && order.id || 0;
    this.code = order && order.code || '';
    this.customerId = order && order.customerId || 0;
    this.customer = order && new Customer(order.customer) || new Customer();

    this.date = order && order.date || new Date();
    this.dueDate = order && order.dueDate || new Date();

    this.discountPercent = order && order.discountPercent || 0;
    this.lineItems = order && order.lineItems || new Array<OrderLineItem>();

    this._grossAmount = order && order.grossAmount || 0;
    this._discountAmount = order && order.discountAmount || 0;
    this._totalAmount = order && order.totalAmount || 0;
  }

  updateLineItems() {
    if (this.customerId && this.customerId != 0) {
      this.lineItems.forEach((lineItem) => {
        lineItem.updatePriceList(this._selectedCustomer.priceListId);
      });
    }
  };
}
