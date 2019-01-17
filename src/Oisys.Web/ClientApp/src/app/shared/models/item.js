"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Item = /** @class */ (function () {
    function Item(item) {
        this.id = item && item.id || 0;
        this.code = item && item.code || '';
        this.name = item && item.name || '';
        this.description = item && item.description || '';
        this.categoryId = item && item.categoryId || 0;
        this.categoryName = item && item.categoryName || '';
        this.quantity = item && item.quantity || 0;
        this.unit = item && item.unit || '';
        this.weight = item && item.weight || '';
        this.thickness = item && item.thickness || '';
        this.mainPrice = item && item.mainPrice || 0;
        this.walkInPrice = item && item.walkInPrice || 0;
        this.nePrice = item && item.nePrice || 0;
    }
    Object.defineProperty(Item.prototype, "selectedCategory", {
        get: function () {
            return this._selectedCategory;
        },
        set: function (category) {
            if (category) {
                this._selectedCategory = category;
                this.categoryId = category.id;
            }
        },
        enumerable: true,
        configurable: true
    });
    return Item;
}());
exports.Item = Item;
//# sourceMappingURL=item.js.map