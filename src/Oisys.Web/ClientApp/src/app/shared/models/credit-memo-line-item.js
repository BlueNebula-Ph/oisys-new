import { JsonModelBase } from "./json-model-base";
export class CreditMemoLineItem extends JsonModelBase {
    get selectedItem() {
        return this._selectedItem;
    }
    set selectedItem(orderLineItem) {
        if (orderLineItem) {
            this._selectedItem = orderLineItem;
            this.orderLineItemId = orderLineItem.id;
            this.itemId = orderLineItem.itemId;
        }
    }
    get unit() {
        return this._selectedItem && this._selectedItem.unit || '';
    }
    get categoryName() {
        return this._selectedItem && this._selectedItem.categoryName || '';
    }
    get unitPrice() {
        return this._selectedItem && this._selectedItem.unitPrice || 0;
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
    }
}
//# sourceMappingURL=credit-memo-line-item.js.map