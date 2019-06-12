import { JsonModelBase } from "./json-model-base";
import { DeliveryLineItem } from "./delivery-line-item";
import { DeliveryGroupItem } from "./delivery-group-item";
import { DeliveryGroupCategory } from "./delivery-group-category";

export class Delivery extends JsonModelBase {
  public id: number;
  public deliveryNumber: number;
  public date: Date;
  public plateNumber: string;
  public deliveryAreas: string;

  public groupedItems: DeliveryGroupItem[] = new Array<DeliveryGroupItem>();
  public groupedCategories: DeliveryGroupCategory[] = new Array<DeliveryGroupCategory>();
  public deliveryAreaList: string[] = new Array<string>();

  public rowVersion: string;

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
    this.deliveryAreas = delivery && delivery.deliveryAreas || '';
    this.rowVersion = delivery && delivery.rowVersion || '';

    this.lineItems = (delivery && delivery.lineItems) ?
      delivery.lineItems.map(lineItem => new DeliveryLineItem(lineItem)) : new Array<DeliveryLineItem>();
  }

  updateQuantity(orderLineItemId: number, newQuantity: number) {
    const deliveryLineItem = this.lineItems.find(item => item.orderLineItemId == orderLineItemId);
    deliveryLineItem.quantity = newQuantity;
  };

  groupLineItems() {
    this.groupedItems = new Array<DeliveryGroupItem>();
    this.groupedCategories = new Array<DeliveryGroupCategory>();
    this.deliveryAreaList = new Array<string>();

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

        // Get line item areas
        const lineItemArea = `${lineItem.customer.cityName}, ${lineItem.customer.provinceName}`;
        const area = this.deliveryAreaList.find(i => i == lineItemArea);
        if (!area) {
          this.deliveryAreaList.push(lineItemArea);
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
