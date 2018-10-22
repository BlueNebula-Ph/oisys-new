export class Customer {
  constructor(public id?: number,
    public name?: string,
    public email?: string,
    public contactNumber?: string,
    public contactPerson?: string,
    public address?: string,
    public cityId?: number,
    public provinceId?: number,
    public terms?: string,
    public discount?: string,
    public priceList?: string,
    public keywords?: string) { }
}
