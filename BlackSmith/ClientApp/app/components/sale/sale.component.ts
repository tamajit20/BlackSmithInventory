import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Sale, SaleLoader } from '../../model/sale';
import { SaleService } from '../../services/saleservice';

@Component({
    selector: 'sale',
    templateUrl: './sale.component.html',
    styleUrls: ['../app/app.component.css']
})
export class SaleComponent extends BaseComponent implements OnInit {
    model: Sale ;
    modelList: Sale[] =[];
    loader: SaleLoader = new SaleLoader();

    constructor(
        private _service: SaleService
    ) {
        super()
    }

    ngOnInit(): void {
        this.addNew();
        this.getAllCustomer();
        this.getAllProduct();
    }

    addNew() {
        this.model = <Sale>({});
    }

    getAllProduct() {
        this._service.getAllProduct().subscribe(data => {
            this.loader.product = data;
        });
    }

    getAllCustomer() {
        this._service.getAllCustomer().subscribe(data => {
            this.loader.customer = data;
        });
    }
}