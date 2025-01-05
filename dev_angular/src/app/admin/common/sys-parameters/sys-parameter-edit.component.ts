import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { AllCodes } from '@app/utilities/enum/all-codes';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { SYS_PARAMETERS_ENTITY, SysParametersServiceProxy } from '@shared/service-proxies/service-proxies';

import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './sys-parameter-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SysParameterEditComponent extends DefaultComponentBase implements IUiAction<SYS_PARAMETERS_ENTITY>, OnInit, AfterViewInit {
    constructor(
        injector: Injector,
        private sysParameterService: SysParametersServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.id = this.getRouteParam('id');
        this.initFilter();
    }
    @ViewChild('editForm') editForm: ElementRef;

    EditPageState = EditPageState;
    AllCodes = AllCodes;
    editPageState: EditPageState;
    inputModel: SYS_PARAMETERS_ENTITY = new SYS_PARAMETERS_ENTITY();
    filterInput: SYS_PARAMETERS_ENTITY;
    isShowError = false;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    ngOnInit(): void {
    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Administration');
        switch (this.editPageState) {
            case EditPageState.add:
                this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
                this.appToolbar.setRole('SysParameter', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('SysParameter', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getSysParameter();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('SysParameter', false, false, false, false, false, false, true, false);
                this.appToolbar.setButtonApproveVisible(false);
                this.getSysParameter();
                break;
        }

        this.appToolbar.setUiAction(this);
        this.cdr.detectChanges();
    }

    getSysParameter() {
        this.sysParameterService.sYS_PARAMETERS_ById(this.inputModel.id).subscribe(response => {
            this.inputModel = response;
            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) {
                this.appToolbar.setButtonApproveEnable(false);
            }
        });
    }

    saveInput() {
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.showErrorMessage(this.l('FormInvalid'));
            return;
        }

        if (this.editPageState != EditPageState.viewDetail) {
            this.showLoading();
            this.showLoading();
            if (!this.inputModel.id) {

                this.sysParameterService.sYS_PARAMETERS_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            this.addNewSuccess();
                        }
                    });
            } else {
                this.sysParameterService.sYS_PARAMETERS_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            this.updateSuccess();
                        }
                    });
            }
        }
    }

    goBack() {
        this.navigatePassParam('/app/admin/sys-parameter', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: SYS_PARAMETERS_ENTITY): void {
    }

    onDelete(item: SYS_PARAMETERS_ENTITY): void {
    }

    onApprove(item: SYS_PARAMETERS_ENTITY): void {

    }

    onViewDetail(item: SYS_PARAMETERS_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }

    onReject(item: SYS_PARAMETERS_ENTITY): void {
        throw new Error('Method not implemented.');
    }
    onSendApp(item: SYS_PARAMETERS_ENTITY): void {
        throw new Error('Method not implemented.');
    }

}
