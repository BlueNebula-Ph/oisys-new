import { JsonModelBase } from "./json-model-base";

export class CreditMemoLineItem extends JsonModelBase {
  public id: number;


  constructor();
  constructor(creditMemoLineItem: CreditMemoLineItem);
  constructor(creditMemoLineItem?: any) {
    super();
  }
}
