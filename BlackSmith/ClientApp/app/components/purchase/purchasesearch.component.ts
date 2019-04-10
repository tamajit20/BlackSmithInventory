import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader, PurchasePayment, PurchaseList } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Product } from '../../model/product';
import { saveAs } from 'file-saver';
import { SearchObject } from '../../model/searchobject';
import { Customer } from '../../model/customer';

@Component({
    selector: 'purchasesearch',
    templateUrl: './purchasesearch.component.html',
    styleUrls: ['../../style/style.css']
})

export class PurchaseSearchComponent extends BaseComponent implements OnInit {
    model: SearchObject;
    payment: PurchasePayment = new PurchasePayment();
    searchResult: PurchaseList = new PurchaseList();
    loader: PurchaseLoader = new PurchaseLoader();
    purchaseDetails: Purchase = new Purchase();
    purchasePayments: Purchase = new Purchase();

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
        private _service: PurchaseService
    ) {
        super();
        let d: Date = new Date();
    }

    ngOnInit(): void {
        this.addNew();
       // this.purchaseDetails.customer = new Customer();
        //this.purchasePayments.customer = new Customer();
        this.payment.purchase = new Purchase();
      //  this.payment.purchase.customer = new Customer();

        this.getAllSupliers();
        this.getAllItems();
        this.loader.items = [];
        this.search();
    }

    addNew() {
        this.model = <SearchObject>({ isFailure: false });
        this.fromDate = this.threeMonthsBack;
        this.toDate = this.today;
    }

    getAllItems() {
        this._service.getAllProduct().subscribe(data => {
            this.loader.items = data;
        });
    }

    getAllSupliers() {
        this._service.getAllSuplier().subscribe(data => {
            this.loader.supliers = data;
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

        this._service.getPurchaseListOnScreen(this.model).subscribe(data => {
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
        this._service.getPurchaseListOnExcel(this.model);
    }

    showPurchaseDetail(purchase: any) {
        this.purchaseDetails = purchase;
      //  this.purchaseDetails.suplier = purchase.suplier;
    }

    showPaymentDetail(purchase: any) {
        this.purchasePayments = purchase;
    }

    downloadBill(id: any) {
        const purchase = <Purchase>({ id: id });
        this._service.downloadBill(purchase);
    }

    showPayment(purchase: any) {
        this.payment = <PurchasePayment>({});
        this.payment.purchase = purchase;
        this.payment.fK_PurchaseId = purchase.id;
        this.payment.id = 0;
        this.paymentDate = this.today;
    }

    pay() {
        console.log(this.payment);
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
            if (!this.payment.fK_PurchaseId || this.payment.purchase.id <= 0) {
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
            if (this.payment.purchase.due < this.payment.amount) {
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