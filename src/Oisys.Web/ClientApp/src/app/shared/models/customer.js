"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var price_list_1 = require("./price-list");
var Customer = /** @class */ (function () {
    function Customer(customer) {
        this.id = customer && customer.id || 0;
        this.name = customer && customer.name || '';
        this.email = customer && customer.email || '';
        this.contactNumber = customer && customer.contactNumber || '';
        this.contactPerson = customer && customer.contactPerson || '';
        this.address = customer && customer.address || '';
        this.terms = customer && customer.terms || '';
        this.discount = customer && customer.discount || '';
        this.provinceId = customer && customer.provinceId || 0;
        this.provinceName = customer && customer.provinceName || '';
        this.cityId = customer && customer.cityId || 0;
        this.cityName = customer && customer.cityName || '';
        this.priceListId = customer && customer.priceListId || price_list_1.PriceList["Main Price"];
    }
    Object.defineProperty(Customer.prototype, "selectedProvince", {
        get: function () {
            return this._selectedProvince;
        },
        set: function (prov) {
            if (prov) {
                this._selectedProvince = prov;
                this.provinceId = prov.id;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Customer.prototype, "selectedCity", {
        get: function () {
            return this._selectedCity;
        },
        set: function (city) {
            if (city) {
                this._selectedCity = city;
                this.cityId = city.id;
            }
            else {
                this._selectedCity = undefined;
                this.cityId = 0;
            }
        },
        enumerable: true,
        configurable: true
    });
    return Customer;
}());
exports.Customer = Customer;
//# sourceMappingURL=customer.js.map