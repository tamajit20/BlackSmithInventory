import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Sale, SaleLoader, SaleDetail } from '../../model/sale';
import { SaleService } from '../../services/saleservice';
import { Production, ProductionLoader, ProductionProduct, ProductionInventoryItem } from '../../model/production';
import { saveAs } from 'file-saver';
import { ProductionService } from '../../services/productionservice';
import { Product } from '../../model/product';

@Component({
    selector: 'productionentry',
    templateUrl: './productionentry.component.html',
    styleUrls: ['../../style/style.css']
})

export class ProductionEntryComponent extends BaseComponent implements OnInit {
    model: Production;
    modelList: Production[] = [];
    loader: ProductionLoader = new ProductionLoader();
    currentProductNo: number = 0;
    currentItemNo: number = 0;

    myDatePickerOptions = {
        editableDateField: false,
        dateFormat: 'dd/mm/yyyy'
    };
    today = { date: { year: (new Date()).getFullYear(), month: (new Date()).getMonth() + 1, day: (new Date()).getDate() } };
    productionDate: any = this.today;

    constructor(
        private _service: ProductionService
    ) {
        super();
    }

    ngOnInit(): void {
        this.addNew();
        
        this.getAllInventoryItem();
        this.getAllProduct();

        this.loader.products = [];
    }

    addNew() {
        this.model = <Production>({ isFailure: false });
        this.productionDate = this.today;
        this.model.productionItems = [];
        this.model.productionProducts = [];
    }

    getAllProduct() {
        this._service.getAllProduct().subscribe(data => {
            this.loader.products = data;
        });
    }

    getAllInventoryItem() {
        this._service.getAllInventoryItem().subscribe(data => {
            this.loader.items = data;
        });
    }


    addNewProduct() {
        this.model.isGenerated = false;
        this.currentProductNo = this.currentProductNo + 1;
        const newDetail = <ProductionProduct>({ detailNo : this.currentProductNo });
        this.model.productionProducts.push(newDetail);
    }

    deleteProduct(detailNo: number) {
        this.model.productionProducts.splice(this.model.productionProducts.findIndex(item => item.detailNo === detailNo), 1);
    }

    getProductIndexByDetailNo(detailNo: number) {
        return this.model.productionProducts.findIndex(item => item.detailNo === detailNo);
    }



    addNewItem() {
        this.model.isGenerated = false;
        this.currentItemNo = this.currentItemNo + 1;
        const newDetail = <ProductionInventoryItem>({detailNo : this.currentItemNo});
        this.model.productionItems.push(newDetail);
    }

    deleteItem(detailNo: number) {
        this.model.productionItems.splice(this.model.productionItems.findIndex(item => item.detailNo === detailNo), 1);
    }

    getItemIndexByDetailNo(detailNo: number) {
        return this.model.productionItems.findIndex(item => item.detailNo === detailNo);
    }


    validate() {
        this.model.msg = "Invalid input";
        this.model.isFailure = true;
        if (this.model) {
            if (!this.productionDate) {
                this.model.msg = "Invalid Production Date";
                return false;
            }
           
            if (!this.model.productionItems || this.model.productionItems.length === 0) {
                this.model.msg = "No Inventory Item";
                return false;
            }

            if (!this.model.productionProducts || this.model.productionProducts.length === 0) {
                this.model.msg = "No Product";
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
            this.model.date = this.productionDate.date.month + "/" + this.productionDate.date.day + "/" + this.productionDate.date.year;
            console.log(this.model);
            //this._service.save(this.model).subscribe(data => {
            //    this.model = data;
            //    if (!this.model.isFailure) {
            //        this.model.isGenerated = true;
            //        this.model.msg = "Saved";
            //    }
            //});
        }
    }
}