import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { LazyDropdownResponse } from "@app/shared/core/controls/dropdown-lazy-control/dropdown-lazy-control.component";
import { PrimeTableOption, TextAlign } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { ReportTypeConsts } from "@app/shared/core/utils/consts/ReportTypeConsts";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppMenuServiceProxy, AsposeServiceProxy, ReportInfo, TL_MENU_ENTITY } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs";

@Component({
    templateUrl: './app-menu-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],

})

export class AppMenuListComponent extends ListComponentBase<TL_MENU_ENTITY> implements IUiAction<TL_MENU_ENTITY>, OnInit, AfterViewInit {

    filterInput: TL_MENU_ENTITY = new TL_MENU_ENTITY();
    menuItems: TL_MENU_ENTITY[];
    options: PrimeTableOption<TL_MENU_ENTITY>;
    isFirstCall: boolean = true;

    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private asposeService: AsposeServiceProxy,
        private fileDownloadService: FileDownloadService,
        private menuService: AppMenuServiceProxy,
    ) {
        super(injector);
    }

    onReject(item: TL_MENU_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSendApp(item: TL_MENU_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'MenuCode', name: 'menU_ID', sortField: 'menU_ID', width: '120px', align: TextAlign.Left },
                { title: 'MenuName', name: 'menU_NAME', sortField: 'menU_NAME', width: '200px' },
                { title: 'MenuEnglishName', name: 'menU_NAME_EL', sortField: 'menU_NAME_EL', width: '200px' },
                { title: 'MenuIcon', name: 'menU_ICON', sortField: 'menU_ICON', width: '150px' },
                { title: 'MenuPath', name: 'menU_LINK', sortField: 'menU_LINK', width: '200px' },
                { title: 'MenuParent', name: 'menU_PARENT', sortField: 'menU_PARENT', width: '120px', align: TextAlign.Center },
                { title: 'MenuOrder', name: 'menU_ORDER', sortField: 'menU_ORDER', width: '120px', align: TextAlign.Center },
                { title: 'ApproveFunc', name: 'isapprovE_FUNC', sortField: 'isapprovE_FUNC', width: '150px', align: TextAlign.Center },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS', width: '150px' },
            ],
            config: {
                indexing: true,
                checkbox: false
            }
        }
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setUiAction(this);
        this.appToolbar.setPrefix('Pages.Administration');
        this.appToolbar.setRole('Menu', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.search();
        this.changeDetector.detectChanges();
    }

    search(): void {
        this.showLoading();
        this.setSortingForFilterModel(this.filterInputSearch);
        this.menuService.tL_MENU_Search(this.filterInputSearch)
            .pipe(finalize(() => {
                this.hideLoading();
            }))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
                this.appToolbar.setEnableForListPage();
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/admin/app-menu-add', null, { filterInput: JSON.stringify(this.filterInput) });
    }

    onUpdate(item: TL_MENU_ENTITY): void {
        console.log(item)
        this.navigatePassParam('/app/admin/app-menu-edit', { id: item.menU_ID }, { filterInput: JSON.stringify(this.filterInput) });
    }

    onDelete(item: TL_MENU_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.menU_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.menuService.tL_MENU_Del(item.menU_ID)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            } else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                                this.search();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: TL_MENU_ENTITY): void {

    }

    onViewDetail(item: TL_MENU_ENTITY): void {
        this.navigatePassParam('/app/admin/app-menu-view', { id: item.menU_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {

    }

    onResetSearch(): void {
        this.changePage(0);
    }

    exportToExcel() {
        let reportInfo = new ReportInfo();
        reportInfo.typeExport = ReportTypeConsts.Excel;

        let reportFilter = { ...this.filterInputSearch };

        reportFilter.maxResultCount = -1;

        reportInfo.parameters = this.GetParamsFromFilter(reportFilter)

        reportInfo.values = this.GetParamsFromFilter({
            A1: this.l('CompanyReportHeader')
        });

        reportInfo.pathName = "/COMMON/BC_THONGTINTRANG.xlsx";
        reportInfo.storeName = "TL_MENU_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }

    getAllMenu(data?: LazyDropdownResponse): void {
        const filterInput: TL_MENU_ENTITY = new TL_MENU_ENTITY();
        filterInput.skipCount = data.state?.skipCount;
        filterInput.totalCount = data.state?.totalCount;
        filterInput.maxResultCount = data.state?.maxResultCount;
        filterInput.menU_NAME = data.state?.filter;
        this.menuService.tL_MENU_Search(filterInput)
            .subscribe(result => {
                if (this.isFirstCall) {
                    this.isFirstCall = false;
                    var nullValue: any = {
                        menU_ID: ' ',
                        menU_NAME: this.l('NullSelect')
                    };
                    result.items.unshift(nullValue);
                }
                data.callback(result)
            })
    }
}
