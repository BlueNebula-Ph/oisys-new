import { AdjustmentType } from "./adjustment-type";
import { Item } from "./item";

export class Adjustment {
  public id: number;
  public itemId: number;
  public adjustmentType: number;
  public adjustmentQuantity: number;
  public remarks: string;
  public operator: string;
  public machine: string;

  private _selectedItem: Item;
  get selectedItem() {
    return this._selectedItem;
  }
  set selectedItem(item: Item) {
    if (item) {
      this._selectedItem = item;
      this.itemId = item.id;
    }
  }

  constructor();
  constructor(adjustment: Adjustment);
  constructor(adjustment?: any) {
    this.id = adjustment && adjustment.id || 0;
    this.adjustmentType = adjustment && adjustment.adjustmentType || AdjustmentType['Add'];
    this.adjustmentQuantity = adjustment && adjustment.adjustmentQuantity || 0;
    this.remarks = adjustment && adjustment.remarks || '';
    this.operator = adjustment && adjustment.operator || '';
    this.machine = adjustment && adjustment.machine || '';
  };
}
