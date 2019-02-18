import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Customer } from '../../model/customer';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { CustomerSerivce } from '../../services/customerservice';
import { BaseComponent } from '../base.component';
import { FormGroup } from '@angular/forms/src/model';
import { UserService } from '../../services/userservice';
import { User } from '../../model/user';
import { Router } from '@angular/router';

@Component({
    selector: 'login',
    templateUrl: './login.component.html'
})
export class LoginComponent extends BaseComponent implements OnInit {

   // model: User;

    constructor(
        private _service: UserService,
        public router: Router
    ) {
        super()
    }

    ngOnInit(): void {

    }

    ValidateUser() {
        //this._service.ValidateUser(this.model).subscribe(data => {
        //    this.model = data;
        //});
        this.router.navigate(['/home', { outlets: {homeoutlet:['dashboard']}}]);
    }
}
