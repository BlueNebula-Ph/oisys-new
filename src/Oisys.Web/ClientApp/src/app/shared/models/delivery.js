import { JsonModelBase } from "./json-model-base";
export class Delivery extends JsonModelBase {
    constructor(delivery) {
        super();
        this.id = delivery && delivery.id || 0;
    }
}
//# sourceMappingURL=delivery.js.map