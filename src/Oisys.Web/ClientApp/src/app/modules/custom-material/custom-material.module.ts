import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  MatButtonModule,
  MatTableModule,
  MatPaginatorModule,
  MatSortModule,
  MatInputModule,
  MatFormFieldModule,
  MatTabsModule,
  MatCardModule,
  MatSnackBarModule,
  MatSelectModule,
  MatDividerModule,
  MatRadioModule,
  MatAutocompleteModule,
} from '@angular/material';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgxDatatableModule } from '@swimlane/ngx-datatable';


@NgModule({
  imports: [
    CommonModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatFormFieldModule,
    MatTabsModule,
    MatCardModule,
    MatSnackBarModule,
    MatSelectModule,
    MatDividerModule,
    MatRadioModule,
    MatAutocompleteModule,
    BrowserAnimationsModule,
    NgxDatatableModule
  ],
  exports: [
    MatButtonModule, 
    MatTableModule, 
    MatPaginatorModule, 
    MatSortModule, 
    MatInputModule,
    MatFormFieldModule,
    MatTabsModule,
    MatCardModule,
    MatSnackBarModule,
    MatSelectModule,
    MatDividerModule,
    MatRadioModule,
    MatAutocompleteModule,
    BrowserAnimationsModule,
    NgxDatatableModule
  ],
  declarations: []
})
export class CustomMaterialModule { }
