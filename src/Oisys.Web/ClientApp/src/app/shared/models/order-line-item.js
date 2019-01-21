"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var model_base_1 = require("./model-base");
var price_list_1 = require("./price-list");
var OrderLineItem = /** @class */ (function (_super) {
    __extends(OrderLineItem, _super);
    function OrderLineItem(orderLineItem) {
        var _this = _super.call(this) || this;
        _this.id = orderLineItem && orderLineItem.id || 0;
        _this.quantity = orderLineItem && orderLineItem.quantity || 0;
        _this.itemId = orderLineItem && orderLineItem.itemId || 0;
        _this.priceList = orderLineItem && orderLineItem.priceList || price_list_1.PriceList["Main Price"];
        _this.unitPrice = orderLineItem && orderLineItem.unitPrice || 0;
        _this.totalPrice = orderLineItem && orderLineItem.totalPrice || 0;
        return _this;
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
            if (this._unitPrice && this._unitPrice != 0) {
                return this._unitPrice;
            }
            var unitPrice = 0;
            if (this._selectedItem && this.itemId != 0) {
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
        set: function (value) {
            this._unitPrice = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrderLineItem.prototype, "totalPrice", {
        get: function () {
            if (this._totalPrice && this._totalPrice != 0) {
                return this._totalPrice;
            }
            return this.quantity * this.unitPrice;
        },
        set: function (value) {
            this._totalPrice = value;
        },
        enumerable: true,
        configurable: true
    });
    OrderLineItem.prototype.updatePriceList = function (priceList) {
        this.priceList = priceList || price_list_1.PriceList["Main Price"];
    };
    ;
    return OrderLineItem;
}(model_base_1.ModelBase));
exports.OrderLineItem = OrderLineItem;
//# sourceMappingURL=order-line-item.js.map