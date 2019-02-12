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

  //constructor();
  constructor(lineItem: InvoiceLineItem = {} as InvoiceLineItem) {
    super();

    this.id = lineItem.id || 0;
    this.code = lineItem.code || 0;
    this.date = lineItem.date || new Date();
    this.totalAmount = lineItem.totalAmount || 0;

    this.orderId = lineItem.orderId || 0;
    this.creditMemoId = lineItem.creditMemoId || 0;
  }
}
