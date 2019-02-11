import { JsonModelBase } from "./json-model-base";
export class Delivery extends JsonModelBase {
    get selectedProvince() {
        return this._selectedProvince;
    }
    set selectedProvince(prov) {
        if (prov) {
            this._selectedProvince = prov;
            this.provinceId = prov.id;
        }
    }
    get selectedCity() {
        return this._selectedCity;
    }
    set selectedCity(city) {
        if (city) {
            this._selectedCity = city;
            this.cityId = city.id;
        }
        else {
            this._selectedCity = undefined;
            this.cityId = 0;
        }
    }
    constructor(delivery) {
        super();
        this.id = delivery && delivery.id || 0;
        this.date = delivery && delivery.date || new Date();
        this.plateNumber = delivery && delivery.plateNumber || '';
        this.provinceId = delivery && delivery.provinceId || 0;
        this.provinceName = delivery && delivery.provinceName || '';
        this.cityId = delivery && delivery.cityId || 0;
        this.cityName = delivery && delivery.cityName || '';
        this.lineItems = delivery && delivery.lineItems || new Array();
    }
}
//# sourceMappingURL=delivery.js.map