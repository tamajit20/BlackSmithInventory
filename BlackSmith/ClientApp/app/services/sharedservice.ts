import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'

@NgModule({
    imports: [HttpModule]
})


@Injectable()
export class SharedService {

    constructor(public http: Http) {

    }

    getAllCustomer() {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.CUSTOMER_GETALL, null, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getAllProduct() {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PRODUCT_GETALL, null, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getAllSuplier() {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SUPLIER_GETALL, null, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getAllInventoryItem() {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.INVENTORYITEM_GETALL, null, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }
}