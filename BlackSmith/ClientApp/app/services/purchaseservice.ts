import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule, RequestOptions, ResponseContentType } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'
import { SharedService } from "./sharedservice";

@NgModule({
    imports: [HttpModule]
})


@Injectable()
export class PurchaseService extends SharedService {

    constructor(public http: Http) {
        super(http);
    }

    save(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PURCHASE_SAVE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    delete(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PURCHASE_DELETE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getPurchaseListOnScreen(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PURCHASE_LIST_ONSCREEN, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getPurchaseListOnExcel(input: any) {
        this.http.get(AppConfig.API_ENDPOINT + AppConfig.PURCHASE_LIST_ONEXCEL, new RequestOptions({ params: { searchObject: JSON.stringify(input) }, responseType: ResponseContentType.Blob })).subscribe(res => {
            saveAs(res.blob(), "report.xlsx");
        });
    }

    payment(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PURCHASE_PAYMENT, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }


    downloadBill(input: any) {
        this.http.get(AppConfig.API_ENDPOINT + AppConfig.PURCHASE_DOWNLOAD, new RequestOptions({ params: { id: input.id }, responseType: ResponseContentType.Blob })).subscribe(res => {
            saveAs(res.blob(), "C:/bill.pdf");
            this.print(res.blob());

        });
    }
}