"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Order = /** @class */ (function () {
    function Order(order) {
        this.id = order && order.id || 0;
        this.code = order && order.code || '';
        this.customerId = order && order.customerId || 0;
        this.date = order && order.date || new Date();
        this.dueDate = order && order.dueDate || new Date();
        this.discountPercent = order && order.discountPercent || 0;
        this.lineItems = order && order.lineItems || new Array();
    }
    Object.defineProperty(Order.prototype, "selectedCustomer", {
        get: function () {
            return this._selectedCustomer;
        },
        set: function (customer) {
            if (customer) {
                this._selectedCustomer = customer;
                this.customerId = customer.id;
                this.updateLineItems();
            }
        },
        enumerable: true,
        configurable: true
    });
    Order.prototype.updateLineItems = function () {
        var _this = this;
        if (this.customerId && this.customerId != 0) {
            this.lineItems.forEach(function (lineItem) {
                lineItem.updatePriceList(_this._selectedCustomer.priceListId);
            });
        }
    };
    ;
    return Order;
}());
exports.Order = Order;
//# sourceMappingURL=order.js.map