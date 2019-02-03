import { JsonModelBase } from "./json-model-base";

export class DeliveryLineItem extends JsonModelBase {
  public id: number;

  constructor();
  constructor(deliveryLineItem: DeliveryLineItem);
  constructor(deliveryLineItem?: any) {
    super();

    this.id = deliveryLineItem && deliveryLineItem.id || 0;
  }
}
