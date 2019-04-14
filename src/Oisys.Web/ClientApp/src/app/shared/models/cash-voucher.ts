import { VoucherCategory } from "./voucher-category";

export class CashVoucher {
  public id: number;
  public voucherNumber: number;
  public payTo: string;
  public date: Date;
  public description: string;
  public amount: number;
  public category: VoucherCategory;
  public categoryName: string;
  public releasedBy: string;
  public rowVersion: string;

  constructor();
  constructor(cashVoucher: CashVoucher);
  constructor(cashVoucher?: any) {
    this.id = cashVoucher && cashVoucher.id || 0;
    this.voucherNumber = cashVoucher && cashVoucher.voucherNumber || 0;
    this.payTo = cashVoucher && cashVoucher.payTo || '';
    this.date = (cashVoucher && cashVoucher.date) ? new Date(cashVoucher.date) : new Date();
    this.description = cashVoucher && cashVoucher.description || '';
    this.amount = cashVoucher && cashVoucher.amount || 0;
    this.category = (cashVoucher && cashVoucher.category) ? VoucherCategory[cashVoucher.category as string] : VoucherCategory.Automotive;
    this.categoryName = cashVoucher && cashVoucher.category || '';
    this.releasedBy = cashVoucher && cashVoucher.releasedBy || '';
    this.rowVersion = cashVoucher && cashVoucher.rowVersion || '';
  }
}
