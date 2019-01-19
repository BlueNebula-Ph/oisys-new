"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Province = /** @class */ (function () {
    function Province(province) {
        this.id = province && province.id || 0;
        this.name = province && province.name || '';
        this.rowVersion = province && province.rowVersion || '';
        this.cities = province && province.cities || new Array();
    }
    Object.defineProperty(Province.prototype, "citiesValid", {
        get: function () {
            return this.cities.some(function (city) { return city.name != '' && !city.isDeleted; });
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Province.prototype, "cityNames", {
        get: function () {
            return this.cities.map(function (sc) { return sc.name; }).join(', ');
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    return Province;
}());
exports.Province = Province;
//# sourceMappingURL=Province.js.map