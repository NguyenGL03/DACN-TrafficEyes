import { Component } from "@angular/core";
import { ToolbarComponent } from "../toolbar/toolbar.component";
import { ActionRole } from "@app/utilities/enum/action-role";
import { IUiAction } from "@app/utilities/ui-action";

@Component({
    selector: 'appToolbar_re',
    templateUrl: './toolbar-reject-ext.component.html',
    styleUrls: ['./toolbar.component.css']
})
export class ToolbarRejectExtComponent extends ToolbarComponent {


    buttonRejectEnable: boolean;
    buttonRejectVisible: boolean;

    setRole(funct: string, add: boolean, edit: boolean, update: boolean, del: boolean, view: boolean, search: boolean, approve: boolean, resetSearch: boolean, reject?: boolean) {
        super.setRole(funct, add, edit, update, del, view, search, approve, resetSearch);
        if (reject) {
            this.setButtonRejectVisible(reject && this.permission.isGranted('Pages.Administration.' + funct + '.' + ActionRole.Approve));
        }
    }

    get uiActionRejectExt(): IUiAction<any> {
        return this.uiAction as IUiAction<any>;
    }

    public setEnableForListPage(): void {
        super.setEnableForEditPage();
        this.setButtonRejectEnable(false);
    }

    public setEnableForEditPage(): void {
        super.setEnableForEditPage();
        this.setButtonRejectEnable(false);
    }

    public setEnableForViewDetailPage(): void {
        super.setEnableForViewDetailPage();
        this.setButtonRejectEnable(false);
    }

    setButtonRejectEnable(enable: boolean): void {
        if (!this.buttonResetSearchVisible) {
            enable = false;
        }
        this.buttonRejectEnable = enable;
    }

    setButtonRejectVisible(visible: boolean): void {
        this.buttonRejectVisible = visible;
    }

    reject(): void {
        if (this.uiActionRejectExt) {
            this.uiActionRejectExt.onReject(this.selectedItem);
        }
    }

}
