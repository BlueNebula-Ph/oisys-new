"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var City = /** @class */ (function () {
    function City(city) {
        this.isDeleted = false;
        this.id = city && city.id || 0;
        this.name = city && city.name || '';
        this.rowVersion = city && city.rowVersion || '';
    }
    ;
    return City;
}());
exports.City = City;
//# sourceMappingURL=city.js.map