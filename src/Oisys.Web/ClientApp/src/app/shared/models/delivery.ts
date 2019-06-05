import { JsonModelBase } from "./json-model-base";
import { Province } from "./province";
import { City } from "./city";
import { DeliveryLineItem } from "./delivery-line-item";
import { DeliveryGroupItem } from "./delivery-group-item";
import { DeliveryGroupCategory } from "./delivery-group-category";

export class Delivery extends JsonModelBase {
  public id: number;
  public deliveryNumber: number;
  public date: Date;
  public provinceId: number;
  public provinceName: string;
  public cityId: number;
  public cityName: string;
  public plateNumber: string;
  
  public groupedItems: DeliveryGroupItem[] = new Array<DeliveryGroupItem>();
  public groupedCategories: DeliveryGroupCategory[] = new Array<DeliveryGroupCategory>();

  public rowVersion: string;

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

  get isNew() {
    const today = new Date();
    const sevenDaysBefore = new Date(today.setDate(today.getDate() - 7));
    return this.date > sevenDaysBefore;
  }

  private _lineItems: DeliveryLineItem[];
  get lineItems() {
    return this._lineItems;
  }
  set lineItems(val: DeliveryLineItem[]) {
    this._lineItems = val;
    this.groupLineItems();
  }

  constructor();
  constructor(delivery: Delivery);
  constructor(delivery?: any) {
    super();

    this.id = delivery && delivery.id || 0;
    this.deliveryNumber = delivery && delivery.deliveryNumber || 0;
    this.date = (delivery && delivery.date) ? new Date(delivery.date) : new Date();
    this.plateNumber = delivery && delivery.plateNumber || '';
    this.rowVersion = delivery && delivery.rowVersion || '';

    this.provinceId = delivery && delivery.provinceId || 0;
    this.provinceName = delivery && delivery.provinceName || '';

    this.cityId = delivery && delivery.cityId || 0;
    this.cityName = delivery && delivery.cityName || '';

    this.lineItems = (delivery && delivery.lineItems) ?
      delivery.lineItems.map(lineItem => new DeliveryLineItem(lineItem)) : new Array<DeliveryLineItem>();
    this.province = (delivery && delivery.province) ? new Province(delivery.province) : undefined;
    this.city = (delivery && delivery.city) ? new City(delivery.city) : undefined;
  }

  updateQuantity(orderLineItemId: number, newQuantity: number) {
    const deliveryLineItem = this.lineItems.find(item => item.orderLineItemId == orderLineItemId);
    deliveryLineItem.quantity = newQuantity;
  };

  groupLineItems() {
    this.groupedItems = new Array<DeliveryGroupItem>();
    this.groupedCategories = new Array<DeliveryGroupCategory>();

    if (this.lineItems && this.lineItems.length > 0) {
      this.lineItems.forEach((lineItem) => {
        // Group line items per item name
        const groupItem = this.groupedItems.find(i => i.itemCode === lineItem.itemCode && i.itemName == lineItem.itemName && i.categoryName == lineItem.categoryName);
        if (groupItem) {
          groupItem.addLineItem(lineItem);
        } else {
          const deliveryGroupItem = new DeliveryGroupItem(lineItem.itemCode, lineItem.itemName, lineItem.categoryName,[lineItem]);
          this.groupedItems.push(deliveryGroupItem);
        }

        // Group line items per category
        const groupCategory = this.groupedCategories.find(i => i.categoryName == lineItem.categoryName);
        if (groupCategory) {
          groupCategory.addLineItem(lineItem);
        } else {
          const deliveryGroupCategory = new DeliveryGroupCategory(lineItem.categoryName, [lineItem]);
          this.groupedCategories.push(deliveryGroupCategory);
        }
      });

      // Add total items
      const lineItem = new DeliveryLineItem();
      lineItem.quantity = this.groupedItems.length;
      const itemCategory = new DeliveryGroupCategory("Items", [lineItem]);
      this.groupedCategories.push(itemCategory);
    }
  };
}
