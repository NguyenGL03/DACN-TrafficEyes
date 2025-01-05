import { Component, Injector, Input, ViewEncapsulation } from "@angular/core";
import { BranchServiceProxy, CM_BRANCH_ENTITY } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";
import { PopupBaseComponent } from "../popup-base/popup-base.component-old";

@Component({
    selector: "branch-dep-modal",
    templateUrl: "./branch-dep-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class BranchDepModalComponent extends PopupBaseComponent<CM_BRANCH_ENTITY> {


    @Input() isLoadAll: boolean = false;

    constructor(injector: Injector,
        private branchService: BranchServiceProxy) {
        super(injector);
        this.filterInput = new CM_BRANCH_ENTITY();
        this.filterInput.top = 300;
        this.filterInput.brancH_LOGIN = this.appSession.user.subbrId;
        this.filterInput.autH_STATUS = 'A';
        this.filterInput.isLoadAll = false;

        this.keyMember = 'brancH_ID';

    }

    initComboFromApi() {

        let input = this.getFillterForCombobox();

        input.isLoadAll = this.isLoadAll;


        this.branchService.cM_BRANCH_Search(input).subscribe(result => {
            this.lstBranch = result.items;
            let HO = new CM_BRANCH_ENTITY();
            HO.brancH_ID = 'DV0001';
            HO.brancH_NAME = 'Hội Sở'
            HO.brancH_CODE = '069';
            this.lstBranch.unshift(HO);
            this.updateView();
        });
    }


    @Input() branchTitle: string = this.l('SearchBranchInfo') // branch use title: this.l('Search') + ' ' + this.l('BranchNameUse').toLowerCase()
    @Input() showColPotential: boolean = true
    @Input() showColAuthStatus: boolean = true


    lstBranch: CM_BRANCH_ENTITY[];
    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInputSearch);

        // this.filterInputSearch.autH_STATUS = AuthStatusConsts.Approve;
        // this.filterInputSearch.recorD_STATUS = RecordStatusConsts.Active;

        // if (!this.filterInputSearch.brancH_LOGIN) {
        //     this.filterInputSearch.brancH_LOGIN = this.appSession.user.subbrId;
        // }

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        var result = await this.branchService.cM_BRANCH_DEP_Search(this.filterInputSearch)
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
