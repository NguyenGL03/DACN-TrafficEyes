import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { DynamicPrimeTableServiceProxy, DYNAMIC_SCREEN_PRIME_TABLE, DYNAMIC_COLUMN_PRIME_TABLE,DYNAMIC_PRIME_TABLE_ENTITY, DYNAMIC_CONFIG_PRIME_TABLE } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { finalize } from "rxjs";
@Component({
    templateUrl: './dynamic-prime-table-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DynamicPrimeTableListComponent extends ListComponentBase<DYNAMIC_SCREEN_PRIME_TABLE> implements OnInit, AfterViewInit, IUiAction<DYNAMIC_SCREEN_PRIME_TABLE>
{
    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private _dynamicPrimeTable: DynamicPrimeTableServiceProxy,
    ) {
        super(injector);
        this.initFilter();
        this.initCombobox();
    }
    filterInput: DYNAMIC_SCREEN_PRIME_TABLE = new DYNAMIC_SCREEN_PRIME_TABLE();
    defaultTable : DYNAMIC_PRIME_TABLE_ENTITY[];

    initCombobox() {
    }
    ngAfterViewInit(): void {
        this.initDefault(); // action edit when select
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        this.appToolbar.setRole('DynamicPrimeTable', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.changeDetector.detectChanges();
    }
    async loadDefaultTable() {
        try {
            const response = await this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_GetAllData('primeTableSearch').toPromise();
            //console.log('Default Table Response:', response);
            this.defaultTable = response;
            console.log(response);
        } catch (error) {
            //console.error('Error loading default table:', error);
        }
    }
    async ngOnInit(): Promise<void> {
        await this.loadDefaultTable();
        if (this.defaultTable && this.defaultTable.length > 0) {
            this.options = {
                columns: this.defaultTable[0].columns.map((col: any) => ({
                    title: col.title,
                    name: col.name,
                    sortField: col.sortField,
                    width: col.width,
                    // inputType: InputType.Text
                })),
                config: {
                    indexing: this.defaultTable[0].config.indexing,
                    checkbox: this.defaultTable[0].config.checkbox,
                    isShowButtonAdd : this.defaultTable[0].config.isShowButtonAdd,
                    isShowButtonDelete : this.defaultTable[0].config.isShowButtonDelete,
                    isShowError: this.defaultTable[0].config.isShowError
                }
            };
        } else {
            //console.error('empty or undefined');
        }
    }
    
    onAdd(): void {
        this.navigatePassParam('/app/main/dynamic-prime-table-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }
    onUpdate(item: DYNAMIC_SCREEN_PRIME_TABLE): void {
        this.navigatePassParam('/app/main/dynamic-prime-table-edit', { id: item.tableName }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }
    onViewDetail(item: DYNAMIC_SCREEN_PRIME_TABLE): void {
        this.navigatePassParam('/app/main/dynamic-prime-table-view', { id: item.tableName }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }
   
    search(): void {
        this.showTableLoading();
        this.setSortingForFilterModel(this.filterInputSearch);
        this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_Search(this.filterInputSearch)
            .pipe(finalize(() => {
                this.hideLoading();
            }))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
                this.appToolbar.setEnableForListPage();
            });
    }
    onDelete(item: DYNAMIC_SCREEN_PRIME_TABLE): void {
        if(item.autH_STATUS == 'A')
        {
            this.showErrorMessage('Can not delete a prime table that has approved auth status.')
        }
        this.message.confirm(
            this.l('DeleteWarningMessage', item.tableName),
            this.l('AreYouSure'),
            (isConfirmed) =>{
                if(isConfirmed){
                    this.showLoading();
                    this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_Del(item.tableName)
                    .pipe(finalize(() =>{this.hideLoading();}))
                    .subscribe((response) =>{
                        if(response.result != '0')
                        {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else{
                            this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                            this.filterInputSearch.totalCount = 0;
                            this.reloadPage();
                        }
                    });
                }
            }
        );
    }
    onApprove(item: DYNAMIC_SCREEN_PRIME_TABLE, btnElmt?: any): void {
        throw new Error("Method not implemented.");
    }
    onSave(): void {
        throw new Error("Method not implemented.");
    }
    onResetSearch(): void {
        this.filterInput = new DYNAMIC_SCREEN_PRIME_TABLE();
        this.changePage(0);
    }
    onReject(item: DYNAMIC_SCREEN_PRIME_TABLE): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: DYNAMIC_SCREEN_PRIME_TABLE): void {
        throw new Error("Method not implemented.");
    }
    
}