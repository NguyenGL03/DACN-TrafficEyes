import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from "@app/shared/core/utils/consts/RecordStatusConsts";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DIVISION_ENTITY, DivisionServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";

@Component({
    selector: "division-modal",
    templateUrl: "./division-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class DivisionModalComponent extends PopupBaseComponent<CM_DIVISION_ENTITY> implements OnInit {

    constructor(injector: Injector,
        private divisionService: DivisionServiceProxy,
        private branchService: BranchServiceProxy) {
        super(injector);
        this.filterInput = new CM_DIVISION_ENTITY();
        this.keyMember = 'deP_ID';
        this.filterInput.recorD_STATUS = '1';
        this.initCombobox();
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'diV_CODE', name: 'diV_CODE', sortField: 'diV_CODE', width: '250px' },
                { title: 'diV_NAME', name: 'diV_NAME', sortField: 'diV_NAME', width: '250px' },
                { title: 'brancH_NAME', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '250px' },
                { title: 'addr', name: 'addr', sortField: 'addr', width: '250px' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple
            }
        }
    }


    branchs: CM_BRANCH_ENTITY[];
    @Input() labelTitle = this.l('Search') + ' ' + this.l("Division").toLowerCase()
    @Input() showColDivCode: boolean = false
    @Input() showColDivName: boolean = true
    @Input() showColBranchCode: boolean = true
    @Input() showColAddress: boolean = true
    options: PrimeTableOption<CM_DIVISION_ENTITY>;

    initCombobox() {
        this.branchService.cM_BRANCH_Search(this.getFillterForCombobox()).subscribe(response => {
            this.branchs = response.items;
        });
    }

    async getResult(checkAll: boolean = false): Promise<any> {

        this.setSortingForFilterModel(this.filterInputSearch);

        this.filterInputSearch.autH_STATUS = AuthStatusConsts.Approve;
        this.filterInputSearch.recorD_STATUS = RecordStatusConsts.Active;

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        var result = await this.divisionService.cM_DIVISION_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading())).toPromise();

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.setRecords(result.items, result.totalCount);
            this.filterInputSearch.totalCount = result.totalCount;
        }

        return result;
    }
}