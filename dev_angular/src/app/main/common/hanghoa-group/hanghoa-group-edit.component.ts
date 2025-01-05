import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { finalize } from 'rxjs/operators';
import { CM_HANGHOA_GROUP_ENTITY, HangHoaGroupServiceProxy, UltilityServiceProxy } from '../../../../shared/service-proxies/service-proxies';

@Component({
    templateUrl: './hanghoa-group-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class HangHoaGroupEditComponent extends DefaultComponentBase implements OnInit, IUiAction<CM_HANGHOA_GROUP_ENTITY>, AfterViewInit {

    constructor(
        injector: Injector,
        private ultilityService: UltilityServiceProxy,
        private hangHoaGroupService: HangHoaGroupServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.hH_GROUP_ID = this.getRouteParam('id');
        this.initFilter();
        this.initIsApproveFunct();
    }

    @ViewChild('editForm') editForm: ElementRef;

    EditPageState = EditPageState;
    editPageState: EditPageState;

    inputModel: CM_HANGHOA_GROUP_ENTITY = new CM_HANGHOA_GROUP_ENTITY();
    filterInput: CM_HANGHOA_GROUP_ENTITY;
    isApproveFunct: boolean;
    rExp: RegExp = /^\S+$/;
    rExpSp: RegExp = /^\S+/;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
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
                this.appToolbar.setRole('HangHoaGroup', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('HangHoaGroup', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getHangHoaGroup();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('HangHoaGroup', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getHangHoaGroup();
                break;
        }
        this.cdr.detectChanges();
    }


    initIsApproveFunct() {
        this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe(isApproveFunct => {
            this.isApproveFunct = isApproveFunct;
        })
    }


    getHangHoaGroup() {
        this.hangHoaGroupService.cM_HANGHOA_GROUP_ById(this.inputModel.hH_GROUP_ID).subscribe(response => {
            this.inputModel = response;

        });
    }

    saveInput() {

        if (!this.rExp.test(this.inputModel.hH_GROUP_CODE)) {
            this.showErrorMessage(this.l('Mã nhóm hàng hóa có khoảng trắng,vui lòng không nhập dấu cách'));
            return;
        }

        if (!this.rExpSp.test(this.inputModel.hH_GROUP_NAME)) {
            this.showErrorMessage(this.l('Tên nhóm hàng hóa có khoảng trắng đầu tiên,vui lòng nhập ký tự trước'));
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
            if (!this.inputModel.hH_GROUP_ID) {
                this.hangHoaGroupService.cM_HANGHOA_GROUP_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            setTimeout(() => {
                                this.addNewSuccess();
                            }, 1500);
                            this.navigatePassParam('/app/main/hanghoagroup-edit', { id: response["id"] }, { inputModel: JSON.stringify(this.inputModel) })

                        }
                    });
            }
            else {
                this.hangHoaGroupService.cM_HANGHOA_GROUP_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
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
        this.navigatePassParam('/app/main/hanghoagroup', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: CM_HANGHOA_GROUP_ENTITY): void {
    }

    onDelete(item: CM_HANGHOA_GROUP_ENTITY): void {
    }

    onApprove(item: CM_HANGHOA_GROUP_ENTITY): void {
        if (!this.inputModel.hH_GROUP_ID) {
            return;
        }
        var currentUserName = this.appSession.user.userName;
        if (currentUserName == this.inputModel.makeR_ID) {
            this.showErrorMessage(this.l('ApproveFailed'));
            return;
        }
        this.message.confirm(
            this.l('ApproveWarningMessage', this.l(this.inputModel.hH_GROUP_NAME)),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.hangHoaGroupService.cM_HANGHOA_GROUP_App(this.inputModel.hH_GROUP_ID, currentUserName)
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

    onViewDetail(item: CM_HANGHOA_GROUP_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }

    onReject(item: CM_HANGHOA_GROUP_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSendApp(item: CM_HANGHOA_GROUP_ENTITY): void {
        throw new Error('Method not implemented.');
    }

}
