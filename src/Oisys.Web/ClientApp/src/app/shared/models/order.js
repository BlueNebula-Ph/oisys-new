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
var customer_1 = require("./customer");
var Order = /** @class */ (function (_super) {
    __extends(Order, _super);
    function Order(order) {
        var _this = _super.call(this) || this;
        _this.id = order && order.id || 0;
        _this.code = order && order.code || '';
        _this.customerId = order && order.customerId || 0;
        _this.customer = order && new customer_1.Customer(order.customer) || new customer_1.Customer();
        _this.date = order && order.date || new Date();
        _this.dueDate = order && order.dueDate || new Date();
        _this.discountPercent = order && order.discountPercent || 0;
        _this.lineItems = order && order.lineItems || new Array();
        _this._grossAmount = order && order.grossAmount || 0;
        _this._discountAmount = order && order.discountAmount || 0;
        _this._totalAmount = order && order.totalAmount || 0;
        return _this;
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
    Object.defineProperty(Order.prototype, "grossAmount", {
        get: function () {
            if (this._grossAmount) {
                return this._grossAmount;
            }
            ;
            var totalGrossAmount = 0;
            if (this.lineItems) {
                this.lineItems.forEach(function (val) {
                    if (val && val.totalPrice) {
                        totalGrossAmount += val.totalPrice;
                    }
                });
            }
            return totalGrossAmount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Order.prototype, "discountAmount", {
        get: function () {
            if (this._discountAmount) {
                return this._discountAmount;
            }
            return this.grossAmount * this.discountPercent / 100;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Order.prototype, "totalAmount", {
        get: function () {
            if (this._totalAmount && this._totalAmount != 0) {
                return this._totalAmount;
            }
            return this.grossAmount - this.discountAmount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Order.prototype, "lineItemsValid", {
        get: function () {
            return this.lineItems.every(function (lineItem) { return lineItem.itemId != 0; });
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
}(model_base_1.ModelBase));
exports.Order = Order;
//# sourceMappingURL=order.js.map