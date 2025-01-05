import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { AllCodes } from '@app/utilities/enum/all-codes';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, CM_EMPLOYEE_ENTITY, CM_REGION_ENTITY, DepartmentServiceProxy, EmployeeServiceProxy, RegionServiceProxy } from '@shared/service-proxies/service-proxies';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { finalize } from 'rxjs';
import { DepartmentModalComponent } from '@app/shared/common/modals/dep-modal/department-modal.component';

@Component({
    templateUrl: './employee-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class EmployeeEditComponent extends DefaultComponentBase implements OnInit, IUiAction<CM_EMPLOYEE_ENTITY>, AfterViewInit {

    constructor(
        injector: Injector,
        // private ultilityService: UltilityServiceProxy,
        private departmentService: DepartmentServiceProxy,
        private branchService: BranchServiceProxy,
        private _regionService: RegionServiceProxy,
        private employeeService: EmployeeServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.emP_ID = this.getRouteParam('id');
        this.initFilter();
        this.initIsApproveFunct();
        // this.inputModel.brancH_ID = this.appSession.user.subbrId;
        // this.inputModel.brancH_TYPE = this.appSession.user.branch.brancH_TYPE;
    }
    onReject(item: CM_EMPLOYEE_ENTITY): void {
        throw new Error('Method not implemented.');
    }
    onSendApp(item: CM_EMPLOYEE_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    @ViewChild('editForm') editForm: ElementRef;
    @ViewChild('depModal') depModal: DepartmentModalComponent;

    EditPageState = EditPageState;
    AllCodes = AllCodes;
    editPageState: EditPageState;

    regions: CM_REGION_ENTITY[];
    branchs: CM_BRANCH_ENTITY[];
    lstBranch: CM_BRANCH_ENTITY[];
    areas: CM_BRANCH_ENTITY[];
    inputModel: CM_EMPLOYEE_ENTITY = new CM_EMPLOYEE_ENTITY();
    filterInput: CM_EMPLOYEE_ENTITY;
    isApproveFunct: boolean;

    branchType: string;


    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    isShowError = false;

    ngOnInit(): void {

    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        switch (this.editPageState) {
            case EditPageState.add:
                this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
                this.appToolbar.setRole('Employee', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.initComboboxs();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('Employee', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getEmployee();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('Employee', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getEmployee();
                break;
        }

        this.appToolbar.setUiAction(this);
        this.cdr.detectChanges();
    }

    initIsApproveFunct() {
        // this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe(isApproveFunct => {
        //     this.isApproveFunct = isApproveFunct;
        // })
    }

    initComboboxs() {
        var filterCombobox = this.getFillterForCombobox();
        this._regionService.cM_REGION_Search(this.getFillterForCombobox()).subscribe(response => {
            this.regions = response.items;
        });

        this.branchService.cM_BRANCH_Search(filterCombobox).subscribe(response => {
            this.lstBranch = response.items;
            this.branchs = response.items;
            if (this.editPageState == EditPageState.add) {
                let branch = this.branchs.firstOrDefault(undefined);
                this.inputModel.brancH_ID = branch.brancH_ID;
                this.branchType = branch.brancH_TYPE;
            }
            else {
                this.inputModel.khU_VUC = this.branchs.firstOrDefault(e => e.brancH_ID == this.inputModel.brancH_ID).regioN_ID;
            }
        });

    }
    reloadFatherList(item: CM_REGION_ENTITY) {
        this.branchs = this.lstBranch;
        this.branchs = this.branchs.filter(e => e.regioN_ID == item.regioN_ID);
        if (this.branchs.filter(x => x.brancH_ID == this.inputModel.brancH_ID).length == 0) {
            this.inputModel.brancH_ID = undefined;
        }
    }
    getEmployee() {
        this.employeeService.cM_EMPLOYEE_ById(this.inputModel.emP_ID).subscribe(response => {
            this.inputModel = response;
            this.initComboboxs()
            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) {
                this.appToolbar.setButtonApproveEnable(false);
            }
        });
    }
    depModalShow() {
        this.depModal.setRecords([], 0);
        this.depModal.branch_id = this.inputModel.brancH_ID;
        this.depModal.show();
    }

    KhuVuc: string = '';
    onChangeKhuVuc(item: any) {
        if (item == null) return;
        this.KhuVuc = item['content'];
        this.inputModel.khU_VUC = item['cdval']
    }

    onChangeBranch(branch: CM_BRANCH_ENTITY) {
        this.inputModel.brancH_TYPE = branch.brancH_TYPE;
        if (branch.brancH_TYPE != 'HS') {
            this.inputModel.deP_ID = undefined;
            this.inputModel.deP_NAME = undefined;
        }
        if (branch) {
            this.branchType = branch.brancH_TYPE;
        } else {
            // this.branchType = this.appSession.user.branch.brancH_TYPE;
        }
        // this.depModal.filterInput.brancH_ID = (branch || { branchID: this.appSession.user.subbrId } as any).brancH_ID;
        this.depModal.dataTable.records = [];
    }

    saveInput() {
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
            if (!this.inputModel.emP_ID) {
                this.employeeService.cM_EMPLOYEE_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            this.addNewSuccess();
                            if (!this.isApproveFunct) {
                                this.employeeService.cM_EMPLOYEE_App(response.id, this.appSession.user.userName)
                                    .pipe(finalize(() => { this.hideLoading(); }))
                                    .subscribe((response) => {
                                        if (response.result != '0') {
                                            this.showErrorMessage(response.errorDesc);
                                        }
                                    });
                            }
                        }
                    });
            } else {
                this.employeeService.cM_EMPLOYEE_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        } else {
                            this.updateSuccess();
                            if (!this.isApproveFunct) {
                                this.employeeService.cM_EMPLOYEE_App(this.inputModel.emP_ID, this.appSession.user.userName)
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
        this.navigatePassParam('/app/main/employee', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: CM_EMPLOYEE_ENTITY): void {
    }

    onDelete(item: CM_EMPLOYEE_ENTITY): void {
    }

    onApprove(item: CM_EMPLOYEE_ENTITY): void {
        if (!this.inputModel.emP_ID) return;

        var currentUserName = this.appSession.user.userName;
        if (currentUserName == this.inputModel.makeR_ID) {
            this.showErrorMessage(this.l('ApproveFailed'));
            return;
        }
        this.message.confirm(
            this.l('ApproveWarningMessage', this.l(this.inputModel.emP_NAME)),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.employeeService.cM_EMPLOYEE_App(this.inputModel.emP_ID, currentUserName)
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

    onSelectDepartment(dep: CM_DEPARTMENT_ENTITY) {
        this.inputModel.deP_ID = dep.deP_ID;
        this.inputModel.deP_NAME = dep.deP_NAME;
    }

    onViewDetail(item: CM_EMPLOYEE_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }
}
