import { JsonModelBase } from "./json-model-base";
import { Province } from "./Province";
import { City } from "./city";
import { DeliveryLineItem } from "./delivery-line-item";

export class Delivery extends JsonModelBase {
  public id: number;
  public date: Date;
  public provinceId: number;
  public provinceName: string;
  public cityId: number;
  public cityName: string;
  public plateNumber: string;
  public lineItems: DeliveryLineItem[];

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
  constructor(delivery: Delivery);
  constructor(delivery?: any) {
    super();

    this.id = delivery && delivery.id || 0;
    this.date = delivery && delivery.date || new Date();
    this.plateNumber = delivery && delivery.plateNumber || '';

    this.provinceId = delivery && delivery.provinceId || 0;
    this.provinceName = delivery && delivery.provinceName || '';

    this.cityId = delivery && delivery.cityId || 0;
    this.cityName = delivery && delivery.cityName || '';

    this.lineItems = delivery && delivery.lineItems || new Array<DeliveryLineItem>();
  }
}
