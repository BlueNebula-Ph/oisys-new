import { VoucherCategory } from "./voucher-category";

export class CashVoucher {
  public id: number;
  public voucherNumber: number;
  public payTo: string;
  public date: Date;
  public description: string;
  public amount: number;
  public category: VoucherCategory;
  public releasedBy: string;

  constructor();
  constructor(cashVoucher: CashVoucher);
  constructor(cashVoucher?: any) {
    this.id = cashVoucher && cashVoucher.id || 0;
    this.voucherNumber = cashVoucher && cashVoucher.voucherNumber || 0;
    this.payTo = cashVoucher && cashVoucher.payTo || '';
    this.date = cashVoucher && cashVoucher.date || new Date();
    this.description = cashVoucher && cashVoucher.description || '';
    this.amount = cashVoucher && cashVoucher.amount || 0;
    this.category = cashVoucher && cashVoucher.category || VoucherCategory.Automotive;
    this.releasedBy = cashVoucher && cashVoucher.releasedBy || '';
  }
}
