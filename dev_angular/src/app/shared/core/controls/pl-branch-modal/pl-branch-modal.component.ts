import { Component, Injector, Input, ViewEncapsulation } from "@angular/core";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, DepartmentServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";
import { PopupBaseComponent } from "../popup-base/popup-base.component-old";

@Component({
    selector: "pl-branch-modal",
    templateUrl: "./pl-branch-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class PLanBranchModalComponent extends PopupBaseComponent<CM_BRANCH_ENTITY> {

    @Input() typeFilter: number = 0;
    @Input() isLoadAll: boolean = false;
    @Input() isRequestTransfer: boolean = false;
    // sonnq 15-3-24
    @Input() isAccpet: boolean = true;
    @Input() unitRegular: string = "";

    constructor(injector: Injector,
        private branchService: BranchServiceProxy,
        private departmentService: DepartmentServiceProxy
    ) {
        super(injector);
        this.filterInput = new CM_BRANCH_ENTITY();
        this.filterInput.top = 300;
        this.filterInput.brancH_LOGIN = this.appSession.user.subbrId;
        this.filterInput.autH_STATUS = 'A';
        this.filterInput.isLoadAll = true;
        this.filterInput.useR_LOGIN = this.appSession.user.userName;

        this.keyMember = 'brancH_ID';

    }

    initComboFromApi() {
        let filterCombobox = this.getFillterForCombobox();
        filterCombobox.isLoadAll = this.isLoadAll;
        this.branchService.cM_BRANCH_Search(filterCombobox).subscribe(response => {
            this.branchs = response.items;
            this.updateView();
        });

        this.onChangeBranch(undefined);
    }

    onChangeBranch(branch: CM_BRANCH_ENTITY) {
        if (!branch) {
            branch = { brancH_ID: this.appSession.user.subbrId } as any
        }
        this.filterInput.deP_ID = undefined;
        this.filterInput.deP_NAME = undefined;
        let filterCombobox = this.getFillterForCombobox();
        filterCombobox.brancH_ID = branch.brancH_ID;
        this.departmentService.cM_DEPARTMENT_Search(filterCombobox).subscribe(response => {
            this.departments = response.items;
            this.updateView();
        })
    }

    @Input() branchTitle: string = this.l('SearchBranchInfo');
    @Input() showColPotential: boolean = true;
    @Input() showColAuthStatus: boolean = true;

    departments: CM_DEPARTMENT_ENTITY[];
    branchs: CM_BRANCH_ENTITY[];



    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInputSearch);
        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        var result = this.isRequestTransfer
            ? await this.branchService.cM_BRANCH_DEP_Search_v2(this.filterInputSearch)
                .pipe(finalize(() => this.hideTableLoading())).toPromise()
            : await this.branchService.cM_BRANCH_DEP_Search(this.filterInputSearch)
                .pipe(finalize(() => this.hideTableLoading())).toPromise();

        if (checkAll) {
            var item = "";
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
