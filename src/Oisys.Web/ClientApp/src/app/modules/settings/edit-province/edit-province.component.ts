import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ProvinceService } from '../../../shared/services/province.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Province } from '../../../shared/models/province';
import { City } from '../../../shared/models/city';

@Component({
  selector: 'app-edit-province',
  templateUrl: './edit-province.component.html',
  styleUrls: ['./edit-province.component.css']
})
export class EditProvinceComponent implements OnInit {
  @Input() province: Province = new Province();
  @Output() onProvinceSaved: EventEmitter<Province> = new EventEmitter<Province>();

  isSaving: boolean = false;

  constructor(private provinceService: ProvinceService, private util: UtilitiesService) { }

  ngOnInit() {
    this.clearProvince();
  }

  saveProvince(provinceForm: NgForm) {
    if (provinceForm.valid) {
      this.provinceService
        .saveProvince(this.province)
        .subscribe(result => {
          this.clearProvince();
          this.onProvinceSaved.emit(result);

          provinceForm.resetForm();
        });
    }
  }

  addNewCity() {
    var city = new City();
    this.province.cities.push(city);
  }

  clearProvince() {
    this.province = new Province();
    this.addNewCity();
  }

  deleteCity(city) {
    city.isDeleted = true;
  }

  undoDelete(city) {
    city.isDeleted = false;
  }
}
