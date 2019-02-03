import { JsonModelBase } from "./json-model-base";
export class DeliveryLineItem extends JsonModelBase {
    constructor(deliveryLineItem) {
        super();
        this.id = deliveryLineItem && deliveryLineItem.id || 0;
    }
}
//# sourceMappingURL=delivery-line-item.js.map