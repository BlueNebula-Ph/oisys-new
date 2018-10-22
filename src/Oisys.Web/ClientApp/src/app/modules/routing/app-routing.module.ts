import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from '../../shared/components/home/home.component';
import { CounterComponent } from '../../shared/components/counter/counter.component';
import { FetchDataComponent } from '../../shared/components/fetch-data/fetch-data.component';
import { OrderListComponent } from '../../shared/components/order-list/order-list.component';
import { CustomerListComponent } from '../customer/customer-list/customer-list.component';
import { ViewSettingsComponent } from '../settings/view-settings/view-settings.component';
import { EditCustomerComponent } from '../customer/edit-customer/edit-customer.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'order-list', component: OrderListComponent },
  { path: 'customers', component: CustomerListComponent },
  { path: 'customers/edit/:id', component: EditCustomerComponent },
  { path: 'settings', component: ViewSettingsComponent}
];
 
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
