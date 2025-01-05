import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AllCodeServiceProxy, CM_ALLCODE_ENTITY } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './all-code-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AllCodeEditComponent extends DefaultComponentBase implements OnInit, IUiAction<CM_ALLCODE_ENTITY>, AfterViewInit {

    constructor(
        injector: Injector,
        private allCodeService: AllCodeServiceProxy,
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.id = this.getRouteParam('id');
        this.inputModel.cdname = this.getRouteParam('cdName');
        this.inputModel.cdtype = this.getRouteParam('cdType');
        this.inputModel.cdval = this.getRouteParam('cdVal');
        this.initFilter();
    }

    @ViewChild('editForm') editForm: ElementRef;
    EditPageState: EditPageState;
    editPageState: EditPageState;

    inputModel: CM_ALLCODE_ENTITY = new CM_ALLCODE_ENTITY();
    filterInput: CM_ALLCODE_ENTITY;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    isShowError = false;

    ngOnInit(): void {

    }

    ngAfterViewInit(): void {
        switch (this.editPageState) {
            case EditPageState.add:
                this.appToolbar.setRole('AllCode', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('AllCode', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getAllCode();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('AllCode', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getAllCode();
                break;
        }

        this.appToolbar.setUiAction(this);
    }


    getAllCode() {
        this.allCodeService.cM_ALLCODE_ById_v2(this.inputModel.cdtype, this.inputModel.cdname, this.inputModel.cdval).subscribe(response => {
            this.inputModel = response;
            this.appToolbar.setButtonApproveEnable(false);
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
            if (!this.inputModel.id) {
                this.allCodeService.cM_ALLCODE_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.addNewSuccess();
                        }
                    });
            }
            else {
                this.allCodeService.cM_ALLCODE_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.updateSuccess();
                            this.getAllCode();
                        }
                    });
            }
        }
    }

    goBack() {
        this.navigatePassParam('/app/admin/all-code', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: CM_ALLCODE_ENTITY): void {
    }

    onDelete(item: CM_ALLCODE_ENTITY): void {
    }

    onApprove(item: CM_ALLCODE_ENTITY): void {
    }

    onViewDetail(item: CM_ALLCODE_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }

    onReject(item: CM_ALLCODE_ENTITY): void {
        throw new Error('Method not implemented.');
    }
    
    onSendApp(item: CM_ALLCODE_ENTITY): void {
        throw new Error('Method not implemented.');
    }

}
