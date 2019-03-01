export class DeliveryGroupItem {
    constructor(customerName, customerAddress, lineItems) {
        this.customerName = customerName;
        this.customerAddress = customerAddress;
        this.lineItems = lineItems;
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