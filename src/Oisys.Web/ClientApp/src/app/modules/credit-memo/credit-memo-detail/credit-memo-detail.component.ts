import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CreditMemo } from '../../../shared/models/credit-memo';
import { CreditMemoService } from '../../../shared/services/credit-memo.service';

@Component({
  selector: 'app-credit-memo-detail',
  templateUrl: './credit-memo-detail.component.html',
  styleUrls: ['./credit-memo-detail.component.css']
})
export class CreditMemoDetailComponent implements AfterContentInit {
  creditMemo$: Observable<CreditMemo>;

  constructor(
    private creditMemoService: CreditMemoService,
    private route: ActivatedRoute
  ) { }

  ngAfterContentInit() {
    this.loadCreditMemo();
  };

  loadCreditMemo() {
    const creditMemoId = +this.route.snapshot.paramMap.get('id');
    if (creditMemoId && creditMemoId != 0) {
      this.creditMemo$ = this.creditMemoService
        .getCreditMemoById(creditMemoId)
        .pipe(
          map(creditMemo => new CreditMemo(creditMemo))
        );
    }
  };
}
