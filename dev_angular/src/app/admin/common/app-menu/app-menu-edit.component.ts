import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppMenuServiceProxy, TL_MENU_ENTITY } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';

@Component({
    templateUrl: './app-menu-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AppMenuEditComponent extends DefaultComponentBase implements OnInit, IUiAction<TL_MENU_ENTITY>, AfterViewInit {

    @ViewChild('editForm') editForm: ElementRef;

    PagesMain: string = 'Pages.Main';
    PagesAdministration: string = 'Pages.Administration';

    editPageState: EditPageState;

    filterInput: TL_MENU_ENTITY;
    inputModel: TL_MENU_ENTITY = new TL_MENU_ENTITY();
    menuItems: TL_MENU_ENTITY[];
    prefixList: any[] = [{ value: this.PagesAdministration }, { value: this.PagesMain }];
    selectedMenu: TL_MENU_ENTITY;
    baseLink: string
    isApproveFunct: boolean;


    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private menuService: AppMenuServiceProxy,
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.menU_ID = this.getRouteParam('id');
        console.log('Filter:', this.getFillterForCombobox());
        this.menuService.tL_MENU_Search(this.getFillterForCombobox())
            .subscribe((response) => {
                console.log(response);
                this.menuItems = response.items;
            })
    }

    onReject(item: TL_MENU_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSendApp(item: TL_MENU_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    ngOnInit(): void { }

    ngAfterViewInit(): void {
        this.appToolbar.setUiAction(this);
        this.appToolbar.setPrefix('Pages.Administration');
        switch (this.editPageState) {
            case EditPageState.add:
                this.appToolbar.setRole('Menu', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.initDefault();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('Menu', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getAppMenu();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('Menu', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getAppMenu();
                break;
        }
        this.changeDetector.detectChanges();
    }

    initDefault(): void {
        this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
        this.inputModel.prefix = this.PagesMain;
        this.inputModel.menU_ICON = 'fa-solid fa-folder';
        this.inputModel.isapprovE_FUNC = '1'
    }

    getAppMenu(): void {
        this.menuService.tL_MENU_ById(this.inputModel.menU_ID).subscribe(response => {
            this.inputModel = response;
            if (this.inputModel.menU_LINK.startsWith('/app/main/')) {
                this.inputModel.prefix = this.PagesMain;
                this.inputModel.menU_LINK = this.inputModel.menU_LINK.replace('/app/main/', '');
            }
            else if (this.inputModel.menU_LINK.startsWith('/app/admin/')) {
                this.inputModel.prefix = this.PagesAdministration;
                this.inputModel.menU_LINK = this.inputModel.menU_LINK.replace('/app/admin/', '');
            }

            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) this.appToolbar.setButtonApproveEnable(false);
        });
    }

    saveInput() {
        // if (this.isApproveFunct == undefined) {
        //     this.showErrorMessage(this.l('PageLoadUndone'));
        //     return;
        // }

        if ((this.editForm as any).form.invalid) {

            this.showErrorMessage(this.l('FormInvalid'));
            return;
        }

        this.inputModel.makeR_ID = this.appSession.user.userName;
        this.showLoading();
        const saveInput: TL_MENU_ENTITY = { ...this.inputModel } as TL_MENU_ENTITY;
        saveInput.menU_LINK = (this.inputModel.prefix == this.PagesMain ? '/app/main/' : '/app/admin/') + this.inputModel.menU_LINK;

        if (this.editPageState != EditPageState.viewDetail) {
            if (!this.inputModel.menU_ID) {
                this.menuService.tL_MENU_Ins(saveInput).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {

                            this.addNewSuccess();
                            if (!this.isApproveFunct) {
                                this.menuService.tL_MENU_App(parseInt(response.id), this.appSession.user.userName)
                                    .pipe(finalize(() => { this.hideLoading(); }))
                                    .subscribe((response) => {
                                        if (response.result != '0') {
                                            this.showErrorMessage(response.errorDesc);
                                        }
                                    });
                            }
                        }
                    });
            }
            else {
                this.menuService.tL_MENU_Upd(saveInput).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.updateSuccess();
                            if (!this.isApproveFunct) {
                                this.menuService.tL_MENU_App(this.inputModel.menU_ID, this.appSession.user.userName)
                                    .pipe(finalize(() => { this.hideLoading(); }))
                                    .subscribe((response) => {
                                        if (response.result != '0') {
                                            this.showErrorMessage(response.errorDesc);
                                        }
                                        else {
                                            this.inputModel.autH_STATUS = AuthStatusConsts.Approve;
                                            this.appToolbar.setButtonApproveEnable(false);
                                        }
                                    });
                            }
                            else {
                                this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove;
                            }
                        }
                    });
            }
        }
    }

    goBack() {
        this.navigatePassParam('/app/admin/app-menu', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: TL_MENU_ENTITY): void {
    }

    onDelete(item: TL_MENU_ENTITY): void {
    }

    onApprove(item: TL_MENU_ENTITY): void {
        if (!this.inputModel.menU_ID) {
            return;
        }

        if (this.appSession.user.userName == this.inputModel.makeR_ID) {
            this.showErrorMessage(this.l('ApproveFailed'));
            return;
        }

        this.message.confirm(
            this.l('ApproveWarningMessage', this.inputModel.menU_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.menuService.tL_MENU_App(this.inputModel.menU_ID, this.appSession.user.userName)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.approveSuccess();
                            }
                        });
                }
            }
        );
    }

    onViewDetail(item: TL_MENU_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }

    onChangeEnglishName(event: any): void {
        this.inputModel.menU_LINK = this.inputModel.menU_NAME_EL.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
    }
    imgPath = '../../../../assets/new-set-icons/'
    checkIsIconClass(value: string) {
        return value.includes('fa')
    }
}
