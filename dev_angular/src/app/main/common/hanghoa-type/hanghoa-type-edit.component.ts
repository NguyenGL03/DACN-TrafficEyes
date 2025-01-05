import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { CM_HANGHOA_GROUP_ENTITY, CM_HANGHOA_TYPE_ENTITY, HangHoaGroupServiceProxy, HangHoaTypeServiceProxy, UltilityServiceProxy } from '../../../../shared/service-proxies/service-proxies';
import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { finalize } from 'rxjs/operators';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { AllCodes } from '@app/utilities/enum/all-codes';

@Component({
    templateUrl: './hanghoa-type-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class HangHoaTypeEditComponent extends DefaultComponentBase implements OnInit, IUiAction<CM_HANGHOA_TYPE_ENTITY>, AfterViewInit {

    constructor(
        injector: Injector,
        private ultilityService: UltilityServiceProxy,
        private hangHoaGroupService: HangHoaGroupServiceProxy,
        private hangHoaTypeService: HangHoaTypeServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.hH_TYPE_ID = this.getRouteParam('id');
        this.initFilter();
        this.initIsApproveFunct();
        this.initCombobox();
    }

    @ViewChild('editForm') editForm: ElementRef;

    EditPageState = EditPageState;
    AllCodes = AllCodes;
    editPageState: EditPageState;

    hangHoaGroups: CM_HANGHOA_GROUP_ENTITY[];
    inputModel: CM_HANGHOA_TYPE_ENTITY = new CM_HANGHOA_TYPE_ENTITY();
    filterInput: CM_HANGHOA_TYPE_ENTITY;
    isApproveFunct: boolean;
    rExp: RegExp = /^\S+$/;
    rExpSp: RegExp = /^\S+/;
    authStatus = false;


    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail || this.authStatus == true;
    }

    isShowError = false;

    ngOnInit(): void {

    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        switch (this.editPageState) {
            case EditPageState.add:
                this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
                this.appToolbar.setRole('HangHoaType', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('HangHoaType', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getGoodsType();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('HangHoaType', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getGoodsType();
                break;
        }
        this.cdr.detectChanges();
    }

    initCombobox() {
        this.hangHoaGroupService.cM_HANGHOA_GROUP_GetAll().subscribe(res => {
            this.hangHoaGroups = res;
        });
    }

    initIsApproveFunct() {
        this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe(isApproveFunct => {
            this.isApproveFunct = isApproveFunct;
        })
    }


    getGoodsType() {
        this.hangHoaTypeService.cM_HANGHOA_TYPE_ById(this.inputModel.hH_TYPE_ID).subscribe(response => {
            this.inputModel = response;

        });
    }

    saveInput() {
        if (!this.rExp.test(this.inputModel.hH_TYPE_CODE)) {
            this.showErrorMessage(this.l('Mã loại hàng hóa có khoảng trắng,vui lòng không nhập dấu cách'));
            return;
        }

        if (!this.rExpSp.test(this.inputModel.hH_TYPE_NAME)) {
            this.showErrorMessage(this.l('Tên loại hàng hóa có khoảng trắng đầu tiên,vui lòng nhập ký tự trước'));
            return;
        }

        if (this.isApproveFunct == undefined) {
            this.showErrorMessage(this.l('PageLoadUndone'));
            return;
        }

        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.showErrorMessage(this.l('FormInvalid'));
            return;
        }
        if (this.editPageState != EditPageState.viewDetail) {
            this.showLoading();
            this.inputModel.makeR_ID = this.appSession.user.userName;
            if (!this.inputModel.hH_TYPE_ID) {
                this.hangHoaTypeService.cM_HANGHOA_TYPE_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            setTimeout(() => {
                                this.addNewSuccess();
                            }, 1500);
                            this.navigatePassParam('/app/main/hanghoatype-edit', { id: response["id"] }, { inputModel: JSON.stringify(this.inputModel) })

                        }
                    });
            }
            else {
                this.hangHoaTypeService.cM_HANGHOA_TYPE_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            this.updateSuccess();
                            this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove;
                        }
                    });
            }
        }
    }

    goBack() {
        this.navigatePassParam('/app/main/hanghoatype', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: CM_HANGHOA_TYPE_ENTITY): void {
    }

    onDelete(item: CM_HANGHOA_TYPE_ENTITY): void {
    }

    onApprove(item: CM_HANGHOA_TYPE_ENTITY): void {
        if (!this.inputModel.hH_TYPE_ID) {
            return;
        }
        var currentUserName = this.appSession.user.userName;
        if (currentUserName == this.inputModel.makeR_ID) {
            this.showErrorMessage(this.l('ApproveFailed'));
            return;
        }
        this.message.confirm(
            this.l('ApproveWarningMessage', this.l(this.inputModel.hH_TYPE_NAME)),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.hangHoaTypeService.cM_HANGHOA_TYPE_App(this.inputModel.hH_TYPE_ID, currentUserName)
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

    onViewDetail(item: CM_HANGHOA_TYPE_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }

    onReject(item: CM_HANGHOA_TYPE_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSendApp(item: CM_HANGHOA_TYPE_ENTITY): void {
        throw new Error('Method not implemented.');
    }
}
