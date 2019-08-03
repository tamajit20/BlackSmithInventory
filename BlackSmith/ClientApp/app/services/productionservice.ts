import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule, ResponseContentType, RequestOptions } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'
import { SharedService } from "./sharedservice";

@NgModule({
    imports: [HttpModule]
})


@Injectable()
export class ProductionService extends SharedService {

    constructor(public http: Http) {
        super(http);
    }

    save(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PRODUCTION_SAVE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    delete(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PRODUCTION_DELETE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getProductionListOnScreen(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.PRODUCTION_LIST_ONSCREEN, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }

    getProductionListOnExcel(input: any) {
        this.http.get(AppConfig.API_ENDPOINT + AppConfig.PRODUCTION_LIST_ONEXCEL, new RequestOptions({ params: { searchObject: JSON.stringify(input) }, responseType: ResponseContentType.Blob })).subscribe(res => {
            saveAs(res.blob(), "report.xlsx");
        });
    }

}