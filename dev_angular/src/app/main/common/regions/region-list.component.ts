import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { LazyDropdownResponse } from "@app/shared/core/controls/dropdown-lazy-control/dropdown-lazy-control.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { CM_REGION_ENTITY, CM_SUPPLIER_ENTITY, RegionServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs";

@Component({
    templateUrl: './region-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class RegionListComponent extends ListComponentBase<CM_REGION_ENTITY> implements IUiAction<CM_REGION_ENTITY>, OnInit, AfterViewInit {
    filterInput: CM_REGION_ENTITY = new CM_REGION_ENTITY();
    options: PrimeTableOption<CM_REGION_ENTITY>;
    editPageState: EditPageState;
    isFirstCall: boolean = true;

    constructor(injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private regionService: RegionServiceProxy) {
        super(injector);
        this.initFilter();
    }

    onReject(item: CM_REGION_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: CM_REGION_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    initDefaultFilter() {
        this.filterInput.top = 200;
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'RegionCode', name: 'regioN_CODE', sortField: 'regioN_CODE', width: '180px'},
                { title: 'RegionName', name: 'regioN_NAME', sortField: 'regioN_NAME', width: '180px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '180px' },
                { title: 'IsActive', name: 'recorD_STATUS_NAME', sortField: 'recorD_STATUS_NAME', width: '180px' },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS_NAME', width: '180px' },
            ],
            config: {
                indexing: true,
                checkbox: false
            }
        }
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setRole('Region', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }

    // exportToExcel() {
    //     let reportInfo = new ReportInfo();
    //     reportInfo.typeExport = ReportTypeConsts.Excel;

    //     let reportFilter = { ...this.filterInputSearch };

    //     reportFilter.maxResultCount = -1;

    //     reportInfo.parameters = this.GetParamsFromFilter(reportFilter)

    //     reportInfo.values = this.GetParamsFromFilter({
    //         A1 : this.l('CompanyReportHeader')
    //     });

    //     reportInfo.pathName = "/COMMON/BC_VUNGMIEN.xlsx";
    //     reportInfo.storeName = "CM_REGIONS_Search";

    //     this.asposeService.getReport(reportInfo).subscribe(x => {
    //         this.fileDownloadService.downloadTempFile(x);
    //     });
    // }

    search(): void {

        this.showTableLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.regionService.cM_REGION_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading()))
            .subscribe(result => {
                this.appToolbar.setEnableForListPage();
                this.setRecords(result.items, result.totalCount);
                this.updateView();
                console.log(result);
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/region-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_REGION_ENTITY): void {
        this.navigatePassParam('/app/main/region-edit', { id: item.regioN_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: CM_REGION_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.regioN_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.saving = true;
                    this.regionService.cM_REGION_Del(item.regioN_ID)
                        .pipe(finalize(() => { this.saving = false; }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.notify.info(this.l('SuccessfullyDeleted'));
                                this.filterInputSearch.totalCount = 0;         
                                this.reloadPage();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: CM_REGION_ENTITY): void {

    }

    onViewDetail(item: CM_REGION_ENTITY): void {
        this.navigatePassParam('/app/main/region-view', { id: item.regioN_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {

    }



    onResetSearch(): void {
        this.filterInput = new CM_REGION_ENTITY();
        this.changePage(0);
    }

    getAllMenu(data?: LazyDropdownResponse): void {
        const filterInput: CM_REGION_ENTITY = new CM_REGION_ENTITY();
        filterInput.skipCount = data.state?.skipCount;
        filterInput.totalCount = data.state?.totalCount;
        filterInput.maxResultCount = data.state?.maxResultCount;
        filterInput.regioN_NAME = data.state?.filter;
        this.regionService.cM_REGION_Search(filterInput)
            .subscribe(result => {
                if (this.isFirstCall) {
                    this.isFirstCall = false;
                    var nullValue: any = {
                        regioN_CODE: ' ',
                        regioN_NAME: this.l('NullSelect')
                    };
                    result.items.unshift(nullValue);
                }
                data.callback(result)
            })
    }
}
