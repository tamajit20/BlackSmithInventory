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

@Component({
    selector: 'salesearch',
    templateUrl: './salesearch.component.html',
    styleUrls: ['../../style/style.css']
})

export class SaleSearchComponent extends BaseComponent implements OnInit {
    model: SearchObject;
    searchResult: SaleList = new SaleList();
    loader: SaleLoader = new SaleLoader();
    saleDetails: SaleDetail[] = [];
    salePayments: SalePayment[] = [];

    myDatePickerOptions = {
        editableDateField: false,
        dateFormat: 'dd/mm/yyyy'
    };

    threeMonthsBack = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    today = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    fromDate: any = this.threeMonthsBack;
    toDate: any = this.today;

    constructor(
        private _service: SaleService
    ) {
        super();
        let d: Date = new Date();
    }

    ngOnInit(): void {
        this.addNew();

        this.getAllCustomer();
        this.getAllProduct();
        this.loader.products = [];
        this.search();
    }

    addNew() {
        this.model = <SearchObject>({ isFailure: false });
        this.fromDate = this.threeMonthsBack;
        this.toDate = this.today;
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

    search() {
        this.model.fromDate = this.fromDate.date.month + "/" + this.fromDate.date.day + "/" + this.fromDate.date.year;
        this.model.toDate = this.toDate.date.month + "/" + this.toDate.date.day + "/" + this.toDate.date.year;

        this._service.getSaleList(this.model).subscribe(data => {
            this.searchResult = data;
        });
    }

    showSaleDetail(saleDetails: any) {
        this.saleDetails = saleDetails;
    }

    showPaymentDetail(salePayments: any) {
        this.salePayments = salePayments;
    }

    downloadBill(id: any) {
        const sale = <Sale>({id:id});
        this._service.downloadBill(sale);
    }
}