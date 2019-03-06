import { JsonModelBase } from "./json-model-base";
import { InvoiceLineItemType } from "./invoice-line-item-type";

export class InvoiceLineItem extends JsonModelBase {
  public id?: number;
  public code?: number;
  public orderId?: number;
  public creditMemoId?: number;
  public date?: Date;
  public type?: InvoiceLineItemType;
  public totalAmount?: number;

  get isOrder() {
    return this.type == InvoiceLineItemType.Order;
  }

  get isCreditMemo() {
    return this.type == InvoiceLineItemType.CreditMemo;
  }

  constructor();
  constructor(lineItem: InvoiceLineItem);
  constructor(lineItem?: any) {
    super();

    this.id = lineItem && lineItem.id || 0;
    this.code = lineItem && lineItem.code || 0;
    this.date = lineItem && lineItem.date || new Date();
    this.totalAmount = lineItem && lineItem.totalAmount || 0;

    this.type = (lineItem && lineItem.type) ? InvoiceLineItemType[lineItem.type as string] : InvoiceLineItemType.Order;

    this.orderId = lineItem && lineItem.orderId || 0;
    this.creditMemoId = lineItem && lineItem.creditMemoId || 0;
  }
}
