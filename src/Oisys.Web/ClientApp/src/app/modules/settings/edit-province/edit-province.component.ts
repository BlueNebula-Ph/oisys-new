import { Component, OnInit, Input, Output, ViewChild, OnDestroy, EventEmitter, ElementRef, AfterContentInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ProvinceService } from '../../../shared/services/province.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';

import { Province } from '../../../shared/models/province';
import { City } from '../../../shared/models/city';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-edit-province',
  templateUrl: './edit-province.component.html',
  styleUrls: ['./edit-province.component.css']
})
export class EditProvinceComponent implements AfterContentInit, OnDestroy {
  private _province: Province;
  @Input()
  get province() {
    return this._province;
  };
  set province(value) {
    if (value) {
      this._province = value;
      this.provinceNameField.nativeElement.focus();
    }
  };

  @Output() onProvinceSaved: EventEmitter<Province> = new EventEmitter<Province>();

  saveProvinceSub: Subscription;
  isSaving: boolean = false;
  focus: boolean = false;

  @ViewChild('provinceName') provinceNameField: ElementRef;

  constructor(
    private provinceService: ProvinceService,
    private util: UtilitiesService
  ) { }

  ngAfterContentInit() {
    this.clearProvince();
    this.provinceNameField.nativeElement.focus();
  }

  ngOnDestroy() {
    if (this.saveProvinceSub) { this.saveProvinceSub.unsubscribe(); }
  }

  saveProvince(provinceForm: NgForm) {
    if (provinceForm.valid) {
      this.isSaving = true;
      this.saveProvinceSub = this.provinceService
        .saveProvince(this.province)
        .subscribe(this.saveSuccess, this.saveFailed, this.saveCompleted);
    }
  };

  saveSuccess = (result) => {
    this.clearProvince();
    this.util.showSuccessMessage("Province saved successfully.");
    this.provinceNameField.nativeElement.focus();
    this.onProvinceSaved.emit(result);
  };

  saveFailed = (error) => {
    this.util.showErrorMessage("An error occurred while saving. Please try again.");
    console.log(error);
  };

  saveCompleted = () => {
    this.isSaving = false;
  };

  addNewCity() {
    var city = new City();
    this.province.cities.push(city);
    this.focus = true;
  };

  clearProvince() {
    this.province = new Province();
  };

  deleteCity(city) {
    city.isDeleted = true;
  };

  undoDelete(city) {
    city.isDeleted = false;
  };
}
