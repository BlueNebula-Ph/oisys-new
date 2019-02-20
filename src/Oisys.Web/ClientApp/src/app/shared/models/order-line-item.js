export class OrderLineItem {
    get totalPrice() {
        return this.quantity * this.unitPrice;
    }
    constructor(orderLineItem) {
        this.id = orderLineItem && orderLineItem.id || 0;
        this.orderCode = orderLineItem && orderLineItem.orderCode || '';
        this.orderDate = orderLineItem && orderLineItem.orderDate || '';
        this.itemId = orderLineItem && orderLineItem.itemId || 0;
        this.itemCode = orderLineItem && orderLineItem.itemCode || '';
        this.itemName = orderLineItem && orderLineItem.itemName || '';
        this.quantity = orderLineItem && orderLineItem.quantity || 0;
        this.unit = orderLineItem && orderLineItem.unit || '';
        this.unitPrice = orderLineItem && orderLineItem.unitPrice || 0;
        this.categoryName = orderLineItem && orderLineItem.categoryName || '';
        this.quantityReturned = orderLineItem && orderLineItem.quantityReturned || 0;
        this.quantityDelivered = orderLineItem && orderLineItem.quantityDelivered || 0;
    }
}
//# sourceMappingURL=order-line-item.js.map