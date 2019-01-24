import { PriceList } from "./price-list";
import { JsonModelBase } from "./json-model-base";
export class LineItem extends JsonModelBase {
    get selectedItem() {
        return this._selectedItem;
    }
    set selectedItem(item) {
        if (item) {
            this._selectedItem = item;
            this.itemId = item.id;
        }
    }
    get unit() {
        return this._selectedItem && this._selectedItem.unit || '';
    }
    get categoryName() {
        return this._selectedItem && this._selectedItem.categoryName || '';
    }
    get unitPrice() {
        if (this._unitPrice && this._unitPrice != 0) {
            return this._unitPrice;
        }
        var unitPrice = 0;
        if (this._selectedItem && this.itemId != 0) {
            switch (this.priceList) {
                case PriceList["Main Price"]:
                    unitPrice = this._selectedItem.mainPrice;
                    break;
                case PriceList["Walk-In Price"]:
                    unitPrice = this._selectedItem.walkInPrice;
                    break;
                case PriceList["N.E. Price"]:
                    unitPrice = this._selectedItem.nePrice;
                    break;
            }
        }
        return unitPrice;
    }
    set unitPrice(value) {
        this._unitPrice = value;
    }
    get totalPrice() {
        return this.quantity * this.unitPrice;
    }
    constructor(lineItem) {
        super();
        this.id = lineItem && lineItem.id || 0;
        this.quantity = lineItem && lineItem.quantity || 0;
        this.itemId = lineItem && lineItem.itemId || 0;
        this.itemName = lineItem && lineItem.itemName || '';
        this.priceList = lineItem && lineItem.priceList || PriceList["Main Price"];
        this.unitPrice = lineItem && lineItem.unitPrice || 0;
    }
    updatePriceList(priceList) {
        this.priceList = priceList || PriceList["Main Price"];
    }
    ;
}
//# sourceMappingURL=line-item.js.map