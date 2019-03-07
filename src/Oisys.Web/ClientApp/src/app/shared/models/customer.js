import { PriceList } from "./price-list";
import { Province } from "./province";
import { City } from "./city";
export class Customer {
    get province() {
        return this._province;
    }
    set province(prov) {
        if (prov) {
            this._province = prov;
            this.provinceId = prov.id;
        }
    }
    get city() {
        return this._city;
    }
    set city(city) {
        if (city) {
            this._city = city;
            this.cityId = city.id;
        }
        else {
            this._city = undefined;
            this.cityId = 0;
        }
    }
    constructor(customer) {
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
        this.priceListId = customer && customer.priceListId || PriceList["Main Price"];
        this.priceList = (customer && customer.priceListId) ? PriceList[customer.priceListId] : '';
        this.province = (customer && customer.province) ? new Province(customer.province) : undefined;
        this.city = (customer && customer.city) ? new City(customer.city) : undefined;
    }
}
//# sourceMappingURL=customer.js.map