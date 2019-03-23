import { Component, AfterContentInit } from '@angular/core';
import { Observable } from 'rxjs';

import { ReportService } from '../../../shared/services/report.service';

import { ItemCount } from '../../../shared/models/item-count';

@Component({
  selector: 'app-count-sheet',
  templateUrl: './count-sheet.component.html',
  styleUrls: ['./count-sheet.component.css']
})
export class CountSheetComponent implements AfterContentInit {
  items$: Observable<ItemCount[]>;

  constructor(
    private reportService: ReportService
  ) { }

  ngAfterContentInit() {
    this.loadReport();
  }

  loadReport() {
    this.items$ = this.reportService.getCountSheet();
  };
}
