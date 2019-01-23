"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var voucher_category_1 = require("./voucher-category");
var CashVoucher = /** @class */ (function () {
    function CashVoucher(cashVoucher) {
        this.id = cashVoucher && cashVoucher.id || 0;
        this.voucherNumber = cashVoucher && cashVoucher.voucherNumber || 0;
        this.payTo = cashVoucher && cashVoucher.payTo || '';
        this.date = cashVoucher && cashVoucher.date || new Date();
        this.description = cashVoucher && cashVoucher.description || '';
        this.amount = cashVoucher && cashVoucher.amount || 0;
        this.category = cashVoucher && cashVoucher.category || voucher_category_1.VoucherCategory.Automotive;
        this.releasedBy = cashVoucher && cashVoucher.releasedBy || '';
    }
    return CashVoucher;
}());
exports.CashVoucher = CashVoucher;
//# sourceMappingURL=cash-voucher.js.map