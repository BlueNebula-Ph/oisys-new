export class DeliveryGroupItem {
    constructor(customerId, customerName, customerAddress, lineItems) {
        this.customerId = customerId;
        this.customerName = customerName;
        this.customerAddress = customerAddress;
        this.lineItems = lineItems;
        this.customerId = customerId || 0;
        this.customerName = customerName || '';
        this.customerAddress = customerAddress || '';
        this.lineItems = lineItems || new Array();
    }
    addLineItem(lineItem) {
        if (lineItem) {
            this.lineItems.push(lineItem);
        }
    }
    ;
}
//# sourceMappingURL=delivery-group-item.js.map