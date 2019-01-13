"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Province = /** @class */ (function () {
    function Province(id, name, rowVersion, cities) {
        this.id = id;
        this.name = name;
        this.rowVersion = rowVersion;
        this.cities = cities;
        this.id = this.id || 0;
        this.name = this.name || '';
        this.rowVersion = this.rowVersion || '';
        this.cities = this.cities || new Array();
    }
    Object.defineProperty(Province.prototype, "citiesValid", {
        get: function () {
            return this.cities.some(function (city) { return city.name != '' && !city.isDeleted; });
        },
        enumerable: true,
        configurable: true
    });
    ;
    return Province;
}());
exports.Province = Province;
//# sourceMappingURL=province.js.map