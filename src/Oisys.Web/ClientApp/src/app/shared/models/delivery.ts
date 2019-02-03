import { JsonModelBase } from "./json-model-base";

export class Delivery extends JsonModelBase {
  public id: number;

  constructor();
  constructor(delivery: Delivery);
  constructor(delivery?: any) {
    super();

    this.id = delivery && delivery.id || 0;
  }
}
