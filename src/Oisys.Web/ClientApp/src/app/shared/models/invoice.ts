import { JsonModelBase } from "./json-model-base";
import { Customer } from "./customer";
import { InvoiceLineItem } from "./invoice-line-item";
import { InvoiceLineItemType } from "./invoice-line-item-type";

export class Invoice extends JsonModelBase {
  public customerId: number;

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

  get totalAmountDue() {
    return this.computeTotalAmount(InvoiceLineItemType.Order);
  }

  get totalCreditAmount() {
    return this.computeTotalAmount(InvoiceLineItemType.CreditMemo);
  }

  get totalAmount() {
    return this.totalAmountDue - this.totalCreditAmount - this.discountAmount;
  }

  get discountAmount() {
    return parseFloat(((this.totalAmountDue - this.totalCreditAmount) * this.discountPercent / 100).toFixed(2));
  }

  constructor();
  constructor(invoice?: any);
  constructor(
    public id?: number,
    public invoiceNumber?: number,
    public date?: Date,
    public discountPercent?: number,
    public lineItems?: InvoiceLineItem[]
  ) {
    super();

    this.id = id || 0;
    this.invoiceNumber = invoiceNumber || 0;
    this.date = date || new Date();
    this.discountPercent = discountPercent || 0;

    this.lineItems = lineItems || new Array<InvoiceLineItem>();
  }

  computeTotalAmount(type: InvoiceLineItemType): number {
    var totalAmount = 0;
    this.lineItems.forEach(val => {
      if (val && val.totalAmount && val.type == type) {
        totalAmount += val.totalAmount;
      }
    });
    return totalAmount;
  };
}
