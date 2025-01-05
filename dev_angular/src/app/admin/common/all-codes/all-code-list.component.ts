import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { ReportTypeConsts } from "@app/shared/core/utils/consts/ReportTypeConsts";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AllCodeServiceProxy, AsposeServiceProxy, CM_ALLCODE_ENTITY, ReportInfo, } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs/operators";

@Component({
    templateUrl: './all-code-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class AllCodeListComponent extends ListComponentBase<CM_ALLCODE_ENTITY> implements IUiAction<CM_ALLCODE_ENTITY>, OnInit, AfterViewInit {
    options: PrimeTableOption<CM_ALLCODE_ENTITY>;
    filterInput: CM_ALLCODE_ENTITY = new CM_ALLCODE_ENTITY();
    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private allCodeService: AllCodeServiceProxy,
    ) {
        super(injector);
        this.initFilter();
    }
    onReject(item: CM_ALLCODE_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: CM_ALLCODE_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'CDType', name: 'cdtype', sortField: 'cdtype', width: '200px' },
                { title: 'CDName', name: 'cdname', sortField: 'cdname', width: '200px' },
                { title: 'CDVal', name: 'cdval', sortField: 'cdval', width: '200px' },
                { title: 'CDContent', name: 'content', sortField: 'content', width: '150px' },
                { title: 'CDlstord', name: 'lstord', sortField: 'lstord', width: '200px' },
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
        this.appToolbar.setRole('AllCode', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.search();
        this.changeDetector.detectChanges();
    }

    search(): void {
        this.showTableLoading();
        this.setSortingForFilterModel(this.filterInput);
        this.allCodeService.cM_ALLCODE_Search(this.filterInput)
            .pipe(finalize(() => this.hideTableLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/admin/all-code-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_ALLCODE_ENTITY): void {
        this.navigatePassParam('/app/admin/all-code-edit', { cdName: item.cdname, cdType: item.cdtype, cdVal: item.cdval }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: CM_ALLCODE_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.content),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.allCodeService.cM_ALLCODE_Del(item.id)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            } else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                                this.reloadPage();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: CM_ALLCODE_ENTITY): void {
    }

    onViewDetail(item: CM_ALLCODE_ENTITY): void {
        this.navigatePassParam('/app/admin/all-code-view', { cdName: item.cdname, cdType: item.cdtype, cdVal: item.cdval }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {
    }

    onResetSearch(): void {
        this.filterInput = new CM_ALLCODE_ENTITY();
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

        reportInfo.pathName = "/COMMON/BC_ALLCODE.xlsx";
        reportInfo.storeName = "CM_ALLCODE_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }

}
