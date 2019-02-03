import { JsonModelBase } from "./json-model-base";

export class Invoice extends JsonModelBase {
  public id: number;

  constructor();
  constructor(invoice: Invoice);
  constructor(invoice?: any) {
    super();

    this.id = invoice && invoice.id || 0;
  }
}
