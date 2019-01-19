export class City {
  public id: number;
  public name: string;
  public rowVersion: string;
  public isDeleted: boolean = false;

  constructor();
  constructor(city: City);
  constructor(city?: any) {
    this.id = city && city.id || 0;
    this.name = city && city.name || '';
    this.rowVersion = city && city.rowVersion || '';
  };
}
