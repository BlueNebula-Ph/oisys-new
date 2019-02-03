import { JsonModelBase } from "./json-model-base";
export class InvoiceLineItem extends JsonModelBase {
    constructor(invoiceLineItem) {
        super();
        this.id = invoiceLineItem && invoiceLineItem.id || 0;
    }
}
//# sourceMappingURL=invoice-line-item.js.map