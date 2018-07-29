import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'
import { SharedService } from "./sharedservice";

@NgModule({
    imports: [HttpModule]
})


@Injectable()
export class UserService extends SharedService {

    constructor(public http: Http) {
        super(http);
    }

    ValidateUser(input: any) {
        return this.http.post(AppConfig.API_ENDPOINT + AppConfig.USER_VALIDATE, input, AppConfig.REQUEST_HEADER)
            .map(res => res.json());
    }
}