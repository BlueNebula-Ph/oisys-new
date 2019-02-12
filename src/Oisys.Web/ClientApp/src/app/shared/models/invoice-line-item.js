import { JsonModelBase } from "./json-model-base";
import { InvoiceLineItemType } from "./invoice-line-item-type";
export class InvoiceLineItem extends JsonModelBase {
    get isOrder() {
        return this.type == InvoiceLineItemType.Order;
    }
    get isCreditMemo() {
        return this.type == InvoiceLineItemType.CreditMemo;
    }
    //constructor();
    constructor(lineItem = {}) {
        super();
        this.id = lineItem.id || 0;
        this.code = lineItem.code || 0;
        this.date = lineItem.date || new Date();
        this.totalAmount = lineItem.totalAmount || 0;
        this.orderId = lineItem.orderId || 0;
        this.creditMemoId = lineItem.creditMemoId || 0;
    }
}
//# sourceMappingURL=invoice-line-item.js.map