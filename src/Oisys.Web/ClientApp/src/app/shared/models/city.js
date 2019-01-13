"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var City = /** @class */ (function () {
    function City(id, name, rowVersion) {
        this.id = id;
        this.name = name;
        this.rowVersion = rowVersion;
        this.isDeleted = false;
        this.id = this.id || 0;
        this.name = this.name || '';
        this.rowVersion = this.rowVersion || '';
    }
    ;
    return City;
}());
exports.City = City;
//# sourceMappingURL=city.js.map