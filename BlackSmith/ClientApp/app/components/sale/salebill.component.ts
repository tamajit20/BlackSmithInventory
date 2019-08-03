import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Sale, SaleLoader, SaleDetail } from '../../model/sale';
import { SaleService } from '../../services/saleservice';
import { Product } from '../../model/product';
import { saveAs } from 'file-saver';

@Component({
    selector: 'salebill',
    templateUrl: './salebill.component.html',
    styleUrls: ['../../style/style.css']
})

export class SaleBillComponent extends BaseComponent implements OnInit {
    model: Sale;
    modelList: Sale[] = [];
    loader: SaleLoader = new SaleLoader();
    currentSaleDetailNo: number = 0;
    myDatePickerOptions = {
        editableDateField: false,
        dateFormat: 'dd/mm/yyyy'
    };
    today = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    billDate: any = this.today;
    paymentTerms = ['Cash', 'Check'];

    constructor(
        private _service: SaleService
    ) {
        super();
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
        this.model = <Sale>({ isFailure: false });
        this.billDate = this.today;
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
        const newSaleDetail = <SaleDetail>({ saleDetailNo: this.currentSaleDetailNo });
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

        if (obj.availableQuantity >= obj.quantity) {
            if (obj) {
                obj.total = obj.price * obj.quantity;
            }
        }
        else {
            obj.quantity = 0;
            alert("Available : " + obj.availableQuantity);
        }
        return '0';
    }

    download() {
        this._service.downloadBill(this.model);
    }

    validate() {
        this.model.msg = "Invalid input";
        this.model.isFailure = true;
        if (this.model) {
            if (!this.billDate) {
                this.model.msg = "Invalid Bill Date";
                return false;
            }
            if (!this.model.fK_CustomerId) {
                this.model.msg = "Invalid Customer";
                return false;
            }
            if (!this.model.saleDetails || this.model.saleDetails.length === 0) {
                this.model.msg = "No Sale Item";
                return false;
            }
            this.model.msg = "";
            this.model.isFailure = false;
            return true;
        }
        return false;
    }

    calculateAvailibility(saleDetailNo: number, productId: number) {
        var obj = this.loader.products.find(x => x.id == productId);
        var saleDetail = this.model.saleDetails.find(x => x.saleDetailNo === saleDetailNo);

        if (obj && saleDetail) {
            saleDetail.availableQuantity = obj.availibility;
        }
    }

    save() {
        if (this.validate()) {
            this.model.isGenerated = false;
            this.model.billDate = this.billDate.date.month + "/" + this.billDate.date.day + "/" + this.billDate.date.year;
            this._service.save(this.model).subscribe(data => {
                this.model = data;
                if (!this.model.isFailure) {
                    this.model.isGenerated = true;
                    this.model.msg = "Bill Generated";
                }
            });
        }
    }
}