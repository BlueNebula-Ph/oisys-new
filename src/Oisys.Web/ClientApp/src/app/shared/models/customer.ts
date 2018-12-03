import { PriceList } from "./price-list";
import { Province } from "./Province";
import { City } from "./city";

export class Customer {
  private _selectedProvince: Province;
  get selectedProvince() {
    return this._selectedProvince;
  }
  set selectedProvince(prov: Province) {
    if (prov) {
      this._selectedProvince = prov;
      this.provinceId = prov.id;
    }
  }

  private _selectedCity: City;
  get selectedCity() {
    return this._selectedCity;
  }
  set selectedCity(city: City) {
    if (city) {
      this._selectedCity = city;
      this.cityId = city.id;
    } else {
      this._selectedCity = undefined;
      this.cityId = 0;
    }
  }

  constructor(public id?: number,
    public name?: string,
    public email?: string,
    public contactNumber?: string,
    public contactPerson?: string,
    public address?: string,
    public cityId?: number,
    public provinceId?: number,
    public terms?: string,
    public discount?: number,
    public priceListId?: number)
  {
    if (!id) {
      id = 0;
    }

    if (!priceListId) {
      priceListId = PriceList["Main Price"];
    }
  }
}
