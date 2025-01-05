import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, CM_EMPLOYEE_ENTITY, CM_REGION_ENTITY, DepartmentServiceProxy, EmployeeServiceProxy, RegionServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";

@Component({
    selector: "employee-mapping-modal",
    templateUrl: "./employee-mapping-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class EmployeeMappingModalComponent extends PopupBaseComponent<CM_EMPLOYEE_ENTITY> implements OnInit {
    constructor(injector: Injector,
        private employeeService: EmployeeServiceProxy,
        private branchService: BranchServiceProxy,
        private regionService: RegionServiceProxy,
        private departmentService: DepartmentServiceProxy
    ) {
        super(injector);
        this.title = this.l('EmployeeModalTitle');
        this.filterInput = new CM_EMPLOYEE_ENTITY();
        this.keyMember = 'emP_ID';
        this.filterInput.level = 'UNIT';
        // this.filterInput.top = 300;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
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
            ],
            config: {
                indexing: true,
                checkbox: this.multiple
            }
        }
    }

    @Input() showEmployeeCode: boolean = true
    @Input() showColKhuVuc: boolean = true
    @Input() showColChiNhanh: boolean = true
    @Input() showColPGD: boolean = true
    @Input() showColEmployeeName: boolean = true
    @Input() showColEmployeeCode: boolean = true

    @Input() dep_Id: string;
    @Input() branchId: string;

    @Input() disableCombobox: boolean = false;

    branchs: CM_BRANCH_ENTITY[];
    regionList: CM_REGION_ENTITY[];
    departmentList: CM_DEPARTMENT_ENTITY[];
    regioN_ID: string;

    runAfterViewed() {
        this.getRegionList();
        this.getBranchList();
        this.getDepartmentList();
    }

    async getResult(checkAll: boolean = false): Promise<any> {
        this.filterInputSearch = this.filterInput;
        this.setSortingForFilterModel(this.filterInputSearch);

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        var result = await this.employeeService.cM_EMPLOYEE_Search_NotMapping(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading())).toPromise();

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.setRecords(result.items, result.totalCount);
        }

        return result;
    }

    getRegionList(): void {
        this.showLoading();
        var filter = this.getFillterForCombobox();
        this.regionService.cM_REGION_Search(filter)
            .pipe(finalize(() => this.hideLoading()))
            .subscribe(res => {
                this.regionList = res.items;
            })
    }

    getDepartmentList(): void {
        let filterCombobox = this.getFillterForCombobox();
        this.departmentService.cM_DEPARTMENT_Search(filterCombobox)
            .pipe(finalize(() => this.hideLoading()))
            .subscribe(res => {
                this.departmentList = res.items;
            })
    }

    getBranchList(): void {
        this.branchService.cM_BRANCH_Search(this.getFillterForCombobox())
            .pipe(finalize(() => this.hideLoading()))
            .subscribe(res => {
                this.branchs = res.items;
            });
    }

    onChangeBranch(branch: CM_BRANCH_ENTITY) {
        if (!branch) {
            branch = { brancH_ID: this.appSession.user.subbrId } as any
        }
        let filterCombobox = this.getFillterForCombobox();
        filterCombobox.brancH_ID = branch.brancH_ID;
        this.departmentService.cM_DEPARTMENT_Search(filterCombobox).subscribe(response => {
            this.departmentList = response.items;
        })
    }

    clearData() {
        this.setRecords([], 0);
    }
}
