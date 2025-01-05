import { Component, Input, ViewEncapsulation } from "@angular/core";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";

@Component({
    selector: "auth-badge",
    templateUrl: './auth-badge.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AuthBadgeComponent<T> {
    @Input() item: T;
    @Input() fieldValue: string = 'autH_STATUS';
    @Input() fieldName: string = 'autH_STATUS_NAME';
    authStatus = AuthStatusConsts;

}
