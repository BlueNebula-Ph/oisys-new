import { City } from "./city";

export class Province {
  public cityNames: string;

  get citiesValid(): boolean {
    return this.cities.some((city) => { return city.name != '' && !city.isDeleted; });
  }

  constructor(public id?: number, public name?: string, public rowVersion?: string, public cities?: City[]) {
    this.id = this.id || 0;
    this.name = this.name || '';
    this.rowVersion = this.rowVersion || '';
    this.cities = this.cities || new Array<City>();
  };
}
