import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CM_HANGHOA_ENTITY } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { CM_DVDM_ENTITY, CM_HANGHOA_TYPE_ENTITY, CM_SUPPLIER_ENTITY, CM_UNIT_ENTITY, DVDMServiceProxy, HangHoaServiceProxy, HangHoaTypeServiceProxy, SYS_GROUP_LIMIT_ENTITY, SupplierServiceProxy, SysGroupLimitServiceProxy, UltilityServiceProxy, UnitServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { IUiAction } from '@app/utilities/ui-action';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { AllCodes } from '@app/utilities/enum/all-codes';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
@Component({
    templateUrl: './hanghoa-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class HangHoaEditComponent extends DefaultComponentBase implements OnInit, IUiAction<CM_HANGHOA_ENTITY>, AfterViewInit {
    constructor(
        injector: Injector,
        private ultilityService: UltilityServiceProxy,
        private unitService: UnitServiceProxy,
        private hangHoaTypeService: HangHoaTypeServiceProxy,
        private supplierService: SupplierServiceProxy,
        private sysGroupLimitService: SysGroupLimitServiceProxy,
        private dvdmService: DVDMServiceProxy,
        private hangHoaService: HangHoaServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.hH_ID = this.getRouteParam('id');
        this.initFilter();
        this.initCombobox();
        this.initIsApproveFunct();
    }
    onReject(item: CM_HANGHOA_ENTITY): void {
        throw new Error('Method not implemented.');
    }
    onSendApp(item: CM_HANGHOA_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    @ViewChild('editForm') editForm: ElementRef;

    EditPageState = EditPageState;
    AllCodes = AllCodes;
    editPageState: EditPageState;

    inputModel: CM_HANGHOA_ENTITY = new CM_HANGHOA_ENTITY();
    filterInput: CM_HANGHOA_ENTITY;
    isApproveFunct: boolean;
    rExp: RegExp = /^\S+$/;
    rExpSp: RegExp = /^\S+/;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    units: CM_UNIT_ENTITY[];
    hangHoaTypes: CM_HANGHOA_TYPE_ENTITY[];
    sysGroupLimitTTCT: SYS_GROUP_LIMIT_ENTITY[];
    sysGroupLimitCDT: SYS_GROUP_LIMIT_ENTITY[];
    dvdmList: CM_DVDM_ENTITY[];
    suppliers: CM_SUPPLIER_ENTITY[];

    isShowError = false;

    ngOnInit(): void {
    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        switch (this.editPageState) {
            case EditPageState.add:
                this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
                this.appToolbar.setRole('HangHoa', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('HangHoa', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getHangHoa();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('HangHoa', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getHangHoa();
                break;
        }
        this.cdr.detectChanges();
    }

    initIsApproveFunct() {
        this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe(isApproveFunct => {
            this.isApproveFunct = isApproveFunct;
        })
    }

    initCombobox() {
        this.unitService.cM_UNIT_Search(this.getFillterForCombobox()).subscribe(response => {
            this.units = response.items;
        });

        this.supplierService.cM_SUPPLIER_Search(this.getFillterForCombobox()).subscribe(response => {
            this.suppliers = response.items;
        })

        this.hangHoaTypeService.cM_HANGHOA_TYPE_GetAll().subscribe(res => {
            this.hangHoaTypes = res;
        });
        let sysGroupLimit_TTCT = new SYS_GROUP_LIMIT_ENTITY();
        sysGroupLimit_TTCT.type = 'TTCT';
        sysGroupLimit_TTCT.maxResultCount = -1;

        this.sysGroupLimitService.sys_Group_Limit_Search(sysGroupLimit_TTCT).subscribe(res => {
            this.sysGroupLimitTTCT = res.items;
        });
        let sysGroupLimit_CDT = new SYS_GROUP_LIMIT_ENTITY();
        sysGroupLimit_CDT.type = 'CDT';
        sysGroupLimit_CDT.maxResultCount = -1;

        this.sysGroupLimitService.sys_Group_Limit_Search(sysGroupLimit_CDT).subscribe(res => {
            this.sysGroupLimitCDT = res.items;
        });

        this.dvdmService.cM_DVDM_GetAll().subscribe(res => {
            this.dvdmList = res;
        });


    }

    getHangHoa() {
        this.hangHoaService.cM_HANGHOA_ById(this.inputModel.hH_ID).subscribe(response => {
            this.inputModel = response;
        });
    }

    saveInput() {


        if (!this.rExp.test(this.inputModel.hH_CODE)) {
            this.showErrorMessage(this.l('Mã hàng hóa có khoảng trắng, vui lòng không nhập dấu cách'));
            return;
        }

        if (!this.rExpSp.test(this.inputModel.hH_NAME)) {
            this.showErrorMessage(this.l('Tên hàng hóa có khoảng trắng đầu tiên, vui lòng nhập ký tự trước'));
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
            // this.inputModel.hH_CODE = this.inputModel.gD_CODE.replace(/\s+/g, '').trim();
            if (!this.inputModel.hH_ID) {
                this.hangHoaService.cM_HANGHOA_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.inputModel.hH_ID = response.id;
                            this.inputModel.hH_CODE = response.ids;
                            setTimeout(() => {
                                this.addNewSuccess();
                            }, 1500);
                            this.navigatePassParam('/app/main/hanghoa-edit', { id: response["id"] }, { inputModel: JSON.stringify(this.inputModel) })
                        }
                    });
            }
            else {
                this.hangHoaService.cM_HANGHOA_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.updateSuccess();
                            if (!this.isApproveFunct) {
                                this.hangHoaService.cM_HANGHOA_App(this.inputModel.hH_ID, this.appSession.user.userName)
                                    .pipe(finalize(() => { this.hideLoading(); }))
                                    .subscribe((response) => {
                                        if (response.result != '0') {
                                            this.showErrorMessage(response.errorDesc);
                                        } else {
                                            this.inputModel.autH_STATUS = AuthStatusConsts.Approve;
                                            this.appToolbar.setButtonApproveEnable(false);
                                        }
                                    });
                            } else {
                                this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove;
                            }
                        }
                    });
            }
        }
    }

    goBack() {
        this.navigatePassParam('/app/main/hanghoa', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: CM_HANGHOA_ENTITY): void {
    }

    onDelete(item: CM_HANGHOA_ENTITY): void {
    }

    onApprove(item: CM_HANGHOA_ENTITY): void {
        if (!this.inputModel.hH_ID) {
            return;
        }
        var currentUserName = this.appSession.user.userName;
        if (currentUserName == this.inputModel.makeR_ID) {
            this.showErrorMessage(this.l('ApproveFailed'));
            return;
        }
        this.message.confirm(
            this.l('ApproveWarningMessage', this.l(this.inputModel.hH_NAME)),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.hangHoaService.cM_HANGHOA_App(this.inputModel.hH_ID, currentUserName)
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
    onViewDetail(item: CM_HANGHOA_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }
}
