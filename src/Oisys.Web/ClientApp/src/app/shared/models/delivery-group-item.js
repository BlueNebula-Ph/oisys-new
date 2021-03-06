export class DeliveryGroupItem {
    constructor(itemCode, itemName, categoryName, lineItems) {
        this.itemCode = itemCode;
        this.itemName = itemName;
        this.categoryName = categoryName;
        this.lineItems = lineItems;
        this.itemCode = itemCode || '';
        this.itemName = itemName || '';
        this.categoryName = categoryName || '';
        this.lineItems = lineItems || new Array();
    }
    get totalQuantity() {
        return this.lineItems.reduce((total, item) => { return total + item.quantity; }, 0);
    }
    addLineItem(lineItem) {
        if (lineItem) {
            this.lineItems.push(lineItem);
        }
    }
    ;
}
//# sourceMappingURL=delivery-group-item.js.map