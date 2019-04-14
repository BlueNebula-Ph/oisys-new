import { JsonModelBase } from "./json-model-base";
import { Customer } from "./customer";
import { CreditMemoLineItem } from "./credit-memo-line-item";

export class CreditMemo extends JsonModelBase {
  public id: number;
  public code: number;
  public customerId: number;
  public customerName: string;
  public customerAddress: string;
  public date: Date;
  public driver: string;
  public lineItems: CreditMemoLineItem[];
  public rowVersion: string;

  private _customer: Customer;
  get customer() {
    return this._customer;
  }
  set customer(customer: Customer) {
    if (customer) {
      this._customer = customer;
      this.customerId = customer.id;
    }
  }

  get totalAmount() {
    var totalAmount = 0;
    if (this.lineItems) {
      this.lineItems.forEach(val => {
        if (val && val.totalPrice) {
          totalAmount += val.totalPrice;
        }
      });
    }
    return totalAmount;
  }

  get lineItemsValid() {
    return this.lineItems && this.lineItems.length > 0 && this.lineItems.every(lineItem => lineItem.itemId && lineItem.itemId != 0);
  }

  get isNew() {
    const today = new Date();
    const sevenDaysBefore = new Date(today.setDate(today.getDate() - 7));
    return this.date > sevenDaysBefore;
  }

  constructor();
  constructor(creditMemo: CreditMemo);
  constructor(creditMemo?: any) {
    super();

    this.id = creditMemo && creditMemo.id || 0;
    this.code = creditMemo && creditMemo.code || 0;
    this.customerId = creditMemo && creditMemo.customerId || 0;
    this.customerName = creditMemo && creditMemo.customerName || '';
    this.customerAddress = creditMemo && creditMemo.customerAddress || '';
    this.date = (creditMemo && creditMemo.date) ? new Date(creditMemo.date) : new Date();
    this.driver = creditMemo && creditMemo.driver || '';
    this.rowVersion = creditMemo && creditMemo.rowVersion || '';

    this.lineItems = (creditMemo && creditMemo.lineItems) ?
      creditMemo.lineItems.map(lineItem => new CreditMemoLineItem(lineItem)) :
      new Array<CreditMemoLineItem>();
    this.customer = (creditMemo && creditMemo.customer) ? new Customer(creditMemo.customer) : undefined;
  }
}
