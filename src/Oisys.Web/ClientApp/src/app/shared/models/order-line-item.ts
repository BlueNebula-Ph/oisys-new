export class OrderLineItem {
  public id: number;
  public orderCode: string;
  public orderDate: string;
  public itemId: number;
  public itemCode: string;
  public itemName: string;
  public quantity: number;
  public unit: string;
  public unitPrice: number;
  public categoryName: string;
  public quantityReturned: number;
  public quantityDelivered: number;
  public discountedUnitPrice: number;

  get totalPrice() {
    return this.quantity * this.unitPrice;
  }

  get quantityNotDelivered() {
    return this.quantity - this.quantityDelivered;
  }

  constructor();
  constructor(orderLineItem: OrderLineItem);
  constructor(orderLineItem?: any) {
    this.id = orderLineItem && orderLineItem.id || 0;
    this.orderCode = orderLineItem && orderLineItem.orderCode || '';
    this.orderDate = orderLineItem && orderLineItem.orderDate || '';
    this.itemId = orderLineItem && orderLineItem.itemId || 0;
    this.itemCode = orderLineItem && orderLineItem.itemCode || '';
    this.itemName = orderLineItem && orderLineItem.itemName || '';
    this.quantity = orderLineItem && orderLineItem.quantity || 0;
    this.unit = orderLineItem && orderLineItem.unit || '';
    this.unitPrice = orderLineItem && orderLineItem.unitPrice || 0;
    this.categoryName = orderLineItem && orderLineItem.categoryName || '';
    this.quantityReturned = orderLineItem && orderLineItem.quantityReturned || 0;
    this.quantityDelivered = orderLineItem && orderLineItem.quantityDelivered || 0;
    this.discountedUnitPrice = orderLineItem && orderLineItem.discountedUnitPrice || 0;
  }
}
