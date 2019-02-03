import { JsonModelBase } from "./json-model-base";
export class CreditMemo extends JsonModelBase {
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
    constructor(creditMemo) {
        super();
        this.id = creditMemo && creditMemo.id || 0;
        this.code = creditMemo && creditMemo.code || 0;
        this.customerId = creditMemo && creditMemo.customerId || 0;
        this.customerName = creditMemo && creditMemo.customerName || '';
        this.customerAddress = creditMemo && creditMemo.customerAddress || '';
        this.date = creditMemo && creditMemo.date || new Date();
        this.driver = creditMemo && creditMemo.driver || '';
        this.lineItems = creditMemo && creditMemo.lineItems || new Array();
    }
}
//# sourceMappingURL=credit-memo.js.map