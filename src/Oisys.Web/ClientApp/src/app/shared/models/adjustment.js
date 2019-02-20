import { AdjustmentType } from "./adjustment-type";
export class Adjustment {
    get selectedItem() {
        return this._selectedItem;
    }
    set selectedItem(item) {
        if (item) {
            this._selectedItem = item;
            this.itemId = item.id;
        }
    }
    constructor(adjustment) {
        this.id = adjustment && adjustment.id || 0;
        this.adjustmentType = adjustment && adjustment.adjustmentType || AdjustmentType['Add'];
        this.adjustmentQuantity = adjustment && adjustment.adjustmentQuantity || 0;
        this.remarks = adjustment && adjustment.remarks || '';
        this.operator = adjustment && adjustment.operator || '';
        this.machine = adjustment && adjustment.machine || '';
    }
    ;
}
//# sourceMappingURL=adjustment.js.map