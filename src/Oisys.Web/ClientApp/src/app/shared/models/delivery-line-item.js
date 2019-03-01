import { JsonModelBase } from "./json-model-base";
import { Customer } from "./customer";
export class DeliveryLineItem extends JsonModelBase {
    constructor(deliveryLineItem) {
        super();
        this.quantityNotDelivered = 0;
        this.id = deliveryLineItem && deliveryLineItem.id || 0;
        this.orderLineItemId = deliveryLineItem && deliveryLineItem.orderLineItemId || 0;
        this.quantity = deliveryLineItem && deliveryLineItem.quantity || 0;
        this.orderCode = deliveryLineItem && deliveryLineItem.orderCode || '';
        this.orderDate = deliveryLineItem && deliveryLineItem.orderDate || '';
        this.itemCode = deliveryLineItem && deliveryLineItem.itemCode || '';
        this.itemName = deliveryLineItem && deliveryLineItem.itemName || '';
        this.unit = deliveryLineItem && deliveryLineItem.unit || '';
        this.categoryName = deliveryLineItem && deliveryLineItem.categoryName || '';
        this.customer = (deliveryLineItem && deliveryLineItem.customer) ?
            new Customer(deliveryLineItem.customer) : undefined;
    }
    get item() {
        return `${this.itemCode} - ${this.itemName}`;
    }
    ;
    get customer() {
        return this._customer;
    }
    set customer(value) {
        if (value) {
            this._customer = value;
        }
    }
}
//# sourceMappingURL=delivery-line-item.js.map