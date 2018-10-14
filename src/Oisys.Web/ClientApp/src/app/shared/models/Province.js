"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Province = /** @class */ (function () {
    function Province(id, name, rowVersion, cities) {
        this.id = id;
        this.name = name;
        this.rowVersion = rowVersion;
        this.cities = cities;
        this.id = id || 0;
        this.name = name || '';
        this.rowVersion = rowVersion || '';
        this.cities = cities || new Array();
    }
    ;
    return Province;
}());
exports.Province = Province;
//# sourceMappingURL=Province.js.map