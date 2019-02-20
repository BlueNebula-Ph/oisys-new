import { JsonModelBase } from "./json-model-base";
import { OrderLineItem } from "./order-line-item";
export class CreditMemoLineItem extends JsonModelBase {
    get orderLineItem() {
        return this._orderLineItem;
    }
    set orderLineItem(orderLineItem) {
        if (orderLineItem) {
            this._orderLineItem = orderLineItem;
            this.orderLineItemId = orderLineItem.id;
            this.itemId = orderLineItem.itemId;
        }
    }
    get unit() {
        return this._orderLineItem && this._orderLineItem.unit || '';
    }
    get categoryName() {
        return this._orderLineItem && this._orderLineItem.categoryName || '';
    }
    get unitPrice() {
        return this._orderLineItem && this._orderLineItem.unitPrice || 0;
    }
    get totalPrice() {
        return this.quantity * this.unitPrice;
    }
    constructor(creditMemoLineItem) {
        super();
        this.id = creditMemoLineItem && creditMemoLineItem.id || 0;
        this.creditMemoId = creditMemoLineItem && creditMemoLineItem.creditMemoId || 0;
        this.orderLineItemId = creditMemoLineItem && creditMemoLineItem.orderLineItemId || 0;
        this.itemId = creditMemoLineItem && creditMemoLineItem.itemId || 0;
        this.quantity = creditMemoLineItem && creditMemoLineItem.quantity || 0;
        this.shouldAddBackToInventory = creditMemoLineItem && creditMemoLineItem.shouldAddBackToInventory || false;
        this.orderLineItem = (creditMemoLineItem && creditMemoLineItem.orderLineItem) ? new OrderLineItem(creditMemoLineItem.orderLineItem) : undefined;
    }
}
//# sourceMappingURL=credit-memo-line-item.js.map