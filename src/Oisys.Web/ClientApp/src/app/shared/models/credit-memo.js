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
    constructor(creditMemo) {
        super();
        this.id = creditMemo && creditMemo.id || 0;
        this.code = creditMemo && creditMemo.code || '';
        this.customerId = creditMemo && creditMemo.customerId || 0;
        this.customerName = creditMemo && creditMemo.customerName || '';
        this.customerAddress = creditMemo && creditMemo.customerAddress || '';
        this.date = creditMemo && creditMemo.date || new Date();
        this.driver = creditMemo && creditMemo.driver || '';
        this.totalAmount = creditMemo && creditMemo.totalAmount || 0;
        this.lineItems = creditMemo && creditMemo.lineItems || new Array();
    }
}
//# sourceMappingURL=credit-memo.js.map