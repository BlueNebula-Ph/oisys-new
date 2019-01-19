import { Customer } from "./customer";
import { OrderLineItem } from "./order-line-item";

export class Order {
  public id: number;
  public code: string;
  public customerId: number;
  public customer: Customer;
  public customerName: string;
  public date: Date;
  public dueDate: Date;
  public discountPercent: number;
  public discountAmount: number;
  public grossAmount: number;
  public totalAmount: number;
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

  constructor();
  constructor(order: Order);
  constructor(order?: any) {
    this.id = order && order.id || 0;
    this.code = order && order.code || '';
    this.customerId = order && order.customerId || 0;

    this.date = order && order.date || new Date();
    this.dueDate = order && order.dueDate || new Date();

    this.discountPercent = order && order.discountPercent || 0;
    this.lineItems = order && order.lineItems || new Array<OrderLineItem>();
  }

  updateLineItems() {
    if (this.customerId && this.customerId != 0) {
      this.lineItems.forEach((lineItem) => {
        lineItem.updatePriceList(this._selectedCustomer.priceListId);
      });
    }
  };
}
