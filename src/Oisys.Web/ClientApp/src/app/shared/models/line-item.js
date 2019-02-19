import { PriceList } from "./price-list";
import { Item } from "./item";
import { JsonModelBase } from "./json-model-base";
export class LineItem extends JsonModelBase {
    get item() {
        return this._item;
    }
    set item(item) {
        if (item) {
            this._item = item;
            this.itemId = item.id;
        }
    }
    get unit() {
        return this._item && this._item.unit || '';
    }
    get categoryName() {
        return this._item && this._item.categoryName || '';
    }
    get unitPrice() {
        if (this._unitPrice && this._unitPrice != 0) {
            return this._unitPrice;
        }
        var unitPrice = 0;
        if (this._item && this.itemId != 0) {
            switch (this.priceList) {
                case PriceList["Main Price"]:
                    unitPrice = this._item.mainPrice;
                    break;
                case PriceList["Walk-In Price"]:
                    unitPrice = this._item.walkInPrice;
                    break;
                case PriceList["N.E. Price"]:
                    unitPrice = this._item.nePrice;
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
        this.item = (lineItem && lineItem.item) ? new Item(lineItem.item) : undefined;
    }
    updatePriceList(priceList) {
        this.priceList = priceList || PriceList["Main Price"];
    }
    ;
}
//# sourceMappingURL=line-item.js.map