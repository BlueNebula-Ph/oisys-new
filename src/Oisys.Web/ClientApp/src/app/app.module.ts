import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppHeaderModule, AppSidebarModule, AppBreadcrumbModule, AppFooterModule } from '@coreui/angular';

import { PerfectScrollbarModule, PERFECT_SCROLLBAR_CONFIG, PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ToastrModule } from 'ngx-toastr';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';

import { CurrencyMaskModule } from 'ng2-currency-mask';
import { CurrencyMaskConfig, CURRENCY_MASK_CONFIG } from 'ng2-currency-mask/src/currency-mask.config';

import { AppRoutingModule } from './modules/routing/app-routing.module';
import { CustomerModule } from './modules/customer/customer.module';
import { InventoryModule } from './modules/inventory/inventory.module';
import { OrderModule } from './modules/order/order.module';
import { InvoiceModule } from './modules/invoice/invoice.module';
import { DeliveryModule } from './modules/delivery/delivery.module';
import { CreditMemoModule } from './modules/credit-memo/credit-memo.module';
import { VoucherModule } from './modules/voucher/voucher.module';
import { QuotationModule } from './modules/quotation/quotation.module';
import { SettingsModule } from './modules/settings/settings.module';
import { SharedModule } from './shared/modules/shared.module';

import { AppComponent } from './shared/components/app/app.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';

import { HomeComponent } from './shared/components/home/home.component';
import { LoginComponent } from './shared/components/login/login.component';
import { JwtInterceptor } from './shared/interceptors/jwt.interceptor';
import { ErrorInterceptor } from './shared/interceptors/error.interceptor';
import { ReportsModule } from './modules/reports/reports.module';

export const CustomScrollConfig: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

export const CustomCurrencyMaskConfig: CurrencyMaskConfig = {
  align: 'left',
  allowNegative: false,
  decimal: '.',
  precision: 2,
  prefix: '₱ ',
  suffix: '',
  thousands: ','
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    SharedModule,
    CustomerModule,
    InventoryModule,
    OrderModule,
    InvoiceModule,
    DeliveryModule,
    CreditMemoModule,
    VoucherModule,
    QuotationModule,
    SettingsModule,
    ReportsModule,
    AppHeaderModule,
    AppSidebarModule,
    AppFooterModule,
    AppBreadcrumbModule.forRoot(),
    PerfectScrollbarModule,
    NgxDatatableModule,
    ToastrModule.forRoot(),
    NgbTypeaheadModule,
    CurrencyMaskModule
  ],
  providers: [
    { provide: PERFECT_SCROLLBAR_CONFIG, useValue: CustomScrollConfig },
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
