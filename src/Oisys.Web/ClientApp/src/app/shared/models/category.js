"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Category = /** @class */ (function () {
    function Category(category) {
        this.id = category && category.id || 0;
        this.name = category && category.name || '';
        this.rowVersion = category && category.rowVersion || '';
    }
    ;
    return Category;
}());
exports.Category = Category;
//# sourceMappingURL=category.js.map