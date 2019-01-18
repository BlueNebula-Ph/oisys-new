"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var OrderLineItem = /** @class */ (function () {
    function OrderLineItem(orderLineItem) {
        this.id = orderLineItem && orderLineItem.id || 0;
        this.quantity = orderLineItem && orderLineItem.quantity || 0;
        this.itemId = orderLineItem && orderLineItem.itemId || 0;
    }
    Object.defineProperty(OrderLineItem.prototype, "selectedItem", {
        get: function () {
            return this._selectedItem;
        },
        set: function (item) {
            if (item) {
                this._selectedItem = item;
                this.itemId = item.id;
            }
        },
        enumerable: true,
        configurable: true
    });
    return OrderLineItem;
}());
exports.OrderLineItem = OrderLineItem;
//# sourceMappingURL=order-line-item.js.map