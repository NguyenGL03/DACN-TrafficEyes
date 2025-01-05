import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { NgForm } from "@angular/forms";
import { AllCodeSelectComponent } from "@app/shared/core/controls/allCodes/all-code-select.component";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { AllCodes } from "@app/utilities/enum/all-codes";
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_REGION_ENTITY, RegionServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';

@Component({
    templateUrl: './branch-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BranchEditComponent extends DefaultComponentBase implements OnInit, AfterViewInit, IUiAction<CM_BRANCH_ENTITY> {
    @ViewChild('editForm') editForm: NgForm;

    EditPageState = EditPageState;
    AllCodes = AllCodes;
    editPageState: EditPageState;
    inputModel: CM_BRANCH_ENTITY = new CM_BRANCH_ENTITY();
    filterInput: CM_BRANCH_ENTITY;
    isApproveFunct: boolean;
    fatherLists: CM_BRANCH_ENTITY[];
    regions: CM_REGION_ENTITY[];
    isShowError = false;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    get editTable(): boolean {
        return this.editPageState != EditPageState.viewDetail && this.editPageState != EditPageState.edit;
    }

    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private branchService: BranchServiceProxy, 
        private regionService: RegionServiceProxy
        // private ultilityService: UltilityServiceProxy,
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.brancH_ID = this.getRouteParam('id');
    }

    ngOnInit(): void {
        this.inputModel.makeR_ID = this.appSession.user.userName;
        this.initDefault();
        this.initFilter();
        this.initCombobox();
    }

    initDefault() {
        this.inputModel.regioN_ID = 'REG000000000002';
        this.inputModel.brancH_TYPE = 'PGD';
        this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
    }
    
    initCombobox() {
        this.regionService.cM_REGION_Search(this.getFillterForCombobox())
            .subscribe(result => {
                this.regions = result.items;
            })
        this.branchService.cM_BRANCH_GetFatherList(this.inputModel.regioN_ID, this.inputModel.brancH_TYPE).subscribe(response => {
            this.fatherLists = response;
            this.onChangeProperty('fatheR_ID');
        })
    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        switch (this.editPageState) {
            case EditPageState.add:
                this.appToolbar.setRole('Branch', true, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('Branch', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getBranch();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('Branch', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getBranch();
                break;
        }
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }

    getBranch() {
        this.branchService.cM_BRANCH_ById(this.inputModel.brancH_ID).subscribe(response => {
            var oldFatherId = response.fatheR_ID;

            this.inputModel = response;
            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) {
                this.appToolbar.setButtonApproveEnable(false);
            }
            this.reloadFatherList();
            this.inputModel.fatheR_ID = oldFatherId;
            this.onChangeProperty('fatheR_ID');
        });
    }

    reloadFatherList() {
        if (!this.inputModel || !this.inputModel.regioN_ID || !this.inputModel.brancH_TYPE) return;

        this.branchService.cM_BRANCH_GetFatherList(this.inputModel.regioN_ID, this.inputModel.brancH_TYPE).subscribe(response => {
            this.fatherLists = response;
            this.onChangeProperty('fatheR_ID');
        })
    }

    saveInput(): void {

        if (this.editPageState != EditPageState.viewDetail) {
            if ((this.editForm as any).form.invalid) {
                this.isShowError = true;
                this.showErrorMessage(this.l('FormInvalid'));
                return;
            }

            this.showLoading();
            this.inputModel.makeR_ID = this.appSession.user.userName;
            if (!this.inputModel.brancH_ID) {
                this.branchService.cM_BRANCH_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            this.addNewSuccess();
                            if (!this.isApproveFunct) {
                                this.branchService.cM_BRANCH_App(response.id, this.appSession.user.userName)
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
                this.branchService.cM_BRANCH_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            this.updateSuccess();
                            if (!this.isApproveFunct) {
                                this.branchService.cM_BRANCH_App(this.inputModel.brancH_ID, this.appSession.user.userName)
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

    onAdd(): void {
        throw new Error('Method not implemented.');
    }

    onUpdate(item: CM_BRANCH_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onDelete(item: CM_BRANCH_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onApprove(item: CM_BRANCH_ENTITY, btnElmt?: any): void {
        throw new Error('Method not implemented.');
    }

    onViewDetail(item: CM_BRANCH_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
        throw new Error('Method not implemented.');
    }

    onResetSearch(): void {
        throw new Error('Method not implemented.');
    }

    onReject(item: CM_BRANCH_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSendApp(item: CM_BRANCH_ENTITY): void {
        throw new Error("Method not implemented.");
    }
}
