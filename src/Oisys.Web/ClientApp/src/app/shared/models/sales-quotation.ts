import { Customer } from "./customer";
import { JsonModelBase } from "./json-model-base";
import { LineItem } from "./line-item";

export class SalesQuotation extends JsonModelBase {
  public id: number;
  public quoteNumber: number;
  public customerId: number;
  public customerName: string;
  public customerAddress: string;
  public date: Date;
  public deliveryFee: number;
  public lineItems: LineItem[];

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
    return this.lineItems.length > 0 && this.lineItems.every(lineItem => lineItem.itemId != 0);
  }

  constructor();
  constructor(salesQuotation: SalesQuotation);
  constructor(salesQuotation?: SalesQuotation) {
    super();

    this.id = salesQuotation && salesQuotation.id || 0;
    this.quoteNumber = salesQuotation && salesQuotation.quoteNumber || 0;
    this.customerId = salesQuotation && salesQuotation.customerId || 0;
    this.customerName = salesQuotation && salesQuotation.customerName || '';
    this.customerAddress = salesQuotation && salesQuotation.customerAddress || '';
    this.date = salesQuotation && salesQuotation.date || new Date();

    this.deliveryFee = salesQuotation && salesQuotation.deliveryFee || 0;

    this.lineItems = salesQuotation && salesQuotation.lineItems || new Array<LineItem>();
  }

  updateLineItems() {
    if (this.customerId && this.customerId != 0) {
      this.lineItems.forEach((lineItem) => {
        lineItem.updatePriceList(this._selectedCustomer.priceListId);
      });
    }
  };
}
