import { PriceList } from "./price-list";
import { Province } from "./Province";
import { City } from "./city";

export class Customer {
  public id: number;
  public name: string;
  public email: string;
  public contactNumber: string;
  public contactPerson: string;
  public address: string;
  public cityId: number;
  public provinceId: number;
  public terms: string;
  public discount: number;
  public priceListId: number;
  public provinceName: string;
  public cityName: string;

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

  constructor();
  constructor(customer: Customer);
  constructor(customer?: any) {
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
  }
}
