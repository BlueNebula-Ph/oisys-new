import { City } from "./city";

export class Province {
  public id: number;
  public name: string;
  public rowVersion: string;
  public cities: City[];

  get citiesValid(): boolean {
    return this.cities.some((city) => { return city.name != '' && !city.isDeleted; });
  }

  get cityNames(): string {
    return this.cities.map((sc) => sc.name).join(', ');
  };

  constructor()
  constructor(province: Province)
  constructor(province?: any) {
    this.id = province && province.id || 0;
    this.name = province && province.name || '';
    this.rowVersion = province && province.rowVersion || '';
    this.cities = province && province.cities || new Array<City>();
  };
}
