import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'
import { SharedService } from "./sharedservice";

@NgModule({
    imports: [HttpModule]
})

@Injectable()
export class SaleService extends SharedService {

    constructor(public http: Http) {
        super(http);
    }

    save(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_SAVE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    download(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_DOWNLOAD, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    delete(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_DELETE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }
}