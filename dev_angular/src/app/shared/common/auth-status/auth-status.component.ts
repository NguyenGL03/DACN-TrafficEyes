import { Component, Input } from '@angular/core';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';

@Component({
    selector: 'auth-status',
    templateUrl: './auth-status.component.html',
})
export class AuthStatusComponent {
    @Input() show: boolean = false;
    @Input() authStatus: AuthStatusConsts;
    AuthStatusConsts = AuthStatusConsts;

}
