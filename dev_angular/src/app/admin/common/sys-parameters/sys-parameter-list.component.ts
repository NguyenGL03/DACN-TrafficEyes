
import { AfterViewInit, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { ReportTypeConsts } from "@app/shared/core/utils/consts/ReportTypeConsts";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AsposeServiceProxy, ReportInfo, SYS_PARAMETERS_ENTITY, SysParametersServiceProxy, } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs/operators";

@Component({
    templateUrl: './sys-parameter-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class SysParameterListComponent extends ListComponentBase<SYS_PARAMETERS_ENTITY> implements IUiAction<SYS_PARAMETERS_ENTITY>, OnInit, AfterViewInit {
    filterInput: SYS_PARAMETERS_ENTITY = new SYS_PARAMETERS_ENTITY();

    constructor(injector: Injector,
        private fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private sysParameterService: SysParametersServiceProxy
    ) {
        super(injector);
        this.initFilter();
    }
    onReject(item: SYS_PARAMETERS_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: SYS_PARAMETERS_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'ParaKey', name: 'paraKey', sortField: 'paraKey', width: '200px' },
                { title: 'ParaValue', name: 'paraValue', sortField: 'paraValue', width: '200px' },
                { title: 'DataType', name: 'dataType', sortField: 'dataType', width: '200px' },
                { title: 'Description', name: 'description', sortField: 'description', width: '200px' },
                { title: 'RecordStatus', name: 'recorD_STATUS_NAME', sortField: 'recorD_STATUS_NAME', width: '200px' },
            ],
            config: {
                indexing: true,
                checkbox: false,
            }
        }
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Administration');
        this.appToolbar.setUiAction(this);
        this.appToolbar.setRole('SysParameter', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.cdr.detectChanges();
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

        reportInfo.pathName = "/COMMON/BC_THAMSO.xlsx";
        reportInfo.storeName = "SYS_PARAMETERS_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }

    search(): void {
        this.showLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.sysParameterService.sYS_PARAMETERS_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
                this.appToolbar.setEnableForListPage();
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/admin/sys-parameter-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: SYS_PARAMETERS_ENTITY): void {
        this.navigatePassParam('/app/admin/sys-parameter-edit', { id: item.id }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: SYS_PARAMETERS_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.paraKey),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.sysParameterService.sYS_PARAMETERS_Del(item.id)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                            this.filterInputSearch.totalCount = 0;
                            this.reloadPage();
                        });
                }
            }
        );
    }

    onApprove(item: SYS_PARAMETERS_ENTITY): void {

    }

    onViewDetail(item: SYS_PARAMETERS_ENTITY): void {
        this.navigatePassParam('/app/admin/sys-parameter-view', { id: item.id }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {

    }

    onResetSearch(): void {
        this.filterInput = new SYS_PARAMETERS_ENTITY();
        this.changePage(0);
    }
}
