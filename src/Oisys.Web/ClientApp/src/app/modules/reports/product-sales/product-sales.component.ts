import { Component, AfterContentInit } from '@angular/core';
import { Observable } from 'rxjs';

import { ReportService } from '../../../shared/services/report.service';

import { ProductSales } from '../../../shared/models/product-sales';

@Component({
  selector: 'app-product-sales',
  templateUrl: './product-sales.component.html',
  styleUrls: ['./product-sales.component.css']
})
export class ProductSalesComponent implements AfterContentInit {
  sales$: Observable<ProductSales[]>;

  dateFrom: Date;
  dateTo: Date;

  constructor(
    private reportService: ReportService
  ) { }

  ngAfterContentInit() {
    this.loadReport();
  }

  loadReport() {
    this.sales$ = this.reportService.getProductSales(this.dateFrom, this.dateTo);
  };
}
