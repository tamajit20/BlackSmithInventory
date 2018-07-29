import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { InventoryItem } from '../../model/inventoryitem';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { InventoryItemService } from '../../services/inventoryitemservice';
import { BaseComponent } from '../base.component';

@Component({
    selector: 'inventoryitem',
    templateUrl: './inventoryitem.component.html'
})
export class InventoryItemComponent extends BaseComponent implements OnInit {

    model: InventoryItem;
    modelList: InventoryItem[];

    constructor(
        private _service: InventoryItemService
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
        this.model = <InventoryItem>({})
    }

    getAll() {
        this._service.getAllInventoryItem().subscribe(data => {
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
