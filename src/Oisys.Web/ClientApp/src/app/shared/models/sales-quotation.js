import { JsonModelBase } from "./json-model-base";
export class SalesQuotation extends JsonModelBase {
    get selectedCustomer() {
        return this._selectedCustomer;
    }
    set selectedCustomer(customer) {
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
    constructor(salesQuotation) {
        super();
        this.id = salesQuotation && salesQuotation.id || 0;
        this.quoteNumber = salesQuotation && salesQuotation.quoteNumber || 0;
        this.customerId = salesQuotation && salesQuotation.customerId || 0;
        this.customerName = salesQuotation && salesQuotation.customerName || '';
        this.customerAddress = salesQuotation && salesQuotation.customerAddress || '';
        this.date = salesQuotation && salesQuotation.date || new Date();
        this.deliveryFee = salesQuotation && salesQuotation.deliveryFee || 0;
        this.lineItems = salesQuotation && salesQuotation.lineItems || new Array();
    }
    updateLineItems() {
        if (this.customerId && this.customerId != 0) {
            this.lineItems.forEach((lineItem) => {
                lineItem.updatePriceList(this._selectedCustomer.priceListId);
            });
        }
    }
    ;
}
//# sourceMappingURL=sales-quotation.js.map