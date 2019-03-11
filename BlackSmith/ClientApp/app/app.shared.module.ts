import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
//import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { CustomerComponent } from './components/customer/customer.component';
import { PurchaseComponent } from './components/purchase/purchase.component';
import { ReportComponent } from './components/report/report.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SuplierComponent } from './components/suplier/suplier.component';
import { ProductComponent } from './components/product/product.component';
import { InventoryItemComponent } from './components/InventoryItem/inventoryitem.component';
import { CustomerSerivce } from './services/customerservice';
import { SuplierService } from './services/suplierservice';
import { InventoryItemService } from './services/inventoryitemservice';
import { ProductSerivce } from './services/productservice';
import { LoginComponent } from './components/login/login.component';
import { UserService } from './services/userservice';
import { PurchaseService } from './services/purchaseservice';
import { SideMenuComponent } from './components/sidemenu/sidemenu.component';
import { SaleService } from './services/saleservice';
import { MyDatePickerModule } from 'mydatepicker';
import { SaleConfigComponent } from './components/sale/saleconfig.component';
import { SalePaymentComponent } from './components/sale/salepayment.component';
import { SaleBillComponent } from './components/sale/salebill.component';
import { BrowserModule } from '@angular/platform-browser';
import { SaleSearchComponent } from './components/sale/salesearch.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        SideMenuComponent,
        LoginComponent,
        HomeComponent,
        CustomerComponent,
        SaleBillComponent,
        SaleConfigComponent,
        SalePaymentComponent,
        SaleSearchComponent,
        PurchaseComponent,
        ReportComponent,
        DashboardComponent,
        ProductComponent,
        SuplierComponent,
        InventoryItemComponent
    ],
    providers: [
        CustomerSerivce,
        SuplierService,
        InventoryItemService,
        ProductSerivce,
        UserService,
        PurchaseService,
        SaleService
    ],
    imports: [
       // CommonModule,
        BrowserModule,
        HttpClientModule,
        HttpModule,
        FormsModule,
        MyDatePickerModule,
        RouterModule.forRoot([
            { path: '', component: LoginComponent },
            {
                path: 'home', component: HomeComponent,
                children: [
                    { path: 'customer', component: CustomerComponent, outlet: 'homeoutlet' },
                    { path: 'saleconfig', component: SaleConfigComponent, outlet: 'homeoutlet' },
                    { path: 'salesearch', component: SaleSearchComponent, outlet: 'homeoutlet' },
                    { path: 'purchase', component: PurchaseComponent, outlet: 'homeoutlet' },
                    { path: 'report', component: ReportComponent, outlet: 'homeoutlet' },
                    { path: 'dashboard', component: DashboardComponent, outlet: 'homeoutlet' },
                    { path: 'product', component: ProductComponent, outlet: 'homeoutlet' },
                    { path: 'suplier', component: SuplierComponent, outlet: 'homeoutlet' },
                    { path: 'inventoryitem', component: InventoryItemComponent, outlet: 'homeoutlet' }
                ]
            },
            { path: '**', redirectTo: '' }
        ])
    ]
})
export class AppModuleShared {
}
