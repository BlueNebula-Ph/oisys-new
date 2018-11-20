import { PriceList } from "./price-list";
import { Province } from "./Province";
import { City } from "./city";

export class Customer {
  private _selectedProvince: Province;
  get selectedProvince() {
    return _selectedProvince;
  }
  set selectedProvince(prov: Province) {
    if (prov) {
      _selectedProvince = prov;
      provinceId = prov.id;
    }
  }

  private _selectedCity: City;
  get selectedCity() {
    return _selectedCity;
  }
  set selectedCity(city: City) {
    if (city) {
      _selectedCity = city;
      cityId = city.id;
    } else {
      _selectedCity = undefined;
      cityId = 0;
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
