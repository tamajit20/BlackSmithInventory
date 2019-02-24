import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Suplier } from '../../model/suplier';
import { SuplierService } from '../../services/suplierservice';
import { SharedService } from '../../services/sharedservice';

@Component({
    selector: 'suplier',
    templateUrl: './suplier.component.html',
    styleUrls: ['../../style/style.css']
})
export class SuplierComponent implements OnInit {

    model: Suplier;
    modelList: Suplier[];
    constructor(
        private _service: SuplierService
    ) {
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
        this.model = <Suplier>({});
    }

    edit(input: any) {
        this.model = input;
    }

    delete(input: any) {
        this._service.delete(input).subscribe();
    }
    getAll() {
        this._service.getAllSuplier().subscribe(data => {
            this.modelList = data;
        });
    }
}
