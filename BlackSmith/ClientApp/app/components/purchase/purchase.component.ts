import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';

@Component({
    selector: 'purchase',
    templateUrl: './purchase.component.html'
})
export class PurchaseComponent extends BaseComponent implements OnInit {
    model: Purchase;
    modelList: Purchase[];
    loader: PurchaseLoader = new PurchaseLoader();

    constructor(
        private _service: PurchaseService
    ) {
        super()
    }

    ngOnInit(): void {
        this.addNew();
        this.getAllSuplier();
        this.getAllInventoryItem();
        console.log(this.loader);
    }

    addNew() {
        this.model = <Purchase>({});
    }

    getAllInventoryItem() {
        this._service.getAllInventoryItem().subscribe(data => {
            this.loader.inventoryItem = data;
        });
    }

    getAllSuplier() {
        this._service.getAllSuplier().subscribe(data => {
            this.loader.suplier = data;
        });
    }
}
