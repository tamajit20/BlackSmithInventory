import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Sale, SaleLoader, SaleDetail } from '../../model/sale';
import { SaleService } from '../../services/saleservice';
import { Product } from '../../model/product';

@Component({
    selector: 'sale',
    templateUrl: './sale.component.html',
    styleUrls: ['../../style/style.css']
})

export class SaleComponent extends BaseComponent implements OnInit {
    model: Sale;
    modelList: Sale[] = [];
    loader: SaleLoader = new SaleLoader();
    currentSaleDetailNo: number = 0;
    billDate: any;
    constructor(
        private _service: SaleService
    ) {
        super()
    }

    ngOnInit(): void {
        this.addNew();
        this.model.cgstRate = 9;
        this.model.sgstRate = 9;

        this.getAllCustomer();
        this.getAllProduct();
        this.loader.products = [];
    }

    addNew() {
        this.model = <Sale>({isFailure:false});
        this.billDate = this.model.billDate;
        this.model.saleDetails = [];
    }

    getAllProduct() {
        this._service.getAllProduct().subscribe(data => {
            this.loader.products = data;
        });
    }

    getAllCustomer() {
        this._service.getAllCustomer().subscribe(data => {
            this.loader.customers = data;
        });
    }

    addNewSaleDetail() {
        this.model.isGenerated = false;
        this.currentSaleDetailNo = this.currentSaleDetailNo + 1;
        const newSaleDetail = <SaleDetail>({ FK_SaleId: 0, saleDetailNo: this.currentSaleDetailNo, price: 0, quantity: 0, fK_ProductId: 1, total: 0 });
        this.model.saleDetails.push(newSaleDetail);
    }

    deleteSaleDetail(delSaleDetailNo: number) {
        this.model.saleDetails.splice(this.model.saleDetails.findIndex(item => item.saleDetailNo === delSaleDetailNo), 1);
    }

    getIndexByDetailNo(saleDetailNo: number) {
        return this.model.saleDetails.findIndex(item => item.saleDetailNo === saleDetailNo);
    }

    calculateTotalPerProduct(saleDetailNo: number) {
        this.model.isGenerated = false;
        var obj = this.model.saleDetails.find(x => x.saleDetailNo === saleDetailNo);
        if (obj) {
            obj.total = obj.price * obj.quantity;
        }
        return '0';
    }

    print() {
        this._service.download(this.model).subscribe(data => {
        });
    }

    save() {
        this.model.isGenerated = false;
        this.model.billDate = this.billDate.formatted;
        this._service.save(this.model).subscribe(data => {
            this.model = data;
            if (!this.model.isFailure) {
                this.model.isGenerated = true;
                this.model.msg = "Bill Generated";
            }
        });
    }
}