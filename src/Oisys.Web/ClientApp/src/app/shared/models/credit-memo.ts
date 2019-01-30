import { JsonModelBase } from "./json-model-base";
import { Customer } from "./customer";
import { CreditMemoLineItem } from "./credit-memo-line-item";

export class CreditMemo extends JsonModelBase {
  public id: number;
  public code: string;
  public customerId: number;
  public customerName: string;
  public customerAddress: string;
  public date: Date;
  public driver: string;
  public totalAmount: number;
  public lineItems: CreditMemoLineItem[];

  private _selectedCustomer: Customer;
  get selectedCustomer() {
    return this._selectedCustomer;
  }
  set selectedCustomer(customer: Customer) {
    if (customer) {
      this._selectedCustomer = customer;
      this.customerId = customer.id;
    }
  }

  constructor();
  constructor(creditMemo: CreditMemo);
  constructor(creditMemo?: any) {
    super();

    this.id = creditMemo && creditMemo.id || 0;
    this.code = creditMemo && creditMemo.code || '';
    this.customerId = creditMemo && creditMemo.customerId || 0;
    this.customerName = creditMemo && creditMemo.customerName || '';
    this.customerAddress = creditMemo && creditMemo.customerAddress || '';
    this.date = creditMemo && creditMemo.date || new Date();
    this.driver = creditMemo && creditMemo.driver || '';
    this.totalAmount = creditMemo && creditMemo.totalAmount || 0;

    this.lineItems = creditMemo && creditMemo.lineItems || new Array<CreditMemoLineItem>();
  }
}
