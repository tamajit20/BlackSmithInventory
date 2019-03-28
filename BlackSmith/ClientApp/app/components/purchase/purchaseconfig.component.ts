import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { BaseComponent } from '../base.component';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Purchase, PurchaseLoader } from '../../model/purchase';
import { PurchaseService } from '../../services/purchaseservice';
import { Sale, SaleLoader, SaleDetail } from '../../model/sale';
import { SaleService } from '../../services/saleservice';
import { Product } from '../../model/product';

@Component({
    selector: 'purchaseconfig',
    templateUrl: './purchaseconfig.component.html',
    styleUrls: ['../../style/style.css']
})

export class PurchaseConfigComponent extends BaseComponent implements OnInit {
    ngOnInit() { }
}

