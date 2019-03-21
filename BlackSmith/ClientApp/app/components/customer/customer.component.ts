import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Customer } from '../../model/customer';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { CustomerSerivce } from '../../services/customerservice';
import { BaseComponent } from '../base.component';
import { FormGroup } from '@angular/forms/src/model';

@Component({
    selector: 'customer',
    templateUrl: './customer.component.html',
    styleUrls: ['../../style/style.css']})

export class CustomerComponent extends BaseComponent implements OnInit {

    model: Customer;
    modelList: Customer[];

    constructor(
        private _service: CustomerSerivce
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

    getAll() {
        this._service.getAllCustomer().subscribe(data => {
            console.log(data);
            this.modelList = data;
        });
    }

    addNew() {
        this.model = <Customer>({});
    }

    edit(input: any) {
        this.model = input;
    }

    delete(input: any) {
        this._service.delete(input).subscribe();
    }
}
