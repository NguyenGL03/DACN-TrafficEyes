import { Component, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { BranchServiceProxy, CM_BRANCH_ENTITY } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";
import { PrimeTableComponent } from "../primeng-table/prime-table/prime-table.component";
import { PrimeTableOption } from "../primeng-table/prime-table/primte-table.interface";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { PopupFrameComponent } from "@app/shared/common/modals/popup-frames/popup-frame.component";
import { PopupBaseComponent } from "../popup-base/popup-base.component";

@Component({
    selector: "inventory-branch-modal",
    templateUrl: "./inventory-branch-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class BranchInventoryModalComponent extends PopupBaseComponent<CM_BRANCH_ENTITY> {
    // @ViewChild('coreTable') coreTable: PrimeTableComponent<CM_BRANCH_ENTITY>;
    coreTableOptions: PrimeTableOption<CM_BRANCH_ENTITY>;
    filterInput = new CM_BRANCH_ENTITY();
    @Output() onSelect: EventEmitter<any> = new EventEmitter<any>();

    constructor(injector: Injector,
        private branchService: BranchServiceProxy) {
        super(injector);
        this.filterInput = new CM_BRANCH_ENTITY();
        this.filterInput.top = 300;
        this.filterInput.brancH_LOGIN = this.appSession.user.subbrId;
        this.filterInput['branch'] = undefined;
        this.filterInput.autH_STATUS = 'A';
        this.filterInput.isLoadAll = true;

        // this.keyMember = 'brancH_ID';

    }
    ngOnInit() {
        this.coreTableOptions = {
            columns: [
                { title: 'BranchCode', name: 'BRANCH_CODE', sortField: 'BRANCH_CODE', width: '180px' },
                { title: 'BranchName', name: 'BRANCH_NAME', sortField: 'BRANCH_NAME', width: '180px' },
                { title: 'Address', name: 'ADDR', sortField: 'ADDR', width: '180px', },
                { title: 'Tel', name: 'TEL', sortField: 'TEL', width: '180px', },
            ],
            config: {
                indexing: true,
                checkbox: false
            }
        }
    }
    initComboFromApi() {
        let branch = this.getFillterForCombobox()
        branch.brancH_ID = this.appSession.user.subbrId;
        this.branchService.cM_BRANCH_Search(branch).subscribe(result => {
            this.lstBranch = result.items;
            this.filterInput.brancH_ID = this.appSession.user.subbrId;
        });
    }


    @Input() branchTitle: string = this.l('SearchBranchInfo') // branch use title: this.l('Search') + ' ' + this.l('BranchNameUse').toLowerCase()
    @Input() showColPotential: boolean = true
    @Input() showColAuthStatus: boolean = true
    @Input() isLoadAllInput: boolean = false; //Nếu modal search hết data đơn vị thì truyền true
    @Input() typeSearch: string = ''; // nếu đăng nhập đơn vị hs thì tìm tất cả

    lstBranch: CM_BRANCH_ENTITY[];
    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInputSearch);


        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }
        this.filterInputSearch.brancH_ID = this.appSession.user.subbrId;
        var result = await this.branchService.cM_BRANCH_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading())).toPromise();

        if (this.isLoadAllInput) {
            this.filterInputSearch.isLoadAll = this.isLoadAllInput;
        }

        if (checkAll) {
            var item = "";
            this.selectedItems = result.items;
        }
        else {
            this.dataTable.setAllRecords(result.items)
            this.filterInputSearch.totalCount = result.totalCount;
        }

        return result;
    }

    onSelectBranch(event: CM_BRANCH_ENTITY): void {
        if (event && event.brancH_ID) {
            this.filterInput.brancH_LOGIN = event.brancH_ID;
            this.filterInput.brancH_ID = event.brancH_ID;
        } else if (!event || (event && !event.brancH_ID)) {
            this.filterInput.brancH_ID = undefined;
            this.filterInput.brancH_LOGIN = this.appSession.user.subbrId;
        }
        //
    }
    onUpdate(item: any): void {
    }

    onDelete(item: any): void {
    }
    onViewDetail(item: any): void {
    }
    accept() {
        if (!this.multiple) {
            this.onSelect.emit(this.currentItem);
            this.close();
        }
        else {
            this.onSelect.emit(this.selectedItems);
            this.close();
        }
    }
}
