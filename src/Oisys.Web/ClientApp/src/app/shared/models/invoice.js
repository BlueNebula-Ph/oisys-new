import { JsonModelBase } from "./json-model-base";
export class Invoice extends JsonModelBase {
    constructor(invoice) {
        super();
        this.id = invoice && invoice.id || 0;
    }
}
//# sourceMappingURL=invoice.js.map