"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Province = /** @class */ (function () {
    function Province(id, name, rowVersion, cities) {
        id = id;
        name = name;
        rowVersion = rowVersion;
        cities = cities;
        id = id || 0;
        name = name || '';
        rowVersion = rowVersion || '';
        cities = cities || new Array();
    }
    ;
    return Province;
}());
exports.Province = Province;
//# sourceMappingURL=Province.js.map