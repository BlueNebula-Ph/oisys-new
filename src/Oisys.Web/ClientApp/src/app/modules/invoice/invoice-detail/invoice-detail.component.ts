import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { InvoiceService } from '../../../shared/services/invoice.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { Invoice } from '../../../shared/models/invoice';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.css']
})
export class InvoiceDetailComponent extends PageBase implements AfterContentInit {
  invoice$: Observable<Invoice>;

  constructor(
    private invoiceService: InvoiceService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);
  }

  ngAfterContentInit() {
    this.loadInvoice();
  }

  loadInvoice() {
    const invoiceId = +this.route.snapshot.paramMap.get('id');
    if (invoiceId && invoiceId != 0) {
      this.invoice$ = this.invoiceService
        .getInvoiceById(invoiceId)
        .pipe(
          map(invoice => new Invoice(invoice))
        );
    }
  };
}
