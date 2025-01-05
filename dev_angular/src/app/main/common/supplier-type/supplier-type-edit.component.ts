import { Component, Injector, ViewChild, OnInit, ViewEncapsulation, ElementRef, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import { SupplierTypeServiceProxy, CM_SUPPLIER_TYPE_ENTITY, UltilityServiceProxy } from '@shared/service-proxies/service-proxies';
import { AllCodes } from '@app/utilities/enum/all-codes';
import { finalize } from 'rxjs/operators';
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';

@Component({
    templateUrl: './supplier-type-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class SupplierTypeEditComponent extends DefaultComponentBase implements OnInit {
    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private ultilityService: UltilityServiceProxy,
        private supplierTypeService: SupplierTypeServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.suP_TYPE_ID = this.getRouteParam('id');
        this.initFilter();
        this.initIsApproveFunct();
    }

    @ViewChild('editForm') editForm: ElementRef;

    EditPageState = EditPageState;
    AllCodes = AllCodes;
    editPageState: EditPageState;

    inputModel: CM_SUPPLIER_TYPE_ENTITY = new CM_SUPPLIER_TYPE_ENTITY();
    filterInput: CM_SUPPLIER_TYPE_ENTITY;
    isApproveFunct: boolean;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    isShowError = false;

    ngOnInit(): void {
        
    }

    ngAfterViewInit(): void {
        // COMMENT: this.stopAutoUpdateView();
        // this.setupValidationMessage();
        // this.appToolbar.setPrefix('Pages.Main');
        // switch (this.editPageState) {
        //     case EditPageState.add:
        //         this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
        //         this.appToolbar.setRole('SupplierType', false, false, true, false, false, false, false, false);
        //         this.appToolbar.setEnableForEditPage();
        //         break;
        //     case EditPageState.edit:
        //         this.appToolbar.setRole('SupplierType', false, false, true, false, false, false, false, false);
        //         this.appToolbar.setEnableForEditPage();
        //         this.getSupplierType();
        //         break;
        //     case EditPageState.viewDetail:
        //         this.appToolbar.setRole('SupplierType', false, false, false, false, false, false, true, false);
        //         this.appToolbar.setEnableForViewDetailPage();
        //         this.getSupplierType();
        //         break;
        // }

        // this.appToolbar.setUiAction(this);
        // this.changeDetector.detectChanges();
    }

    initIsApproveFunct() {
        this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe(isApproveFunct => {
            this.isApproveFunct = isApproveFunct;
        })
    }


    getSupplierType() {
        this.supplierTypeService.cM_SUPPLIERTYPE_ById(this.inputModel.suP_TYPE_ID).subscribe(response => {
            this.inputModel = response;
            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) {
                this.appToolbar.setButtonApproveEnable(false);
            }
            this.updateView();
        });
    }

    get editTable(): boolean {
        return this.editPageState != EditPageState.viewDetail && this.editPageState != EditPageState.edit;
    }

    // saveInput() {
    //     if (this.isApproveFunct == undefined) {
    //         this.showErrorMessage(this.l('PageLoadUndone'));
    //         return;
    //     }

    //     if ((this.editForm as any).form.invalid) {
    //         this.isShowError = true;
    //         this.showErrorMessage(this.l('FormInvalid'));
    //         return;
    //     }
    //     if (this.editPageState != EditPageState.viewDetail) {
    //         this.saving = true;
    //         this.inputModel.makeR_ID = this.appSession.user.userName;
    //         if (!this.inputModel.suP_TYPE_ID) {
    //             this.supplierTypeService.cM_SUPPLIERTYPE_Ins(this.inputModel).pipe(finalize(() => { this.saving = false; }))
    //                 .subscribe((response) => {
    //                     if (response.result != '0') {
    //                         this.showErrorMessage(response.errorDesc);
    //                     }
    //                     else {
    //                         this.addNewSuccess();
    //                         if (!this.isApproveFunct) {
    //                             this.supplierTypeService.cM_SUPPLIERTYPE_App(response.id, this.appSession.user.userName)
    //                                 .pipe(finalize(() => { this.saving = false; }))
    //                                 .subscribe((response) => {
    //                                     if (response.result != '0') {
    //                                         this.showErrorMessage(response.errorDesc);
    //                                     }
    //                                 });
    //                         }
    //                     }
    //                 });
    //         }
    //         else {
    //             this.supplierTypeService.cM_SUPPLIERTYPE_Upd(this.inputModel).pipe(finalize(() => { this.saving = false; }))
    //                 .subscribe((response) => {
    //                     if (response.result != '0') {
    //                         this.showErrorMessage(response.errorDesc);
    //                     }
    //                     else {
    //                         this.updateSuccess();
    //                         if (!this.isApproveFunct) {
    //                             this.supplierTypeService.cM_SUPPLIERTYPE_App(this.inputModel.suP_TYPE_ID, this.appSession.user.userName)
    //                                 .pipe(finalize(() => { this.saving = false; }))
    //                                 .subscribe((response) => {
    //                                     if (response.result != '0') {
    //                                         this.showErrorMessage(response.errorDesc);
    //                                     }
    //                                     else {
    //                                         this.inputModel.autH_STATUS = AuthStatusConsts.Approve;
    //                                         this.appToolbar.setButtonApproveEnable(false);
    //                                         this.updateView();
    //                                     }
    //                                 });
    //                         }
    //                         else {
    //                             this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove;
    //                             this.updateView();
    //                         }
    //                     }
    //                 });
    //         }
    //     }
    // }

    goBack() {
        this.navigatePassParam('/app/admin/supplier-type', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: CM_SUPPLIER_TYPE_ENTITY): void {
    }

    onDelete(item: CM_SUPPLIER_TYPE_ENTITY): void {
    }

    // onApprove(item: CM_SUPPLIER_TYPE_ENTITY): void {
    //     if (!this.inputModel.suP_TYPE_ID) {
    //         return;
    //     }
    //     var currentUserName = this.appSession.user.userName;
    //     if (currentUserName == this.inputModel.makeR_ID) {
    //         this.showErrorMessage(this.l('ApproveFailed'));
    //         return;
    //     }
    //     this.message.confirm(
    //         this.l('ApproveWarningMessage', this.l(this.inputModel.suP_TYPE_NAME)),
    //         this.l('AreYouSure'),
    //         (isConfirmed) => {
    //             if (isConfirmed) {
    //                 this.saving = true;
    //                 this.supplierTypeService.cM_SUPPLIERTYPE_App(this.inputModel.suP_TYPE_ID, currentUserName)
    //                     .pipe(finalize(() => { this.saving = false; }))
    //                     .subscribe((response) => {
    //                         if (response.result != '0') {
    //                             this.showErrorMessage(response.errorDesc);
    //                         }
    //                         else {
    //                             this.approveSuccess();
    //                         }
    //                     });
    //             }
    //         }
    //     );
    // }

    onViewDetail(item: CM_SUPPLIER_TYPE_ENTITY): void {
    }

    // onSave(): void {
    //     this.saveInput();
    // }

    onSearch(): void {
    }

    onResetSearch(): void {
    }

    onReject(item: CM_SUPPLIER_TYPE_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSendApp(item: CM_SUPPLIER_TYPE_ENTITY): void {
        throw new Error("Method not implemented.");
    }
}
