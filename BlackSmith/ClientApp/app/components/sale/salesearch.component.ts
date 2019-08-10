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

@Component({
    selector: 'salesearch',
    templateUrl: './salesearch.component.html',
    styleUrls: ['../../style/style.css']
})

export class SaleSearchComponent extends BaseComponent implements OnInit {
    model: SearchObject;
    payment: SalePayment = new SalePayment();
    searchResult: SaleList = new SaleList();
    loader: SaleLoader = new SaleLoader();
    saleDetails: Sale = new Sale();
    salePayments: Sale = new Sale();

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
        private _service: SaleService
    ) {
        super();
        let d: Date = new Date();
    }

    ngOnInit(): void {
        this.addNew();
        this.saleDetails.customer = new Customer();
        this.salePayments.customer = new Customer();
        this.payment.sale = new Sale();
        this.payment.sale.customer = new Customer();

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

        this._service.getSaleListOnScreen(this.model).subscribe(data => {
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
        this._service.getSaleListOnExcel(this.model);
    }

    showSaleDetail(sale: any) {
        this.saleDetails = sale;
        this.saleDetails.customer = sale.customer;
    }

    showPaymentDetail(sale: any) {
        this.salePayments = sale;
    }

    downloadBill(id: any) {
        this._service.downloadBill(id);
    }

    downloadMultipleBill() {
        var ids = '';
        if (this.searchResult && this.searchResult.sales) {
            this.searchResult.sales.forEach(e => {
                ids = ids + ',' + e.id;
            });
        }
        
        this._service.downloadBill(ids);
    }

    showPayment(sale: any) {
        this.payment = <SalePayment>({});
        this.payment.sale = sale;
        console.log(this.payment);
        this.payment.fK_SaleId = sale.id;
        this.payment.id = 0;
        this.paymentDate = this.today;
    }

    pay() {
        if (this.validate()) {
            this.payment.paymentDate = this.paymentDate.date.month + "/" + this.paymentDate.date.day + "/" + this.paymentDate.date.year;

            this._service.payment(this.payment).subscribe(data => {
                this.payment = data;
                this.payment.amount = 0;
                if (!this.payment.isFailure) {
                    this.payment.msg = "Payment Successful";
                    this.search();
                }
            });
        }
    }

    validate() {
        this.payment.msg = "Invalid Input";
        this.payment.isFailure = true;

        if (this.payment) {
            if (!this.payment.fK_SaleId || this.payment.sale.id <= 0) {
                this.payment.msg = "Sale not found";
                return false;
            }
            if (!this.paymentDate) {
                this.payment.msg = "Invalid Date";
                return false;
            }
            if (this.payment.amount == 0) {
                this.payment.msg = "Amount can't be zero";
                return false;
            }
            if (this.payment.sale.due < this.payment.amount) {
                this.payment.msg = "Over payment";
                return false;
            }
            this.payment.msg = "";
            this.payment.isFailure = false;
            return true;
        }
        return false;
    }
}