import { AfterViewInit, Component, Injector, OnInit, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';
import { PrimeTableOption, TextAlign } from '@app/shared/core/controls/primeng-table/prime-table/primte-table.interface';
import { ReportTypeConsts } from '@app/shared/core/utils/consts/ReportTypeConsts';
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AsposeServiceProxy, BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, CM_EMPLOYEE_ENTITY, CM_REGION_ENTITY, DepartmentServiceProxy, EmployeeServiceProxy, RegionServiceProxy, ReportInfo } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs/operators";

@Component({
    templateUrl: './employee-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class EmployeeListComponent extends ListComponentBase<CM_EMPLOYEE_ENTITY> implements IUiAction<CM_EMPLOYEE_ENTITY>, OnInit, AfterViewInit {
    options: PrimeTableOption<CM_EMPLOYEE_ENTITY>;
    filterInput: CM_EMPLOYEE_ENTITY = new CM_EMPLOYEE_ENTITY();
    departments: CM_DEPARTMENT_ENTITY[];
    branchs: CM_BRANCH_ENTITY[];
    branchsT: CM_BRANCH_ENTITY[];
    regions: CM_REGION_ENTITY[];

    constructor(
        injector: Injector,
        private fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private _departmentService: DepartmentServiceProxy,
        private _branchService: BranchServiceProxy,
        private _regionService: RegionServiceProxy,
        private _employeeService: EmployeeServiceProxy
    ) {
        super(injector);
    }

    initDefaultFilter() {
        this.filterInput.level = 'UNIT';
    }

    initCombobox() {
        let filterCombobox = this.getFillterForCombobox();

        this._branchService.cM_BRANCH_Search(filterCombobox).subscribe(response => {
            this.branchs = response.items;
            this.branchsT = response.items;
        });

        this._regionService.cM_REGION_Search(this.getFillterForCombobox()).subscribe(response => {
            this.regions = response.items;
        });
    }

    reloadFatherList(item: CM_REGION_ENTITY) {
        if (item != null)
            this.branchs = this.branchsT.filter(e => e.regioN_ID == item.regioN_ID);
        else
            this.branchs = this.branchsT;
        this.filterInput.brancH_ID = this.branchs[0].brancH_ID;
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'EmpCode', name: 'emP_CODE', sortField: 'emP_CODE', width: '150px' },
                { title: 'EmpFullName', name: 'emP_NAME', sortField: 'emP_NAME', width: '200px' },
                { title: 'EmpPosCode', name: 'poS_CODE', sortField: 'poS_CODE', width: '200px' },
                { title: 'EmpPosName', name: 'poS_NAME', sortField: 'poS_NAME', width: '150px' },
                { title: 'BranchName', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '200px' },
                { title: 'DepId', name: 'deP_NAME', sortField: 'deP_NAME', width: '200px' },
                { title: 'Area', name: 'khU_VUC', sortField: 'khU_VUC', width: '200px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '200px' },
                { title: 'RecordStatus_Short', name: 'recorD_STATUS_NAME', sortField: 'recorD_STATUS_NAME', width: '150px', align: TextAlign.Center },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS', width: '150px', align: TextAlign.Center },
            ],
            config: {
                indexing: true,
                checkbox: false
            }
        }
        // this.onChangeBranch({
        //     brancH_ID: this.appSession.user.subbrId
        // } as any);
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        this.appToolbar.setRole('Employee', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.initCombobox();
        this.cdr.detectChanges();
    }

    exportToExcel() {
        let reportInfo = new ReportInfo();
        reportInfo.typeExport = ReportTypeConsts.Excel;

        let reportFilter = { ...this.filterInputSearch };

        reportFilter.maxResultCount = -1;

        reportInfo.parameters = this.GetParamsFromFilter(reportFilter)

        reportInfo.values = this.GetParamsFromFilter({
            A1: this.l('CompanyReportHeader')
        });

        reportInfo.pathName = "/COMMON/BC_NHANVIEN.xlsx";
        reportInfo.storeName = "CM_EMPLOYEE_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }



    search(): void {
        this.showLoading();
        this.setSortingForFilterModel(this.filterInputSearch);
        this._employeeService.cM_EMPLOYEE_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount)
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/employee-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_EMPLOYEE_ENTITY): void {
        this.navigatePassParam('/app/main/employee-edit', { id: item.emP_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: CM_EMPLOYEE_ENTITY): void {
        if (item.autH_STATUS == 'A') {
            this.showErrorMessage('Không thể xóa nhân viên có trạng thái duyệt là "Đã duyệt"')
            return;
        }
        this.message.confirm(
            this.l('DeleteWarningMessage', item.emP_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this._employeeService.cM_EMPLOYEE_Del(item.emP_ID)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                                this.filterInputSearch.totalCount = 0;
                                this.reloadPage();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: CM_EMPLOYEE_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onViewDetail(item: CM_EMPLOYEE_ENTITY): void {
        this.navigatePassParam('/app/main/employee-view', { id: item.emP_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {
        throw new Error('Method not implemented.');
    }

    onReject(item: CM_EMPLOYEE_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSendApp(item: CM_EMPLOYEE_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onChangeBranch(branch: CM_BRANCH_ENTITY) {
        // if (!branch) {
        //     branch = { brancH_ID: this.appSession.user.subbrId } as any
        // }
        this.filterInput.deP_ID = undefined;
        this.filterInput.deP_NAME = undefined;
        let filterCombobox = this.getFillterForCombobox();
        filterCombobox.brancH_ID = branch.brancH_ID;
        this._departmentService.cM_DEPARTMENT_Search(filterCombobox).subscribe(response => {
            this.departments = response.items;
        })
    }

    onResetSearch(): void {
        this.filterInput = new CM_EMPLOYEE_ENTITY();
        this.filterInput.level = 'UNIT';
        // this.filterInput.brancH_ID = this.appSession.user.subbrId;
        this.changePage(0);
    }
}
