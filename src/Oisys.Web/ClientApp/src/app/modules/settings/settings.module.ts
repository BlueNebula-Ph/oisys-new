import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewSettingsComponent } from './view-settings/view-settings.component';
import { CategoryListComponent } from './category-list/category-list.component';
import { CustomMaterialModule } from '../custom-material/custom-material.module';
import { EditCategoryComponent } from './edit-category/edit-category.component';
import { ProvinceListComponent } from './province-list/province-list.component';
import { EditProvinceComponent } from './edit-province/edit-province.component';
import { FormsModule } from '@angular/forms';
import { UserListComponent } from './user-list/user-list.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    CustomMaterialModule
  ],
  declarations: [ViewSettingsComponent, CategoryListComponent, EditCategoryComponent, ProvinceListComponent, EditProvinceComponent, UserListComponent]
})
export class SettingsModule { }
