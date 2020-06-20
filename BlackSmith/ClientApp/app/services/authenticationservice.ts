import 'rxjs/add/operator/map'
import { CanActivate, CanActivateChild } from "@angular/router";
import { Injectable, NgModule } from "@angular/core";
import { Http, HttpModule } from "@angular/http";
import { AppConfig } from "../app.config";
import 'rxjs/add/operator/map'

@NgModule({
    imports: [HttpModule]
})

@Injectable()
export class AuthenticationService implements CanActivate {

    constructor() {

    }

    canActivate() {
        var isActive = localStorage.getItem('ISON');
        if (isActive && isActive.toUpperCase() == "TRUE")
            return true;
        return false;
    }
}