import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../../shared/modules/shared.module';

import { CategoryListComponent } from './category-list/category-list.component';
import { EditCategoryComponent } from './edit-category/edit-category.component';
import { ProvinceListComponent } from './province-list/province-list.component';
import { EditProvinceComponent } from './edit-province/edit-province.component';
import { UserListComponent } from './user-list/user-list.component';
import { EditUserComponent } from './edit-user/edit-user.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    CategoryListComponent,
    EditCategoryComponent,
    ProvinceListComponent,
    EditProvinceComponent,
    UserListComponent,
    EditUserComponent
  ]
})
export class SettingsModule { }
