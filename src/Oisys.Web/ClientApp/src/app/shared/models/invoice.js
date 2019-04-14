import { JsonModelBase } from "./json-model-base";
import { Customer } from "./customer";
import { InvoiceLineItem } from "./invoice-line-item";
import { InvoiceLineItemType } from "./invoice-line-item-type";
export class Invoice extends JsonModelBase {
    get customer() {
        return this._customer;
    }
    set customer(customer) {
        if (customer) {
            this._customer = customer;
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
    get isNew() {
        const today = new Date();
        const sevenDaysBefore = new Date(today.setDate(today.getDate() - 7));
        return this.date > sevenDaysBefore;
    }
    constructor(invoice) {
        super();
        this.id = invoice && invoice.id || 0;
        this.invoiceNumber = invoice && invoice.invoiceNumber || 0;
        this.date = (invoice && invoice.date) ? new Date(invoice.date) : new Date();
        this.discountPercent = invoice && invoice.discountPercent || 0;
        this.rowVersion = invoice && invoice.rowVersion || '';
        this.lineItems = (invoice && invoice.lineItems) ?
            invoice.lineItems.map(lineItem => new InvoiceLineItem(lineItem)) : new Array();
        this.customer = (invoice && invoice.customer) ?
            new Customer(invoice.customer) : undefined;
    }
    computeTotalAmount(type) {
        var totalAmount = 0;
        this.lineItems.forEach(val => {
            if (val && val.totalAmount && val.type == type) {
                totalAmount += val.totalAmount;
            }
        });
        return totalAmount;
    }
    ;
}
//# sourceMappingURL=invoice.js.map