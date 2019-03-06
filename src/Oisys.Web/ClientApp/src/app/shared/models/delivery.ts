import { JsonModelBase } from "./json-model-base";
import { Province } from "./province";
import { City } from "./city";
import { DeliveryLineItem } from "./delivery-line-item";
import { DeliveryGroupItem } from "./delivery-group-item";

export class Delivery extends JsonModelBase {
  public id: number;
  public deliveryNumber: number;
  public date: Date;
  public provinceId: number;
  public provinceName: string;
  public cityId: number;
  public cityName: string;
  public plateNumber: string;
  public lineItems: DeliveryLineItem[];

  private _province: Province;
  get province() {
    return this._province;
  }
  set province(prov: Province) {
    if (prov) {
      this._province = prov;
      this.provinceId = prov.id;
    }
  }

  private _city: City;
  get city() {
    return this._city;
  }
  set city(city: City) {
    if (city) {
      this._city = city;
      this.cityId = city.id;
    } else {
      this._city = undefined;
      this.cityId = 0;
    }
  }

  get groupedItems() {
    const groupedItems = new Array<DeliveryGroupItem>();
    if (this.lineItems && this.lineItems.length > 0) {
      this.lineItems.forEach((lineItem) => {
        var groupItem = groupedItems.find(i => i.customerName == lineItem.customer.name);
        if (groupItem) {
          groupItem.addLineItem(lineItem);
        } else {
          var address = `${lineItem.customer.address}, ${lineItem.customer.cityName}, ${lineItem.customer.provinceName}`;
          var deliveryGroupItem = new DeliveryGroupItem(lineItem.customer.name, address, [lineItem]);
          groupedItems.push(deliveryGroupItem);
        }
      });
    }
    return groupedItems;
  }

  get isNew() {
    const today = new Date();
    const sevenDaysBefore = new Date(today.setDate(today.getDate() - 7));
    return this.date > sevenDaysBefore;
  }

  constructor();
  constructor(delivery: Delivery);
  constructor(delivery?: any) {
    super();

    this.id = delivery && delivery.id || 0;
    this.deliveryNumber = delivery && delivery.deliveryNumber || 0;
    this.date = (delivery && delivery.date) ? new Date(delivery.date) : new Date();
    this.plateNumber = delivery && delivery.plateNumber || '';

    this.provinceId = delivery && delivery.provinceId || 0;
    this.provinceName = delivery && delivery.provinceName || '';

    this.cityId = delivery && delivery.cityId || 0;
    this.cityName = delivery && delivery.cityName || '';

    this.lineItems = (delivery && delivery.lineItems) ?
      delivery.lineItems.map(lineItem => new DeliveryLineItem(lineItem)) : new Array<DeliveryLineItem>();
    this.province = (delivery && delivery.province) ? new Province(delivery.province) : undefined;
    this.city = (delivery && delivery.city) ? new City(delivery.city) : undefined;

    console.log(delivery);
  }

  updateQuantity(orderLineItemId: number, newQuantity: number) {
    var deliveryLineItem = this.lineItems.find(item => item.orderLineItemId == orderLineItemId);
    deliveryLineItem.quantity = newQuantity;
  };
}
