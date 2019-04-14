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
    return this.lineItems.length > 0 && this.lineItems.every(lineItem => lineItem.itemId != 0);
  }

  get isNew() {
    const today = new Date();
    const sevenDaysBefore = new Date(today.setDate(today.getDate() - 7));
    return this.date > sevenDaysBefore;
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
    this.date = (salesQuotation && salesQuotation.date) ? new Date(salesQuotation.date) : new Date();

    this.deliveryFee = salesQuotation && salesQuotation.deliveryFee || 0;
    this.rowVersion = salesQuotation && salesQuotation.rowVersion || '';

    this.lineItems = (salesQuotation && salesQuotation.lineItems) ? salesQuotation.lineItems.map(lineItem => new LineItem(lineItem)) : new Array<LineItem>();
    this.customer = (salesQuotation && salesQuotation.customer) ? new Customer(salesQuotation.customer) : undefined;
  }

  updateLineItems() {
    if (this.customerId && this.customerId != 0) {
      this.lineItems.forEach((lineItem) => {
        lineItem.updatePriceList(this._customer.priceListId);
      });
    }
  };
}
