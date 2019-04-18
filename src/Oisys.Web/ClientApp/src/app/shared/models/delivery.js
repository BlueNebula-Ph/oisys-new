import { JsonModelBase } from "./json-model-base";
import { Province } from "./province";
import { City } from "./city";
import { DeliveryLineItem } from "./delivery-line-item";
import { DeliveryGroupItem } from "./delivery-group-item";
export class Delivery extends JsonModelBase {
    constructor(delivery) {
        super();
        this.groupedItems = new Array();
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
            delivery.lineItems.map(lineItem => new DeliveryLineItem(lineItem)) : new Array();
        this.province = (delivery && delivery.province) ? new Province(delivery.province) : undefined;
        this.city = (delivery && delivery.city) ? new City(delivery.city) : undefined;
    }
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
    get isNew() {
        const today = new Date();
        const sevenDaysBefore = new Date(today.setDate(today.getDate() - 7));
        return this.date > sevenDaysBefore;
    }
    get lineItems() {
        return this._lineItems;
    }
    set lineItems(val) {
        this._lineItems = val;
        this.groupLineItems();
    }
    updateQuantity(orderLineItemId, newQuantity) {
        var deliveryLineItem = this.lineItems.find(item => item.orderLineItemId == orderLineItemId);
        deliveryLineItem.quantity = newQuantity;
    }
    ;
    groupLineItems() {
        this.groupedItems = new Array();
        if (this.lineItems && this.lineItems.length > 0) {
            this.lineItems.forEach((lineItem) => {
                var groupItem = this.groupedItems.find(i => i.customerName == lineItem.customer.name);
                if (groupItem) {
                    groupItem.addLineItem(lineItem);
                }
                else {
                    var address = `${lineItem.customer.address}, ${lineItem.customer.cityName}, ${lineItem.customer.provinceName}`;
                    var deliveryGroupItem = new DeliveryGroupItem(lineItem.customer.id, lineItem.customer.name, address, [lineItem]);
                    this.groupedItems.push(deliveryGroupItem);
                }
            });
        }
    }
    ;
}
//# sourceMappingURL=delivery.js.map