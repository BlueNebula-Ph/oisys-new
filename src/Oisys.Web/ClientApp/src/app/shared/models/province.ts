import { City } from "./city";

export class Province {
  public cityNames: string;

  constructor(public id?: number, public name?: string, public rowVersion?: string, public cities?: City[]) {
    this.id = id || 0;
    this.name = name || '';
    this.rowVersion = rowVersion || '';
    this.cities = cities || new Array<City>();
  };
}
