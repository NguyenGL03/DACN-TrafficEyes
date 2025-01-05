import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { ColumnType, PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from "@app/shared/core/utils/consts/RecordStatusConsts";
import { ASS_AMORT_STATUS_ENTITY, ASS_GROUP_ENTITY, ASS_MASTER_ENTITY, ASS_STATUS_ENTITY, ASS_TYPE_ENTITY, AssAmortStatusServiceProxy, AssGroupServiceProxy, AssMasterServiceProxy, AssStatusServiceProxy, AssTypeServiceProxy, BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, DepartmentServiceProxy } from "@shared/service-proxies/service-proxies";
import { lastValueFrom } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    selector: "asset-modal",
    templateUrl: "./asset-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class AssetModalComponent extends PopupBaseComponent<ASS_MASTER_ENTITY> implements OnInit {

    @Input() location: string
    @Input() storeName: string = 'ASS_MASTER_SEARCH';
    @Input() hideColumns: string = 'do not thing';

    filterInput: ASS_MASTER_ENTITY;
    options: PrimeTableOption<ASS_MASTER_ENTITY>;
    assTypes: ASS_TYPE_ENTITY[] = [];
    assGroups: ASS_GROUP_ENTITY[] = [];
    assStatuses: ASS_STATUS_ENTITY[] = [];
    assAmortStatuses: ASS_AMORT_STATUS_ENTITY[] = [];
    branches: CM_BRANCH_ENTITY[] = [];
    departments: CM_DEPARTMENT_ENTITY[] = [];

    constructor(
        injector: Injector,
        private assMasterService: AssMasterServiceProxy,
        private assTypeService: AssTypeServiceProxy,
        private assGroupServie: AssGroupServiceProxy,
        private assStatusService: AssStatusServiceProxy,
        private assAmortStatusService: AssAmortStatusServiceProxy,
        private branchService: BranchServiceProxy,
        private departmentService: DepartmentServiceProxy
    ) {
        super(injector);
        this.filterInput = new ASS_MASTER_ENTITY();
        this.filterInput.recorD_STATUS = RecordStatusConsts.Active;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
        this.title = this.l("AssetModalTitle")
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'AssetCode', name: 'asseT_CODE', sortField: 'asseT_CODE', width: '250px' },
                { title: 'AssetName', name: 'asseT_NAME', sortField: 'asseT_NAME', width: '250px' },
                { title: 'AssGroup', name: 'typE_NAME', sortField: 'typE_NAME', width: '250px' },
                { title: 'AssType', name: 'grouP_NAME', sortField: 'grouP_NAME', width: '250px' },
                { title: 'AssetSerialNo', name: 'asseT_SERIAL_NO', sortField: 'asseT_SERIAL_NO', width: '250px' },
                { title: 'BuyDate', name: 'buY_DATE', sortField: 'buY_DATE', width: '250px', type: ColumnType.Date },
                { title: 'BranchCreateName', name: 'brancH_CREATE_NAME', sortField: 'brancH_CREATE_NAME', width: '250px' },
                { title: 'BranchName', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '250px' },
                { title: 'DepName', name: 'deP_NAME', sortField: 'deP_NAME', width: '250px' },
                { title: 'UseDate', name: 'usE_DATE', sortField: 'usE_DATE', width: '250px', type: ColumnType.Date },
                { title: 'OriginalPrice', name: 'buY_PRICE', sortField: 'buY_PRICE', width: '250px', type: ColumnType.Curency },
                { title: 'RemainAmortizedAmt', name: 'remaiN_AMORTIZED_AMT', sortField: 'remaiN_AMORTIZED_AMT', width: '250px' },
                { title: 'AssStatusName', name: 'asS_STATUS_NAME', sortField: 'asS_STATUS_NAME', width: '250px' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }

    runAfterViewed() {
        this.assTypeService.aSS_TYPE_Search(this.getFillterForCombobox()).
            subscribe(response => {
                this.assTypes = response.items;
            });
        this.assGroupServie.aSS_GROUP_Search(this.getFillterForCombobox()).
            subscribe(response => {
                this.assGroups = response.items;
            });
        this.assStatusService.aSS_STATUS_Lst().subscribe(response => {
            this.assStatuses = response;
        });
        this.assAmortStatusService.aSS_AMORT_STATUS_Lst().subscribe(response => {
            this.assAmortStatuses = response;
        });
        this.branchService.cM_BRANCH_Search(this.getFillterForCombobox())
            .subscribe(response => {
                this.branches = response.items;
            });
        // this.departmentService.cM_DEPARTMENT_Search(this.getFillterForCombobox())
        //     .subscribe(response => {
        //         this.departments = response.items;
        //     });
    }

    async getResult(checkAll: boolean = false): Promise<any> {
        this.filterInputSearch = this.filterInput;
        if (this.location) {
            this.filterInputSearch.location = this.location;
        }

        this.setSortingForFilterModel(this.filterInputSearch);
        if (this.location) {
            this.filterInputSearch.location = this.location;
        }
        if (checkAll || this.filterInputSearch.top) {
            this.filterInputSearch.maxResultCount = -1;
            this.dataTable.pagingClient = true
        }

        this.filterInputSearch.storE_NAME = this.storeName;
        let result = await lastValueFrom(this.assMasterService.aSS_MASTER_Modal(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading())));

        if (checkAll) {
            this.selectedItems = result['items'];
        } else {
            if (this.dataTable.pagingClient) {
                this.dataTable.setAllRecords(result.items)
            } else {
                this.dataTable.setRecords(result.items, result.totalCount);
            }
        }
        return result;
    }

    onChangeBranch(branch: CM_BRANCH_ENTITY) {
        if (!branch) {
            branch = { brancH_ID: this.appSession.user.subbrId } as any
        }
        this.filterInput.depT_ID = undefined;
        this.filterInput.deP_NAME = undefined;
        let filterCombobox = this.getFillterForCombobox();
        filterCombobox.brancH_ID = branch.brancH_ID;
        this.departmentService.cM_DEPARTMENT_Search(filterCombobox).subscribe(response => {
            this.departments = response.items;
        })
    }

    getAssAmortStatusList(): void {
        this.assAmortStatusService.aSS_AMORT_STATUS_Lst().subscribe(response => {
            let amortStatusCustom: string = this.filterInput['amorT_STATUS_CUSTOM'];
            if (amortStatusCustom && amortStatusCustom.length !== 0) {
                this.assAmortStatuses = response.filter(x => amortStatusCustom.includes(x.statuS_CODE))
            } else {
                this.assAmortStatuses = response;
            }
        });
    }
}
