import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { Invoice } from '../../../shared/models/invoice';
import { InvoiceService } from '../../../shared/services/invoice.service';

@Component({
  selector: 'app-invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.css']
})
export class InvoiceDetailComponent implements AfterContentInit {
  invoice$: Observable<Invoice>;

  constructor(
    private invoiceService: InvoiceService,
    private route: ActivatedRoute
  ) { }

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
