import { ChangeDetectorRef, Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption, TextAlign } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { ASS_GROUP_ENTITY, ASS_TYPE_ENTITY, AssGroupServiceProxy, AssTypeServiceProxy } from "@shared/service-proxies/service-proxies";
import { lastValueFrom } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    selector: "ass-group-modal-pyc",
    templateUrl: "./ass-group-modal-pyc.component.html",
    encapsulation: ViewEncapsulation.None
})
export class AssGroupModalPycComponent extends PopupBaseComponent<ASS_GROUP_ENTITY> implements OnInit {

    @Input() title: string = this.l("AssetModalTitle");
    @Input() location: string
    @Input() storeName: string = 'ASS_MASTER_SEARCH';
    @Input() multiple: any

    filterInput: ASS_GROUP_ENTITY;
    options: PrimeTableOption<ASS_GROUP_ENTITY>;

    assTypes: ASS_TYPE_ENTITY[];
    @Input() labelTitle: string = this.l('AssGroupTitleModal');
    @Input() lvl: number;
    @Input() showColGroupCode: boolean = true;
    @Input() showColGroupName: boolean = true;
    @Input() showColTypeName: boolean = true;
    @Input() showColParentCode: boolean = true;
    @Input() showColAuthStatus: boolean = true;
    @Input() disableAssType: boolean = false;

    constructor(
        injector: Injector,
        private assTypeService: AssTypeServiceProxy,
        private assGroupService: AssGroupServiceProxy,
    ) {
        super(injector);
        this.filterInput = new ASS_GROUP_ENTITY();
        this.filterInputSearch = this.filterInput;
        this.filterInputSearch.autH_STATUS = AuthStatusConsts.Approve;
        this.filterInputSearch.top = 200;
        this.keyMember = 'grouP_ID';
        this.initCombobox();
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'AssetGroupCode', name: 'grouP_CODE', sortField: 'grouP_CODE', width: '150px' },
                { title: 'AssGroupName', name: 'grouP_NAME', sortField: 'grouP_NAME', width: '150px' },
                { title: 'AssTypeName', name: 'typE_NAME', sortField: 'typE_NAME', width: '150px' },
                { title: 'Unit', name: 'uniT_NAME', sortField: 'uniT_NAME', width: '150px' },
                { title: 'Image', name: 'image', width: '80px', align: TextAlign.Center },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS_NAME', width: '150px' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }

    initCombobox() {
        this.assTypeService.aSS_TYPE_Search(this.getFillterForCombobox()).subscribe(response => {
            this.assTypes = response.items;
        });
    }

    async getResult(checkAll: boolean = false): Promise<any> {
        this.filterInputSearch = this.filterInput;
        this.setSortingForFilterModel(this.filterInputSearch);
        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        this.filterInputSearch.grouP_LEVEL = this.lvl;
        var result = await lastValueFrom(this.assGroupService.aSS_GROUP_Search(this.filterInputSearch)
            .pipe(finalize(() => { this.hideTableLoading(); this.cdr.detectChanges() })));

        if (checkAll) {
            this.selectedItems = result['items'];
        } else {
            this.setRecords(result.items, result.totalCount);
        }
        return result;
    }
}
