import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { SalesQuotation } from '../../../shared/models/sales-quotation';
import { SalesQuotationService } from '../../../shared/services/sales-quotation.service';

@Component({
  selector: 'app-quotation-detail',
  templateUrl: './quotation-detail.component.html',
  styleUrls: ['./quotation-detail.component.css']
})
export class QuotationDetailComponent implements AfterContentInit {
  salesQuotation$: Observable<SalesQuotation>;

  constructor(
    private salesQuotationService: SalesQuotationService,
    private route: ActivatedRoute
  ) { }

  ngAfterContentInit() {
    this.loadQuotationDetails();
  };

  loadQuotationDetails() {
    const salesQuotationId = +this.route.snapshot.paramMap.get('id');
    if (salesQuotationId && salesQuotationId != 0) {
      this.salesQuotation$ = this.salesQuotationService
        .getSalesQuotationById(salesQuotationId)
        .pipe(
          map(quotation => new SalesQuotation(quotation))
        );
    }
  };
}
