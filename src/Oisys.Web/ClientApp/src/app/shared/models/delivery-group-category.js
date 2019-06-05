export class DeliveryGroupCategory {
    constructor(categoryName, lineItems) {
        this.categoryName = categoryName;
        this.lineItems = lineItems;
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
//# sourceMappingURL=delivery-group-category.js.map