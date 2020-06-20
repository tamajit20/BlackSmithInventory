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

    model: User;
    message: String;

    constructor(
        private _service: UserService,
        public router: Router
    ) {
        super()
    }

    ngOnInit(): void {
        this.model = <User>({});
        localStorage.setItem('ISON', 'false');
    }

    ValidateUser() {
        this._service.ValidateUser(this.model).subscribe(data => {
            if (data) {
                localStorage.setItem('ISON', 'true');
                this.router.navigate(['/home', { outlets: { homeoutlet: ['dashboard'] } }]);
            }
            else {
                this.message = "Invalid Password";
            }
        });
    }
}
