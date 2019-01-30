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
  public category: string;
  public quantityReturned: number;
  public quantityDelivered: number;

  get totalPrice() {
    return this.quantity * this.unitPrice;
  }
}
