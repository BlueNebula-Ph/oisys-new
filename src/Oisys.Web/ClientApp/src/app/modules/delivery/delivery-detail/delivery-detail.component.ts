import { Component, AfterContentInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { DeliveryService } from '../../../shared/services/delivery.service';
import { AuthenticationService } from '../../../shared/services/authentication.service';

import { Delivery } from '../../../shared/models/delivery';

import { PageBase } from '../../../shared/helpers/page-base';

@Component({
  selector: 'app-delivery-detail',
  templateUrl: './delivery-detail.component.html',
  styleUrls: ['./delivery-detail.component.css']
})
export class DeliveryDetailComponent extends PageBase implements AfterContentInit {
  delivery$: Observable<Delivery>;

  constructor(
    private deliveryService: DeliveryService,
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {
    super(authService);
  }

  ngAfterContentInit() {
    this.loadDelivery();
  };

  loadDelivery() {
    const deliveryId = +this.route.snapshot.paramMap.get('id');
    if (deliveryId && deliveryId != 0) {
      this.delivery$ = this.deliveryService
        .getDeliveryById(deliveryId)
        .pipe(
          map(delivery => new Delivery(delivery))
        );
    }
  };
}
