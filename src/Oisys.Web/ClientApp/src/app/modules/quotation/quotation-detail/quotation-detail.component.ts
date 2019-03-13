import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { SalesQuotationService } from '../../../shared/services/sales-quotation.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { SalesQuotation } from '../../../shared/models/sales-quotation';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-quotation-detail',
  templateUrl: './quotation-detail.component.html',
  styleUrls: ['./quotation-detail.component.css']
})
export class QuotationDetailComponent extends PageBase implements AfterContentInit {
  salesQuotation$: Observable<SalesQuotation>;

  constructor(
    private salesQuotationService: SalesQuotationService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);
  }

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
