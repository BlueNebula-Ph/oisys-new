import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppHeaderModule, AppSidebarModule, AppBreadcrumbModule, AppFooterModule } from '@coreui/angular';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './modules/routing/app-routing.module';

import { AppComponent } from './shared/components/app/app.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';

import { HomeComponent } from './shared/components/home/home.component';
import { FetchDataComponent } from './shared/components/fetch-data/fetch-data.component';
import { OrderListComponent } from './shared/components/order-list/order-list.component';
import { CounterComponent } from './shared/components/counter/counter.component';
import { CustomerModule } from './modules/customer/customer.module';
import { InventoryModule } from './modules/inventory/inventory.module';
import { OrderModule } from './modules/order/order.module';
import { InvoiceModule } from './modules/invoice/invoice.module';
import { DeliveryModule } from './modules/delivery/delivery.module';
import { CreditMemoModule } from './modules/credit-memo/credit-memo.module';
import { VoucherModule } from './modules/voucher/voucher.module';
import { QuotationModule } from './modules/quotation/quotation.module';
import { CategoryModule } from './modules/category/category.module';
import { SettingsModule } from './modules/settings/settings.module';
import { CustomMaterialModule } from './modules/custom-material/custom-material.module';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    OrderListComponent,
    CounterComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    CustomerModule,
    InventoryModule,
    OrderModule,
    InvoiceModule,
    DeliveryModule,
    CreditMemoModule,
    VoucherModule,
    QuotationModule,
    CategoryModule,
    SettingsModule,
    CustomMaterialModule,
    AppHeaderModule,
    AppSidebarModule,
    AppFooterModule,
    AppBreadcrumbModule.forRoot(),
    PerfectScrollbarModule,
    NgxDatatableModule,
    ToastrModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
