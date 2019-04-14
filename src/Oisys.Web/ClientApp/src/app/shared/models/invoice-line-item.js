import { JsonModelBase } from "./json-model-base";
import { InvoiceLineItemType } from "./invoice-line-item-type";
export class InvoiceLineItem extends JsonModelBase {
    get isOrder() {
        return this.type == InvoiceLineItemType.Order;
    }
    get isCreditMemo() {
        return this.type == InvoiceLineItemType.CreditMemo;
    }
    constructor(lineItem) {
        super();
        this.id = lineItem && lineItem.id || 0;
        this.code = lineItem && lineItem.code || 0;
        this.date = lineItem && lineItem.date || new Date();
        this.totalAmount = lineItem && lineItem.totalAmount || 0;
        this.type = (lineItem && lineItem.type) ? InvoiceLineItemType[lineItem.type] : InvoiceLineItemType.Order;
        this.orderId = lineItem && lineItem.orderId || null;
        this.creditMemoId = lineItem && lineItem.creditMemoId || null;
    }
}
//# sourceMappingURL=invoice-line-item.js.map