import { Component, Injector, Input, ViewChild, ViewEncapsulation } from "@angular/core";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, CM_DEPT_GROUP_ENTITY, DepartmentServiceProxy, DeptGroupServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";
import { PopupBaseComponent } from "../popup-base/popup-base.component-old";
import { PopupFrameComponent } from "../popup-frames/popup-frame.component-old";
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { RecordStatusConsts } from "../../utils/consts/RecordStatusConsts";

@Component({
    selector: "department-costcenter-modal",
    templateUrl: "./department-costcenter-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class DepartmentCostCenterModalComponent extends PopupBaseComponent<CM_DEPARTMENT_ENTITY> {
    @ViewChild('departmentCostCenterModal') departmentCostCenterModal: PopupFrameComponent;
    @Input() cgtyear: string;
    @Input() isHS_PAN_APP: boolean = false;
    @Input() HM_KEHOACH: string;

    @Input() lstBranchInput:CM_BRANCH_ENTITY[];

    branchs: CM_BRANCH_ENTITY[];
    planMonth: number;
    isShowError = false;
    deptGroups: CM_DEPT_GROUP_ENTITY[];
    editPageState: EditPageState;
    branchModal: CM_BRANCH_ENTITY[];
    constructor(injector: Injector,
        private branchService: BranchServiceProxy,
        private departmentService: DepartmentServiceProxy,
        private deptGroupService: DeptGroupServiceProxy) {
        super(injector);
        this.filterInput = new CM_DEPARTMENT_ENTITY();
    }
    onSelectbranchMaster(branch: CM_BRANCH_ENTITY) {
        // this.filterInput.biD_ID = bid.biD_ID;
        this.filterInput.brancH_ID = branch.brancH_ID;
        // this.onSearch();
    }
    initComboFromApi() {
        this.deptGroupService.cM_DEPT_GROUP_Search(this.getFillterForCombobox()).subscribe(response => {
            this.deptGroups = response.items;
        });

        if(this.lstBranchInput != null)
        {
            this.branchs = this.lstBranchInput;
        }
        else{
            this.branchService.cM_BRANCH_Search(this.getFillterForCombobox()).subscribe(result => {
                this.branchs = result.items;
                super.updateView();
            });
        }
        // this.branchService.cM_BRANCH_Search(this.getFillterForCombobox()).subscribe(result => {
        //     this.branchs = result.items;
        //     super.updateView();
        // });

    }
    onBranchSelectChange(v: CM_BRANCH_ENTITY){
        this.filterInput.brancH_ID = v.brancH_ID;
        this.filterInput.brancH_CODE = v.brancH_CODE;
        this.filterInput.brancH_NAME = v.brancH_NAME;
    }
    get disableInput(): boolean {
        return (
            this.editPageState == EditPageState.viewDetail
        );
    }
    async getResult(checkAll: boolean = false): Promise<any> {
        this.filterInputSearch = this.filterInput;
        this.setSortingForFilterModel(this.filterInputSearch);

        this.filterInputSearch.recorD_STATUS = RecordStatusConsts.Active;

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        var result = await this.departmentService.cM_DEPARTMENT_COSTCENTER_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading())).toPromise();

        if (checkAll) {
            this.selectedItems = result.items;
        }
        else {
            this.dataTable.records = result.items;
            this.dataTable.totalRecordsCount = result.totalCount;
            this.filterInputSearch.totalCount = result.totalCount;
        }
        return result;
    }
}
