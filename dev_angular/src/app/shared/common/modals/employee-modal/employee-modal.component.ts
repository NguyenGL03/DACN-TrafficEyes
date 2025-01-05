import { Component, Injector, Input, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, CM_EMPLOYEE_ENTITY, CM_REGION_ENTITY, DepartmentServiceProxy, EmployeeServiceProxy, RegionServiceProxy } from "@shared/service-proxies/service-proxies";
import { lastValueFrom } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    selector: "employee-modal",
    templateUrl: "./employee-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class EmployeeModalComponent extends PopupBaseComponent<CM_EMPLOYEE_ENTITY> {
    constructor(injector: Injector,
        private employeeService: EmployeeServiceProxy,
        private branchService: BranchServiceProxy,
        private regionService: RegionServiceProxy,
        private departmentService: DepartmentServiceProxy
    ) {
        super(injector);
        this.title = this.l('EmployeeModalTitle')
        this.filterInput = new CM_EMPLOYEE_ENTITY();
        this.keyMember = 'emP_ID';
        this.filterInput.level = 'UNIT';
        this.filterInput.top = 300;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
    }

    @Input() showEmployeeCode: boolean = true
    @Input() showColKhuVuc: boolean = true
    @Input() showColChiNhanh: boolean = true
    @Input() showColPGD: boolean = true
    @Input() showColEmployeeName: boolean = true
    @Input() showColEmployeeCode: boolean = true
    @Input() storedName: string = "CM_EMPLOYEE_SEARCH";
    @Input() dep_Id: any;
    @Input() branchId: string;
    @Input() disableCombobox: boolean = true;
    @Input() disableBranch: boolean = false;
    @Input() disableDep: boolean = false;


    branchs: CM_BRANCH_ENTITY[];
    regionList: CM_REGION_ENTITY[];
    departmentList: CM_DEPARTMENT_ENTITY[];
    regioN_ID: string;


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

    runAfterViewed() {
        this.getRegionList();

        this.getBranchList();

        this.getDepartmentList();
    }

    async getResult(checkAll: boolean = false): Promise<any> {

        this.setSortingForFilterModel(this.filterInput);

        if (!this.filterInput.brancH_ID) {
            this.filterInput.brancH_ID = this.appSession.user.subbrId;
        }

        if (checkAll) {
            this.filterInput.maxResultCount = -1;
        }
        this.filterInput.storeD_NAME = this.storedName;
        const result = await lastValueFrom(this.employeeService.cM_EMPLOYEE_MODAL(this.filterInput)
            .pipe(finalize(() => this.hideTableLoading())));

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.setRecords(result.items, result.totalCount);
        }

        return result;
    }

    getRegionList(): void {
        this.regionService.cM_REGION_Search(this.getFillterForCombobox()).subscribe(res => {
            this.regionList = res.items;
        })
    }

    getBranchList(): void {
        this.branchService.cM_BRANCH_Search(this.getFillterForCombobox()).subscribe(response => {
            this.branchs = response.items;
        });
    }

    getDepartmentList(): void {
        this.departmentService.cM_DEPARTMENT_Search(this.getFillterForCombobox()).subscribe(res => {
            this.departmentList = res.items;
        })
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
}
