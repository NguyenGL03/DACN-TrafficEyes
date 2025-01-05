import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from "@app/shared/core/utils/consts/RecordStatusConsts";
import { BranchServiceProxy, CM_BRANCH_ENTITY } from "@shared/service-proxies/service-proxies";
import { lastValueFrom } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    selector: "branch-modal",
    templateUrl: "./branch-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class BranchModalComponent extends PopupBaseComponent<CM_BRANCH_ENTITY> implements OnInit {

    @Input() title: string = this.l("BranchModalTitle");
    @Input() branchTitle: string = this.l('SearchBranchInfo')
    @Input() showColPotential: boolean = true
    @Input() showColAuthStatus: boolean = true
    @Input() isLoadAllInput: boolean = false;
    @Input() typeSearch: string = '';
    @Input() lstBranchInput: CM_BRANCH_ENTITY[] = null;

    options: PrimeTableOption<CM_BRANCH_ENTITY>;
    branchs: CM_BRANCH_ENTITY[] = [];

    constructor(
        injector: Injector,
        private branchService: BranchServiceProxy
    ) {
        super(injector);
        this.filterInput = new CM_BRANCH_ENTITY();
        this.filterInput.recorD_STATUS = RecordStatusConsts.Active;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;

    }

    ngOnInit(): void { 
        this.options = {
            columns: [
                { title: 'BranchCode', name: 'brancH_CODE', sortField: 'menU_ID', width: 'auto' },
                { title: 'BranchName', name: 'brancH_NAME', sortField: 'menU_NAME', width: 'auto' },
                { title: 'Address', name: 'addr', sortField: 'addr', width: 'auto' },
                { title: 'Tel', name: 'tel', sortField: 'tel', width: 'auto' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }

    runAfterViewed() {
        this.branchService.cM_BRANCH_Search(this.getFillterForCombobox())
            .subscribe(response => {
                this.branchs = response.items;
            });
    }


    async getResult(checkAll: boolean = false): Promise<any> {
        this.filterInputSearch = this.filterInput;
        this.setSortingForFilterModel(this.filterInputSearch);

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        var result = await lastValueFrom(this.branchService.cM_BRANCH_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading())));

        if (this.isLoadAllInput) {
            this.filterInput.isLoadAll = this.isLoadAllInput;
        }

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.setRecords(result.items, result.totalCount);
        }
    }

}
