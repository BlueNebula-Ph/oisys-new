import { JsonModelBase } from "./json-model-base";
export class DeliveryLineItem extends JsonModelBase {
    get item() {
        return `${this.itemCode} - ${this.itemName} in Order # ${this.orderNumber} @ ${this.orderDate}`;
    }
    constructor(deliveryLineItem) {
        super();
        this.id = deliveryLineItem && deliveryLineItem.id || 0;
        this.orderLineItemId = deliveryLineItem && deliveryLineItem.orderLineItemId || 0;
        this.quantity = deliveryLineItem && deliveryLineItem.quantity || 0;
        this.customerId = deliveryLineItem && deliveryLineItem.customerId || 0;
        this.customer = deliveryLineItem && deliveryLineItem.customer || '';
        this.orderNumber = deliveryLineItem && deliveryLineItem.orderNumber || '';
        this.orderDate = deliveryLineItem && deliveryLineItem.orderDate || '';
        this.itemCode = deliveryLineItem && deliveryLineItem.itemCode || '';
        this.itemName = deliveryLineItem && deliveryLineItem.itemName || '';
        this.category = deliveryLineItem && deliveryLineItem.category || '';
    }
}
//# sourceMappingURL=delivery-line-item.js.map