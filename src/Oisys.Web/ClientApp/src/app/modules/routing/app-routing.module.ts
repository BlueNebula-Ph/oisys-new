import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from '../../shared/components/home/home.component';

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

import { LoginComponent } from '../../shared/components/login/login.component';

import { AuthGuard } from '../../shared/guards/auth-guard.guard';

const adminRole = 'admin';
const canView = 'canView';
const canWrite = 'canWrite';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    pathMatch: 'full'
  },
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  // Orders
  {
    path: 'orders',
    component: OrderListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'orders/edit/:id',
    component: OrderFormComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'orders/info/:id',
    component: OrderDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  // Customers
  {
    path: 'customers',
    component: CustomerListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'customers/edit/:id',
    component: EditCustomerComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'customers/info/:id',
    component: CustomerDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  // Items
  {
    path: 'inventory',
    component: InventoryListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'inventory/edit/:id',
    component: InventoryFormComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'inventory/info/:id',
    component: InventoryDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'adjustment',
    component: AdjustItemComponent,
    canActivate: [AuthGuard],
    data: {
      type: 'adjustment',
      roles: [canWrite]
    }, 
  },
  {
    path: 'manufacture',
    component: AdjustItemComponent,
    canActivate: [AuthGuard],
    data: {
      type: 'manufacturing',
      roles: [canWrite]
    }
  },
  // Cash vouchers
  {
    path: 'vouchers',
    component: VoucherListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'vouchers/edit/:id',
    component: VoucherFormComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'vouchers/info/:id',
    component: VoucherDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  // Sales quotations
  {
    path: 'sales-quotations',
    component: QuotationListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'sales-quotations/edit/:id',
    component: QuotationFormComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'sales-quotations/info/:id',
    component: QuotationDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  // Credit memos
  {
    path: 'credit-memos',
    component: CreditMemoListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'credit-memos/edit/:id',
    component: CreditMemoFormComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'credit-memos/info/:id',
    component: CreditMemoDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  // Deliveries
  {
    path: 'deliveries',
    component: DeliveryListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'deliveries/edit/:id',
    component: DeliveryFormComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'deliveries/info/:id',
    component: DeliveryDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  // Invoices
  {
    path: 'invoices',
    component: InvoiceListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  {
    path: 'invoices/edit/:id',
    component: InvoiceFormComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canWrite]
    }
  },
  {
    path: 'invoices/info/:id',
    component: InvoiceDetailComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [canView]
    }
  },
  // Admin
  {
    path: 'categories',
    component: CategoryListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
    data: {
      roles: [adminRole]
    }
  },
  {
    path: 'provinces',
    component: ProvinceListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
    data: {
      roles: [adminRole]
    }
  },
  {
    path: 'users',
    component: UserListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
    data: {
      roles: [adminRole]
    }
  },
  // Otherwise
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
