import { ChangeDetectorRef, Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
// import { AppNavigationService } from '@app/shared/layout/nav/app-navigation.service';
import { ActionRole } from '@app/utilities/enum/action-role';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'appToolbar',
    templateUrl: './toolbar.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ToolbarComponent extends AppComponentBase implements OnInit {
    prefix: 'Pages.Administration' | 'Pages.Main' = 'Pages.Administration'; 

    enable: boolean;

    buttonAddEnable: boolean;
    buttonUpdateEnable: boolean;
    buttonSaveEnable: boolean;
    buttonViewDetailEnable: boolean;
    buttonDeleteEnable: boolean;
    buttonApproveEnable: boolean;
    buttonSearchEnable: boolean;
    buttonResetSearchEnable: boolean;
    buttonRejectEnable: boolean;
    buttonSendAppEnable: boolean;

    buttonAddVisible: boolean;
    buttonUpdateVisible: boolean;
    buttonSaveVisible: boolean;
    buttonViewDetailVisible: boolean;
    buttonDeleteVisible: boolean;
    buttonApproveVisible: boolean;
    buttonSearchVisible: boolean;
    buttonResetSearchVisible: boolean;
    buttonRejectVisible: boolean;
    buttonSendAppVisible: boolean;

    buttonAddHidden: boolean;
    buttonUpdateHidden: boolean;
    buttonSaveHidden: boolean;
    buttonViewDetailHidden: boolean;
    buttonDeleteHidden: boolean;
    buttonApproveHidden: boolean;
    buttonSearchHidden: boolean;
    buttonResetSearchHidden: boolean;
    buttonRejectHidden: boolean;
    buttonSendAppHidden: boolean;

    selectedItem: any;

    uiAction: IUiAction<any>;

    isList: boolean = false;
    isEdit: boolean = false;

    isAssUpdateReport = false;

    funct: string;

    editLabel: string;

    // imgPath = `../../../../../assets/icons/${''}/`;


    constructor(
        injector: Injector,
        private cdr: ChangeDetectorRef,
        // private appNavigationService: AppNavigationService,

    ) {
        super(injector);
        this.setButtonAddVisible(false);
        this.setButtonUpdateVisible(false);
        this.setButtonSaveVisible(false);
        this.setButtonDeleteVisible(false);
        this.setButtonApproveVisible(false);
        this.setButtonViewDetailVisible(true);
        this.setButtonSearchVisible(true);
        this.setButtonResetSearchVisible(false);
        this.setButtonSaveVisible(true);
        this.enable = true;

        this.editLabel = this.l("Edit");
    }

    ngOnInit(): void {
        // this.imgPath = `../../../../assets/icons/${this.appNavigationService.theme.value}/`;
        this.cdr.detectChanges();
    }

    public setUiAction(uiAction: IUiAction<any>): void {
        this.uiAction = uiAction;
    }

    public setEnable(enable: boolean) {
        this.enable = enable;
    }

    public setEnableForListPage(): void {
        this.setButtonAddEnable(true);
        this.setButtonUpdateEnable(false);
        this.setButtonSaveEnable(false);
        this.setButtonViewDetailEnable(false);
        this.setButtonDeleteEnable(false);
        this.setButtonApproveEnable(false);
        this.setButtonSearchEnable(true);
        this.setButtonResetSearchEnable(true);
        this.setButtonResetSearchVisible(true);
        this.setButtonRejectEnable(false);
        this.setButtonSendAppEnable(true);
        this.selectedItem = null;
        this.isList = true;
    }

    public setEnableForEditPage(): void {
        this.setButtonAddEnable(false);
        this.setButtonUpdateEnable(false);
        this.setButtonSaveEnable(true);
        this.setButtonViewDetailEnable(false);
        this.setButtonDeleteEnable(false);
        this.setButtonApproveEnable(false);
        this.setButtonSearchEnable(false);
        this.setButtonResetSearchEnable(false);
        this.setButtonRejectEnable(false);
        this.setButtonSendAppEnable(true);
        this.isEdit = true;
    }

    public setEnableForViewDetailPage(): void {
        this.setButtonAddEnable(false);
        this.setButtonUpdateEnable(false);
        this.setButtonSaveEnable(false);
        this.setButtonViewDetailEnable(false);
        this.setButtonDeleteEnable(false);
        this.setButtonApproveEnable(true);
        this.setButtonSearchEnable(false);
        this.setButtonResetSearchEnable(false);
        this.setButtonRejectEnable(true);
        this.setButtonSendAppEnable(false);
        this.isEdit = true;
    }

    public setEnableForViewOnly(): void {
        this.setButtonAddEnable(false);
        this.setButtonUpdateEnable(false);
        this.setButtonSaveEnable(false);
        this.setButtonViewDetailEnable(false);
        this.setButtonDeleteEnable(false);
        this.setButtonApproveEnable(false);
        this.setButtonSearchEnable(false);
        this.setButtonResetSearchEnable(false);
        this.isEdit = true;
    }

    public onSelectRow(item: any): void {
        this.selectedItem = item;
        if (item == null) {
            this.setEnableForListPage();
        } else {
            this.setButtonUpdateEnable(true);
            this.setButtonApproveEnable(true);
            this.setButtonViewDetailEnable(true);
            this.setButtonDeleteEnable(true);
        }
    }

    setVisible(add: boolean = false, edit: boolean = false, update: boolean = false, del: boolean = false, view: boolean = false, search: boolean = false, approve: boolean = false, resetSearch: boolean = false) {
        this.isList = true;
        this.isEdit = true;
        this.buttonAddHidden = !add;
        this.buttonUpdateHidden = !edit;
        this.buttonSaveHidden = !update;
        this.buttonViewDetailHidden = !view;
        this.buttonDeleteHidden = !del;
        this.buttonApproveHidden = !approve;
        this.buttonSearchHidden = !search;
        this.buttonResetSearchHidden = !resetSearch;
    }

    setPrefix(prefix: 'Pages.Administration' | 'Pages.Main') {
        this.prefix = prefix;
    }

    setPage(isPage: 'isEdit' | 'isList') {
        this.isEdit = isPage == 'isEdit';
        this.isList = isPage == 'isList';
    }

    setRole(funct: string, add: boolean, edit: boolean, update: boolean, del: boolean, view: boolean, search: boolean, approve: boolean, resetSearch: boolean, reject?: boolean, sendApp?: boolean) {
        this.funct = funct;
        this.setButtonAddVisible(add
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Create)
        );
        this.setButtonUpdateVisible(edit
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Edit)
        );
        this.setButtonSaveVisible(update
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Update)
        );
        this.setButtonDeleteVisible(del
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Delete)
        );
        this.setButtonViewDetailVisible(view
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.View)
        );
        this.setButtonSearchVisible(search
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Search)
        );
        this.setButtonApproveVisible(approve
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Approve)
        );
        this.setButtonResetSearchVisible(true);
        if (reject) {
            this.setButtonRejectVisible(reject
                && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Approve)
            );
        }
        if (sendApp) {
            this.setButtonSendAppVisible(sendApp
                && (this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Update)
                    || this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Create)
                    || this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.View)));
        }
    }

    setButtonAddEnable(enable: boolean): void {
        if (!this.buttonAddVisible) {
            enable = false;
        }
        this.buttonAddEnable = enable;
    }

    setButtonUpdateEnable(enable: boolean): void {
        if (!this.buttonUpdateVisible) {
            enable = false;
        }
        this.buttonUpdateEnable = enable;
    }

    setButtonSaveEnable(enable: boolean): void {
        if (!this.buttonSaveVisible) {
            enable = false;
        }
        this.buttonSaveEnable = enable;
    }

    setButtonViewDetailEnable(enable: boolean): void {
        if (!this.buttonViewDetailVisible) {
            enable = false;
        }
        this.buttonViewDetailEnable = enable;
    }

    setButtonDeleteEnable(enable: boolean): void {
        if (!this.buttonDeleteVisible) {
            enable = false;
        }
        this.buttonDeleteEnable = enable;
    }

    setButtonApproveEnable(enable: boolean): void {
        if (!this.buttonApproveVisible) {
            enable = false;
        }
        this.buttonApproveEnable = enable;
    }

    setButtonSearchEnable(enable: boolean): void {
        if (!this.buttonSearchVisible) {
            enable = false;
        }
        this.buttonSearchEnable = enable;
    }

    setButtonResetSearchEnable(enable: boolean): void {
        if (!this.buttonResetSearchVisible) {
            enable = false;
        }
        this.buttonResetSearchEnable = enable;
    }

    setButtonRejectEnable(enable: boolean): void {
        if (!this.buttonResetSearchVisible) {
            enable = false;
        }
        this.buttonRejectEnable = enable;
    }

    setButtonSendAppEnable(enable: boolean): void {
        if (!this.buttonSendAppVisible) {
            enable = false;
        }
        this.buttonSendAppEnable = enable;
    }

    setButtonAddVisible(visible: boolean): void {
        this.buttonAddVisible = visible;
    }

    setButtonUpdateVisible(visible: boolean): void {
        this.buttonUpdateVisible = visible;
    }

    setButtonSaveVisible(visible: boolean): void {
        this.buttonSaveVisible = visible;
    }

    setButtonViewDetailVisible(visible: boolean): void {
        this.buttonViewDetailVisible = visible;
    }

    setButtonDeleteVisible(visible: boolean): void {
        this.buttonDeleteVisible = visible;
    }

    setButtonApproveVisible(visible: boolean): void {
        this.buttonApproveVisible = visible;
    }

    setButtonSearchVisible(visible: boolean): void {
        this.buttonSearchVisible = visible;
    }

    setButtonResetSearchVisible(visible: boolean): void {
        this.buttonResetSearchVisible = visible;
    }

    setButtonRejectVisible(visible: boolean): void {
        this.buttonRejectVisible = visible;
    }

    setButtonSendAppVisible(visible: boolean): void {
        this.buttonSendAppVisible = visible;
    }

    hasAction(action) {
        this.permission.isGranted('Pages.Administration.' + this.funct + '.' + action)
    }

    add(): void {
        if (this.uiAction) {
            this.uiAction.onAdd();
        }
    }

    update(): void {
        if (this.selectedItem && this.uiAction) {
            this.uiAction.onUpdate(this.selectedItem);
        }
    }

    save(): void {
        if (this.uiAction) {
            this.uiAction.onSave();
        }
    }

    viewDetail(): void {
        if (this.selectedItem && this.uiAction) {
            this.uiAction.onViewDetail(this.selectedItem);
        }
    }

    delete(): void {
        if (this.selectedItem && this.uiAction) {
            this.uiAction.onDelete(this.selectedItem);
        }
    }

    approve(btnElmt?: any): void {
        if (this.uiAction) {
            this.uiAction.onApprove(this.selectedItem, btnElmt);
        }
    }

    search(): void {
        if (this.uiAction) {
            $('#alert-message').remove();
            this.uiAction.onSearch();
        }
    }

    resetSearch(): void {
        if (this.uiAction) {
            this.uiAction.onResetSearch();
        }
    }

    reject(): void {
        if (this.uiAction) {
            this.uiAction.onReject(this.selectedItem);
        }
    }

    sendApp(): void {
        if (this.uiAction) {
            this.uiAction.onSendApp(this.selectedItem);
        }
    }
}
