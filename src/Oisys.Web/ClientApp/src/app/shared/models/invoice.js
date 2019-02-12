import { JsonModelBase } from "./json-model-base";
import { InvoiceLineItemType } from "./invoice-line-item-type";
export class Invoice extends JsonModelBase {
    constructor(id, invoiceNumber, date, discountPercent, lineItems) {
        super();
        this.id = id;
        this.invoiceNumber = invoiceNumber;
        this.date = date;
        this.discountPercent = discountPercent;
        this.lineItems = lineItems;
        this.id = id || 0;
        this.invoiceNumber = invoiceNumber || 0;
        this.date = date || new Date();
        this.discountPercent = discountPercent || 0;
        this.lineItems = lineItems || new Array();
    }
    get selectedCustomer() {
        return this._selectedCustomer;
    }
    set selectedCustomer(customer) {
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