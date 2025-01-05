import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from "@app/shared/core/utils/consts/RecordStatusConsts";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, CM_DEPT_GROUP_ENTITY, DepartmentServiceProxy, DeptGroupServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs";


@Component({
    selector: "dep-modal",
    templateUrl: "./department-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class DepartmentModalComponent extends PopupBaseComponent<CM_DEPARTMENT_ENTITY> implements OnInit {
    @Input() branch_id: string;
    @Input() hiddenInputSearchBranch = true;
    @Input() type: any;
    @Input() father_id: any; // Trung tâm
    @Input() khoi_id: any; // Khối
    @Input() hiddenInputSearchGroup = true; // Ẩn Combobox Nhóm phòng ban
    @Input() isLoadAllInput: boolean = false; // Nếu modal search hết data phòng ban thì truyền true
    @Input() title: string = this.l('DepartmentModalTitle');
    deptGroups: CM_DEPT_GROUP_ENTITY[]; 
    options: PrimeTableOption<CM_DEPARTMENT_ENTITY>;
    filterInput: CM_DEPARTMENT_ENTITY

    constructor(
        injector: Injector,
        private departmentService: DepartmentServiceProxy,
        private deptGroupService: DeptGroupServiceProxy,
        private branchService: BranchServiceProxy
    ) {
        super(injector);
        this.filterInput = new CM_DEPARTMENT_ENTITY();
        this.filterInput.brancH_ID = this.branch_id;
        this.keyMember = 'deP_ID'; 
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'DepCode', name: 'deP_CODE', sortField: 'deP_CODE', width: '200px' },
                { title: 'DepName', name: 'deP_NAME', sortField: 'deP_NAME', width: '200px' },
                { title: 'BranchCode', name: 'brancH_CODE', sortField: 'brancH_CODE', width: '200px' },
                { title: 'BranchName', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '200px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '200px' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }

    runAfterViewed() {
        this.deptGroupService.cM_DEPT_GROUP_Search(this.getFillterForCombobox()).subscribe(response => {
            this.deptGroups = response.items;
        }); 
    }

    async getResult(checkAll: boolean = false): Promise<any> {

        this.filterInput.brancH_ID = this.branch_id;
        this.filterInput.type = this.type;
        this.filterInput.fatheR_ID = this.father_id;
        this.filterInput.khoI_ID = this.khoi_id;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
        this.filterInput.recorD_STATUS = RecordStatusConsts.Active;

        this.setSortingForFilterModel(this.filterInput);

        this.filterInputSearch = this.filterInput;

        if (checkAll) this.filterInputSearch.maxResultCount = -1;


        var result = await this.departmentService.cM_DEPARTMENT_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading())).toPromise();

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.setRecords(result.items, result.totalCount);
        }

        return result;
    }

    clearData() {
        // this.dataTable.records = [];
        // if (!this.isNull(this.dataTable.)) this.dataTable.totalRecordsCount = 0;
        // if (!this.isNull(this.filterInputSearch)) this.filterInputSearch.totalCount = 0;
        // this.updateView();
        this.setRecords([], 0);
    }
}
