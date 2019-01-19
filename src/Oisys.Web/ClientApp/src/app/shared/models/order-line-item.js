"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var price_list_1 = require("./price-list");
var OrderLineItem = /** @class */ (function () {
    function OrderLineItem(orderLineItem) {
        this.id = orderLineItem && orderLineItem.id || 0;
        this.quantity = orderLineItem && orderLineItem.quantity || 0;
        this.itemId = orderLineItem && orderLineItem.itemId || 0;
        this.priceList = orderLineItem && orderLineItem.priceList || price_list_1.PriceList["Main Price"];
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
    Object.defineProperty(OrderLineItem.prototype, "unit", {
        get: function () {
            return this._selectedItem && this._selectedItem.unit || '';
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrderLineItem.prototype, "categoryName", {
        get: function () {
            return this._selectedItem && this._selectedItem.categoryName || '';
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrderLineItem.prototype, "unitPrice", {
        get: function () {
            var unitPrice = 0;
            if (this._selectedItem) {
                switch (this.priceList) {
                    case price_list_1.PriceList["Main Price"]:
                        unitPrice = this._selectedItem.mainPrice;
                        break;
                    case price_list_1.PriceList["Walk-In Price"]:
                        unitPrice = this._selectedItem.walkInPrice;
                        break;
                    case price_list_1.PriceList["N.E. Price"]:
                        unitPrice = this._selectedItem.nePrice;
                        break;
                }
            }
            return unitPrice;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrderLineItem.prototype, "totalPrice", {
        get: function () {
            return this.quantity * this.unitPrice;
        },
        enumerable: true,
        configurable: true
    });
    OrderLineItem.prototype.updatePriceList = function (priceList) {
        this.priceList = priceList || price_list_1.PriceList["Main Price"];
    };
    ;
    return OrderLineItem;
}());
exports.OrderLineItem = OrderLineItem;
//# sourceMappingURL=order-line-item.js.map