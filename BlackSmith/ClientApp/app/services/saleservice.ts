import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule, ResponseContentType, RequestOptions } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'
import { SharedService } from "./sharedservice";
import { Observable } from "rxjs/Observable";
import { HttpHeaders } from "@angular/common/http";
import { saveAs } from 'file-saver';

@NgModule({
    imports: [HttpModule]
})

@Injectable()
export class SaleService extends SharedService {

    constructor(public http: Http, public httpClient: HttpModule) {
        super(http);
    }

    save(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_SAVE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getSaleListOnScreen(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_LIST_ONSCREEN, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getSaleListOnExcel(input: any) {
        this.http.get(AppConfig.API_ENDPOINT + AppConfig.SALE_LIST_ONEXCEL, new RequestOptions({ params: { searchObject: JSON.stringify(input) }, responseType: ResponseContentType.Blob })).subscribe(res => {
            saveAs(res.blob(), "report.xlsx");
        });
    }

    getOne(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_GETONE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    payment(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_PAYMENT, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }


    downloadBill(input: any) {
        this.http.get(AppConfig.API_ENDPOINT + AppConfig.SALE_DOWNLOAD, new RequestOptions({ params: { id: input.id }, responseType: ResponseContentType.Blob })).subscribe(res => {
            //  saveAs(res.blob(), "bill.pdf");
            saveAs(res.blob(), "C:/bill.pdf");
            this.print(res.blob());

        });
    }
  

    delete(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.SALE_DELETE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }
}