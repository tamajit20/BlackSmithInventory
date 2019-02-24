import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Product } from '../../model/product';
import { ProductSerivce } from '../../services/productservice';
import { BaseComponent } from '../base.component';

@Component({
    selector: 'product',
    templateUrl: './product.component.html',
    styleUrls: ['../../style/style.css']
})
export class ProductComponent extends BaseComponent implements OnInit {

    model: Product;
    modelList: Product[];

    constructor(
        private _service: ProductSerivce
    ) {
        super()
    }

    ngOnInit(): void {
        this.addNew();
        this.getAll();
    }

    save() {
        this._service.save(this.model).subscribe(data => {
            this.model = data;
            this.getAll();
        });
    }

    addNew() {
        this.model = <Product>({});
    }

    getAll() {
        this._service.getAllProduct().subscribe(data => {
            this.modelList = data;
        });
    }

    edit(input: any) {
        this.model = input;
    }

    delete(input: any) {
        this._service.delete(input).subscribe();
    }
}
