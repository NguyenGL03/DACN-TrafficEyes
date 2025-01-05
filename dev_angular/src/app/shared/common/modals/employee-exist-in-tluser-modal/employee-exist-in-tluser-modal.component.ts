import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, CM_EMPLOYEE_ENTITY, CM_REGION_ENTITY, DepartmentServiceProxy, EmployeeServiceProxy, RegionServiceProxy } from "@shared/service-proxies/service-proxies";
import { lastValueFrom } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    selector: "employee-exist-in-tluser-modal",
    templateUrl: "./employee-exist-in-tluser-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class EmployeeExistInTluserModalComponent extends PopupBaseComponent<CM_EMPLOYEE_ENTITY> implements OnInit {
    constructor(injector: Injector,
        private employeeService: EmployeeServiceProxy,
        private branchService: BranchServiceProxy,
        private regionService: RegionServiceProxy,
        private departmentService: DepartmentServiceProxy
    ) {
        super(injector);
        this.filterInput = new CM_EMPLOYEE_ENTITY();
        this.keyMember = 'emP_ID';
        this.filterInput.level = 'UNIT';
        this.filterInput.top = 300;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
        console.log(this);
        this.initCombobox();
    }
    @Input() title: string = this.l('EmployeeModalTitle')

    @Input() showEmployeeCode: boolean = true

    @Input() showColKhuVuc: boolean = true
    @Input() showColChiNhanh: boolean = true
    @Input() showColPGD: boolean = true
    @Input() showColEmployeeName: boolean = true
    @Input() showColEmployeeCode: boolean = true
    @Input() dep_Id: any;
    @Input() branchId: string;
    @Input() disableCombobox: boolean = true;
    @Input() disableBranch: boolean = false;
    @Input() disableDep: boolean = false;


    branchs: CM_BRANCH_ENTITY[];
    regionList: CM_REGION_ENTITY[];
    departmentList: CM_DEPARTMENT_ENTITY[];
    regioN_ID: string;
    options: PrimeTableOption<CM_EMPLOYEE_ENTITY>;

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'emP_CODE', name: 'emP_CODE', sortField: 'emP_CODE', width: '250px' },
                { title: 'emP_NAME', name: 'emP_NAME', sortField: 'emP_NAME', width: '250px' },
                { title: 'brancH_CODE', name: 'brancH_CODE', sortField: 'brancH_CODE', width: '250px' },
                { title: 'brancH_NAME', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '250px' },
                { title: 'deP_CODE', name: 'deP_CODE', sortField: 'deP_CODE', width: '250px' },
                { title: 'deP_NAME', name: 'deP_NAME', sortField: 'deP_NAME', width: '250px' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }

    initCombobox() {
        this.getRegionList();

        this.branchService.cM_BRANCH_Search(this.getFillterForCombobox()).subscribe(response => {
            this.branchs = response.items;
        });

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
        var result = await lastValueFrom(this.employeeService.cM_EMPLOYEE_EXIST_IN_TLUSER_MODAL(this.filterInput)
            .pipe(finalize(() => this.hideTableLoading())));

        if (checkAll) {
            this.selectedItems = result.items;
        }
        else {
            this.setRecords(result.items, result.totalCount);
        }

        return result;
    }

    getRegionList(): void {
        var filter = this.getFillterForCombobox();
        this.regionService.cM_REGION_Search(filter).subscribe(res => {
            this.regionList = res.items;
        })
    }

    getDepartmentList(): void {
        let filterCombobox = this.getFillterForCombobox();
        this.departmentService.cM_DEPARTMENT_Search(filterCombobox).subscribe(res => {
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
