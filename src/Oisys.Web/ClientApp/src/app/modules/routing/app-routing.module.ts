import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from '../../shared/components/home/home.component';
import { CounterComponent } from '../../shared/components/counter/counter.component';
import { FetchDataComponent } from '../../shared/components/fetch-data/fetch-data.component';
import { OrderListComponent } from '../../shared/components/order-list/order-list.component';
import { CustomerListComponent } from '../customer/customer-list/customer-list.component';
import { ViewSettingsComponent } from '../settings/view-settings/view-settings.component';
import { EditCustomerComponent } from '../customer/edit-customer/edit-customer.component';
import { CategoryListComponent } from '../settings/category-list/category-list.component';
import { ProvinceListComponent } from '../settings/province-list/province-list.component';
import { UserListComponent } from '../settings/user-list/user-list.component';
import { OrderDetailComponent } from '../order/order-detail/order-detail.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  // Examples
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  // Orders
  { path: 'orders', component: OrderListComponent },
  { path: 'orders', component: OrderDetailComponent },
  { path: 'orders', component: OrderListComponent },
  // Customers
  { path: 'customers', component: CustomerListComponent },
  { path: 'customers/edit/:id', component: EditCustomerComponent },
  // Admin
  { path: 'categories', component: CategoryListComponent, pathMatch: 'full' },
  { path: 'cities', component: ProvinceListComponent, pathMatch: 'full' },
  { path: 'users', component: UserListComponent, pathMatch: 'full' },
];
 
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
