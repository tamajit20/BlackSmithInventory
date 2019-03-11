import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Sale, SaleLoader, SaleDetail, SalePayment } from '../../model/sale';
import { SaleService } from '../../services/saleservice';
import { Product } from '../../model/product';

@Component({
    selector: 'salepayment',
    templateUrl: './salepayment.component.html',
    styleUrls: ['../../style/style.css']
})

export class SalePaymentComponent extends BaseComponent implements OnInit {
    model: SalePayment = new SalePayment();
    myDatePickerOptions = {
        editableDateField: false,
        dateFormat: 'dd/mm/yyyy'
    };
    today = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    paymentDate: any = this.today;

    ngOnInit() {
    }

    constructor(
        private _service: SaleService
    ) {
        super();
        let d: Date = new Date();
    }

    addNew() {
        this.model = <SalePayment>({});
    }

    getOne() {
        this._service.getOne(this.model).subscribe(data => {
            this.model = data;
        });
    }

    pay() {
        if (this.validate()) {
            this.model.paymentDate = this.paymentDate.date.month + "/" + this.paymentDate.date.day + "/" + this.paymentDate.date.year;

            this.model.fK_SaleId = this.model.sale.id;
            this.model.id = 0;
            this._service.payment(this.model).subscribe(data => {
                this.model = data;
                this.model.amount = 0;
                if (!this.model.isFailure) {
                    this.model.msg = "Payment Successful";
                }
            });
        }
    }

    validate() {
        this.model.msg = "Invalid Input";
        this.model.isFailure = true;

        if (this.model) {
            if (!this.model.sale || this.model.sale.id <= 0) {
                this.model.msg = "Sale not found";
                return false;
            }
            if (!this.paymentDate) {
                this.model.msg = "Invalid Date";
                return false;
            }
            if (this.model.sale.due < this.model.amount) {
                this.model.msg = "Over payment";
                return false;
            }
            this.model.msg = "";
            this.model.isFailure = false;
            return true;
        }
        return false;
    }
}

