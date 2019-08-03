import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Sale, SaleLoader, SaleDetail, SaleList, SalePayment } from '../../model/sale';
import { SaleService } from '../../services/saleservice';
import { Product } from '../../model/product';
import { saveAs } from 'file-saver';
import { SearchObject } from '../../model/searchobject';
import { Customer } from '../../model/customer';
import { ProductionService } from '../../services/productionservice';
import { ProductionList,ProductionLoader, ProductionProduct, ProductionInventoryItem } from '../../model/production';

@Component({
    selector: 'productionsearch',
    templateUrl: './productionsearch.component.html',
    styleUrls: ['../../style/style.css']
})

export class ProductionSearchComponent extends BaseComponent implements OnInit {
    model: SearchObject;
    searchResult: ProductionList = new ProductionList();
    loader: ProductionLoader = new ProductionLoader();
    productDetails: ProductionProduct[] = [];
    inventoryDetails: ProductionInventoryItem[] = [];
   
    myDatePickerOptions = {
        editableDateField: false,
        dateFormat: 'dd/mm/yyyy'
    };

    threeMonthsBack = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    today = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    fromDate: any = this.threeMonthsBack;
    toDate: any = this.today;
    paymentDate: any = this.today;

    constructor(
        private _service: ProductionService
    ) {
        super();
        let d: Date = new Date();
    }

    ngOnInit(): void {
        this.addNew();
      
        this.loader.products = [];
        this.search();
    }

    addNew() {
        this.model = <SearchObject>({ isFailure: false });
        this.fromDate = this.threeMonthsBack;
        this.toDate = this.today;
    }

    
    search() {
        if (this.fromDate) {
            this.model.fromDate = this.fromDate.date.month + "/" + this.fromDate.date.day + "/" + this.fromDate.date.year;
        }
        else {
            this.model.fromDate = '01/01/1900';
        }
        if (this.toDate) {
            this.model.toDate = this.toDate.date.month + "/" + this.toDate.date.day + "/" + this.toDate.date.year;
        }
        else {
            this.model.toDate = '01/01/2099';
        }

        this._service.getProductionListOnScreen(this.model).subscribe(data => {
            this.searchResult = data;
        });
    }

    download() {
        if (this.fromDate) {
            this.model.fromDate = this.fromDate.date.month + "/" + this.fromDate.date.day + "/" + this.fromDate.date.year;
        }
        else {
            this.model.fromDate = '01/01/1900';
        }
        if (this.toDate) {
            this.model.toDate = this.toDate.date.month + "/" + this.toDate.date.day + "/" + this.toDate.date.year;
        }
        else {
            this.model.toDate = '01/01/2099';
        }
        this._service.getProductionListOnExcel(this.model);
    }

    showProductDetail(prod: any) {
        console.log(prod);
        this.productDetails = prod;
    }

    showInventoryDetail(inv: any) {
        this.inventoryDetails = inv;
    }
}