import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ProvinceService } from '../../../shared/services/province.service';
import { Province } from '../../../shared/models/Province';
import { City } from '../../../shared/models/city';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-edit-province',
  templateUrl: './edit-province.component.html',
  styleUrls: ['./edit-province.component.css']
})
export class EditProvinceComponent implements OnInit {
  @Input() province: Province;
  @Output() onProvinceSaved: EventEmitter<Province> = new EventEmitter<Province>();

  constructor(private provinceService: ProvinceService) { }

  ngOnInit() {
    clearProvince();
  }

  saveProvince(provinceForm: NgForm) {
    if (provinceForm.valid) {
      provinceService
        .saveProvince(province)
        .subscribe(result => {
          clearProvince();
          onProvinceSaved.emit(result);

          provinceForm.resetForm();
        });
    }
  }

  addNewCity() {
    var city = new City();
    province.cities.push(city);
  }

  clearProvince() {
    province = new Province();
  }
}
