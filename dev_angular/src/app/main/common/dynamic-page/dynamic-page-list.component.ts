import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { DYNAMIC_PAGE_ENTITY, DynamicPageServiceProxy, DynamicPrimeTableServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize, lastValueFrom } from "rxjs";

@Component({
    templateUrl: './dynamic-page-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DynamicPageListComponent extends ListComponentBase<DYNAMIC_PAGE_ENTITY> implements OnInit, AfterViewInit, IUiAction<DYNAMIC_PAGE_ENTITY> {
    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private dynamicPrimeTableService: DynamicPrimeTableServiceProxy,
        private dynamicPageService: DynamicPageServiceProxy
    ) {
        super(injector);
        this.initFilter();
        this.initCombobox();
    }

    filterInput: DYNAMIC_PAGE_ENTITY = new DYNAMIC_PAGE_ENTITY();

    initCombobox() {
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        this.appToolbar.setRole('DynamicPage', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.changeDetector.detectChanges();
    }

    async loadDefaultTable() {
        const response = await lastValueFrom(this.dynamicPrimeTableService.dYNAMIC_PRIME_TABLE_GetAllData('DynamicPageList'));
        this.options = this.handleGenerateOptions(response)[0];
    }

    ngOnInit() {
        this.loadDefaultTable();
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/dynamic-page-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: DYNAMIC_PAGE_ENTITY): void {
        this.navigatePassParam('/app/main/dynamic-page-edit', { id: item.pagE_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onViewDetail(item: DYNAMIC_PAGE_ENTITY): void {
        this.navigatePassParam('/app/main/dynamic-page-view', { id: item.pagE_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    search(): void {
        this.showTableLoading();
        this.setSortingForFilterModel(this.filterInputSearch);
        this.dynamicPageService.dYNAMIC_PAGE_Search(this.filterInputSearch)
            .pipe(finalize(() => {
                this.hideLoading();
            }))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
                this.appToolbar.setEnableForListPage();
            });
    }

    onDelete(item: DYNAMIC_PAGE_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.pagE_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.dynamicPageService.dYNAMIC_PAGE_Del(item.pagE_ID)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted')); 
                                this.reloadPage();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: DYNAMIC_PAGE_ENTITY, btnElmt?: any): void {
        throw new Error("Method not implemented.");
    }

    onSave(): void {
        throw new Error("Method not implemented.");
    }

    onResetSearch(): void {
        this.filterInput = new DYNAMIC_PAGE_ENTITY();
        this.changePage(0);
    }

    onReject(item: DYNAMIC_PAGE_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSendApp(item: DYNAMIC_PAGE_ENTITY): void {
        throw new Error("Method not implemented.");
    }

}