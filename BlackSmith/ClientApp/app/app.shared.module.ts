import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { CustomerComponent } from './components/customer/customer.component';
import { PurchaseComponent } from './components/purchase/purchase.component';
import { ReportComponent } from './components/report/report.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SuplierComponent } from './components/suplier/suplier.component';
import { ProductComponent } from './components/product/product.component';
import { SaleComponent } from './components/sale/sale.component';
import { InventoryItemComponent } from './components/InventoryItem/inventoryitem.component';
import { CustomerSerivce } from './services/customerservice';
import { SuplierService } from './services/suplierservice';
import { InventoryItemService } from './services/inventoryitemservice';
import { ProductSerivce } from './services/productservice';
import { LoginComponent } from './components/login/login.component';
import { UserService } from './services/userservice';
import { PurchaseService } from './services/purchaseservice';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        LoginComponent,
        HomeComponent,
        CustomerComponent,
        SaleComponent,
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
        PurchaseService
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'login', pathMatch: 'full' },
            { path: 'login', component: LoginComponent },
            { path: 'customer', component: CustomerComponent },
            { path: 'sale', component: SaleComponent },
            { path: 'purchase', component: PurchaseComponent },
            { path: 'report', component: ReportComponent },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'product', component: ProductComponent },
            { path: 'suplier', component: SuplierComponent },
            { path: 'inventoryitem', component: InventoryItemComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
