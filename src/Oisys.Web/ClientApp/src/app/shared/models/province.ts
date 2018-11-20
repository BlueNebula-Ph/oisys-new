import { City } from "./city";

export class Province {
  public cityNames: string;

  constructor(public id?: number, public name?: string, public rowVersion?: string, public cities?: City[]) {
    id = id || 0;
    name = name || '';
    rowVersion = rowVersion || '';
    cities = cities || new Array<City>();
  };
}
