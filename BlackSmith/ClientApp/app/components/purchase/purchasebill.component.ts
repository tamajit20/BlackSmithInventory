import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { PurchaseService } from '../../services/purchaseservice';
import { Purchase, PurchaseLoader, PurchaseDetail } from '../../model/purchase';
import { Product } from '../../model/product';
import { saveAs } from 'file-saver';

@Component({
    selector: 'purchasebill',
    templateUrl: './purchasebill.component.html',
    styleUrls: ['../../style/style.css']
})

export class PurchaseBillComponent extends BaseComponent implements OnInit {
    model: Purchase;
    modelList: Purchase[] = [];
    loader: PurchaseLoader = new PurchaseLoader();
    currentPurchaseDetailNo: number = 0;
    myDatePickerOptions = {
        editableDateField: false,
        dateFormat: 'dd/mm/yyyy'
    };
    today = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    billDate: any = this.today;
    paymentTerms = ['Cash','Check'];

    constructor(
        private _service: PurchaseService
    ) {
        super();
        let d: Date = new Date();
    }

    ngOnInit(): void {
        this.addNew();
        this.model.cgstRate = 9;
        this.model.sgstRate = 9;

        this.getAllSuplier();
        this.getAllItems();
        this.loader.items = [];
    }

    addNew() {
        this.model = <Purchase>({ isFailure: false });
        this.billDate = this.today;
        this.model.purchaseDetails = [];
    }

    getAllItems() {
        this._service.getAllInventoryItem().subscribe(data => {
            this.loader.items = data;
        });
    }

    getAllSuplier() {
        this._service.getAllSuplier().subscribe(data => {
            this.loader.supliers = data;
        });
    }

    addNewPurchaseDetail() {
        this.model.isGenerated = false;
        this.currentPurchaseDetailNo = this.currentPurchaseDetailNo + 1;
        const newPurchaseDetail = <PurchaseDetail>({ FK_PurchaseId: 0, purchaseDetailNo: this.currentPurchaseDetailNo, price: 0, quantity: 0, fK_ItemId: 1, total: 0 });
        this.model.purchaseDetails.push(newPurchaseDetail);
    }

    deletePurchaseDetail(delPurchaseDetailNo: number) {
        this.model.purchaseDetails.splice(this.model.purchaseDetails.findIndex(item => item.purchaseDetailNo === delPurchaseDetailNo), 1);
    }

    getIndexByDetailNo(purchaseDetailNo: number) {
        return this.model.purchaseDetails.findIndex(item => item.purchaseDetailNo === purchaseDetailNo);
    }

    calculateTotalPerProduct(purchaseDetailNo: number) {
        this.model.isGenerated = false;
        var obj = this.model.purchaseDetails.find(x => x.purchaseDetailNo === purchaseDetailNo);
        if (obj) {
            obj.total = obj.price * obj.quantity;
        }
        return '0';
    }

    validate() {
        this.model.msg = "Invalid input";
        this.model.isFailure = true;
        if (this.model) {
            if (!this.billDate) {
                this.model.msg = "Invalid Bill Date";
                return false;
            }
           
            if (!this.model.purchaseDetails || this.model.purchaseDetails.length === 0) {
                this.model.msg = "No Item";
                return false;
            }

            this.model.msg = "";
            this.model.isFailure = false;
            return true;
        }
        return false;
    }

    save() {
        if (this.validate()) {
            this.model.isGenerated = false;
            this.model.billDate = this.billDate.date.month + "/" + this.billDate.date.day + "/" + this.billDate.date.year;
            console.log(this.model);
            this._service.save(this.model).subscribe(data => {
                this.model = data;
                if (!this.model.isFailure) {
                    this.model.isGenerated = true;
                    this.model.msg = "Purchase added";
                }
            });
        }
    }
}