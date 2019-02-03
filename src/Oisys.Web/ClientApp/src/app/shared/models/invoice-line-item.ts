import { JsonModelBase } from "./json-model-base";

export class InvoiceLineItem extends JsonModelBase {
  public id: number;

  constructor();
  constructor(invoiceLineItem: InvoiceLineItem);
  constructor(invoiceLineItem?: any) {
    super();

    this.id = invoiceLineItem && invoiceLineItem.id || 0;
  }
}
