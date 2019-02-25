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

    constructor(
        private _service: SaleService
    ) {
        super()
    }

    ngOnInit(): void {
        this.addNew();
        this.model.cGSTRate = 9;
        this.model.total = 0;
        this.model.totalTax = 0;
        this.model.finalTotal = 0;

        this.getAllCustomer();
        this.getAllProduct();
        this.loader.products = [];
        const x = <Product>({ id: 4, name: 'gfgf' });
        this.loader.products.push(x);
        console.log(this.loader.products);
    }

    addNew() {
        this.model = <Sale>({});
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
        this.currentSaleDetailNo = this.currentSaleDetailNo + 1;
        const newSaleDetail = <SaleDetail>({ saleDetailNo: this.currentSaleDetailNo, price: 0, quantitiy: 0, fK_ProductId: 1, total: 0 });
        this.model.saleDetails.push(newSaleDetail);
    }

    deleteSaleDetail(delSaleDetailNo: number) {
        this.model.saleDetails.splice(this.model.saleDetails.findIndex(item => item.saleDetailNo === delSaleDetailNo), 1);
        //this.calculateFinalTotal();
    }

    getIndexByDetailNo(saleDetailNo: number) {
        return this.model.saleDetails.findIndex(item => item.saleDetailNo === saleDetailNo);
    }

    calculateTotalPerProduct(saleDetailNo: number) {
       // this.calculateFinalTotal();
        var obj = this.model.saleDetails.find(x => x.saleDetailNo === saleDetailNo);
        if (obj) {
            obj.total = obj.price * obj.quantitiy;
        }
        return '0';
    }

    //calculateFinalTotal() {
    //    this.model.total = 0;
    //    this.model.finalTotal = 0;

    //    this.model.saleDetails.forEach(each => {
    //        this.model.total = this.model.total + (each.quantitiy * each.price);
    //    });
    //    if (this.model.total) {
    //        this.model.totalTax = this.model.total * (this.model.cGSTRate + this.model.sGSTRate) / 100;
    //        this.model.finalTotal = this.model.totalTax + this.model.total;
    //    }
    //}

    save() {
        this._service.save(this.model).subscribe(data => {

        });
    }
}