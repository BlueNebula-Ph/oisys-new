import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from '../../shared/components/home/home.component';
import { CounterComponent } from '../../shared/components/counter/counter.component';
import { FetchDataComponent } from '../../shared/components/fetch-data/fetch-data.component';

import { CategoryListComponent } from '../settings/category-list/category-list.component';
import { ProvinceListComponent } from '../settings/province-list/province-list.component';
import { UserListComponent } from '../settings/user-list/user-list.component';

import { CustomerListComponent } from '../customer/customer-list/customer-list.component';
import { CustomerDetailComponent } from '../customer/customer-detail/customer-detail.component';
import { EditCustomerComponent } from '../customer/edit-customer/edit-customer.component';

import { InventoryListComponent } from '../inventory/inventory-list/inventory-list.component';
import { InventoryFormComponent } from '../inventory/inventory-form/inventory-form.component';
import { InventoryDetailComponent } from '../inventory/inventory-detail/inventory-detail.component';
import { AdjustItemComponent } from '../inventory/adjust-item/adjust-item.component';

import { OrderListComponent } from '../order/order-list/order-list.component';
import { OrderDetailComponent } from '../order/order-detail/order-detail.component';
import { OrderFormComponent } from '../order/order-form/order-form.component';

import { VoucherListComponent } from '../voucher/voucher-list/voucher-list.component';
import { VoucherFormComponent } from '../voucher/voucher-form/voucher-form.component';
import { VoucherDetailComponent } from '../voucher/voucher-detail/voucher-detail.component';

import { QuotationListComponent } from '../quotation/quotation-list/quotation-list.component';
import { QuotationFormComponent } from '../quotation/quotation-form/quotation-form.component';
import { QuotationDetailComponent } from '../quotation/quotation-detail/quotation-detail.component';

import { CreditMemoListComponent } from '../credit-memo/credit-memo-list/credit-memo-list.component';
import { CreditMemoFormComponent } from '../credit-memo/credit-memo-form/credit-memo-form.component';
import { CreditMemoDetailComponent } from '../credit-memo/credit-memo-detail/credit-memo-detail.component';

import { DeliveryListComponent } from '../delivery/delivery-list/delivery-list.component';
import { DeliveryFormComponent } from '../delivery/delivery-form/delivery-form.component';
import { DeliveryDetailComponent } from '../delivery/delivery-detail/delivery-detail.component';

import { InvoiceListComponent } from '../invoice/invoice-list/invoice-list.component';
import { InvoiceFormComponent } from '../invoice/invoice-form/invoice-form.component';
import { InvoiceDetailComponent } from '../invoice/invoice-detail/invoice-detail.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  // Examples
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  // Orders
  { path: 'orders', component: OrderListComponent },
  { path: 'orders/edit/:id', component: OrderFormComponent },
  { path: 'orders/info/:id', component: OrderDetailComponent },
  // Customers
  { path: 'customers', component: CustomerListComponent },
  { path: 'customers/edit/:id', component: EditCustomerComponent },
  { path: 'customers/info/:id', component: CustomerDetailComponent },
  // Items
  { path: 'inventory', component: InventoryListComponent },
  { path: 'inventory/edit/:id', component: InventoryFormComponent },
  { path: 'inventory/info/:id', component: InventoryDetailComponent },
  { path: 'adjustment', component: AdjustItemComponent, data: { type: 'adjustment' } },
  { path: 'manufacture', component: AdjustItemComponent, data: { type: 'manufacturing' } },
  // Cash vouchers
  { path: 'vouchers', component: VoucherListComponent },
  { path: 'vouchers/edit/:id', component: VoucherFormComponent },
  { path: 'vouchers/info/:id', component: VoucherDetailComponent },
  // Sales quotations
  { path: 'sales-quotations', component: QuotationListComponent },
  { path: 'sales-quotations/edit/:id', component: QuotationFormComponent },
  { path: 'sales-quotations/info/:id', component: QuotationDetailComponent },
  // Credit memos
  { path: 'credit-memos', component: CreditMemoListComponent },
  { path: 'credit-memos/edit/:id', component: CreditMemoFormComponent },
  { path: 'credit-memos/info/:id', component: CreditMemoDetailComponent },
  // Deliveries
  { path: 'deliveries', component: DeliveryListComponent },
  { path: 'deliveries/edit/:id', component: DeliveryFormComponent },
  { path: 'deliveries/info/:id', component: DeliveryDetailComponent },
  // Invoices
  { path: 'invoices', component: InvoiceListComponent },
  { path: 'invoices/edit/:id', component: InvoiceFormComponent },
  { path: 'invoices/info/:id', component: InvoiceDetailComponent },
  // Admin
  { path: 'categories', component: CategoryListComponent, pathMatch: 'full' },
  { path: 'provinces', component: ProvinceListComponent, pathMatch: 'full' },
  { path: 'users', component: UserListComponent, pathMatch: 'full' },
];
 
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
