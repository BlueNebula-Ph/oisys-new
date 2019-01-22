"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var adjustment_type_1 = require("./adjustment-type");
var Adjustment = /** @class */ (function () {
    function Adjustment(adjustment) {
        this.id = adjustment && adjustment.id || 0;
        this.adjustmentType = adjustment && adjustment.adjustmentType || adjustment_type_1.AdjustmentType['Add'];
        this.adjustmentQuantity = adjustment && adjustment.adjustmentQuantity || 0;
        this.remarks = adjustment && adjustment.remarks || '';
        this.operator = adjustment && adjustment.operator || '';
        this.machine = adjustment && adjustment.machine || '';
    }
    Object.defineProperty(Adjustment.prototype, "selectedItem", {
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
    ;
    return Adjustment;
}());
exports.Adjustment = Adjustment;
//# sourceMappingURL=adjustment.js.map