import { VoucherCategory } from "./voucher-category";
export class CashVoucher {
    constructor(cashVoucher) {
        this.id = cashVoucher && cashVoucher.id || 0;
        this.voucherNumber = cashVoucher && cashVoucher.voucherNumber || 0;
        this.payTo = cashVoucher && cashVoucher.payTo || '';
        this.date = (cashVoucher && cashVoucher.date) ? new Date(cashVoucher.date) : new Date();
        this.description = cashVoucher && cashVoucher.description || '';
        this.amount = cashVoucher && cashVoucher.amount || 0;
        this.category = (cashVoucher && cashVoucher.category) ? VoucherCategory[cashVoucher.category] : VoucherCategory.Automotive;
        this.categoryName = cashVoucher && cashVoucher.category || '';
        this.releasedBy = cashVoucher && cashVoucher.releasedBy || '';
        this.rowVersion = cashVoucher && cashVoucher.rowVersion || '';
    }
}
//# sourceMappingURL=cash-voucher.js.map