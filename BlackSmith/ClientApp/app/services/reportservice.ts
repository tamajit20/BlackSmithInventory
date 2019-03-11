import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'
import { SharedService } from "./sharedservice";

@NgModule({
    imports: [HttpModule]
})


@Injectable()
export class ReportSerivce extends SharedService {

    constructor(public http: Http) {
        super(http);
    }
}