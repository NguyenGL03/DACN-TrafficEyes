import { Input, Component, ViewEncapsulation, Injector, AfterViewInit } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { AuthStatusConsts } from "../../utils/consts/AuthStatusConsts";

@Component({
    templateUrl: './auth-status-input-page-inventory.component.html',
    selector: 'auth-status-input-page-inventory',
    styleUrls: ["./auth-status-input-page-inventory.css"],
    encapsulation: ViewEncapsulation.None
})
export class AuthStatusInputPageInventoryComponent extends AppComponentBase implements AfterViewInit {
    ngAfterViewInit(): void {
        // COMMENT: this.stopAutoUpdateView();
    }

    _authStatus: string;
    _processId: string;
    @Input() approve: string = this.l('Approved');
    @Input() reject: string = this.l('Reject');

    @Input() get authStatus(): string {
        return this._authStatus;
    }
    @Input() get processId(): string {
        return this._processId;
    }
    set processId(process: string) {
        this._processId = process;

    }
    set authStatus(auth: string) {
        this._authStatus = auth;

    }

    AuthStatusConsts = AuthStatusConsts;

    constructor(injector: Injector) {
        super(injector);
    }
}
