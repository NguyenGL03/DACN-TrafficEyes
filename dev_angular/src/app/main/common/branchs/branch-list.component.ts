import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, QueryList, ViewChild, ViewChildren, ViewEncapsulation } from "@angular/core";
import { BranchModalComponent } from "@app/shared/common/modals/branch-modal/branch-modal.component";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { InputType, PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { ReportTypeConsts } from "@app/shared/core/utils/consts/ReportTypeConsts";
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AsposeServiceProxy, BranchServiceProxy, CM_BRANCH_ENTITY, DynamicPrimeTableServiceProxy, ReportInfo } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs";

@Component({
    templateUrl: './branch-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class BranchListComponent extends ListComponentBase<CM_BRANCH_ENTITY> implements IUiAction<CM_BRANCH_ENTITY>, OnInit, AfterViewInit {
    filterInput: CM_BRANCH_ENTITY = new CM_BRANCH_ENTITY();
    options: PrimeTableOption<CM_BRANCH_ENTITY>;
    editPageState: EditPageState;
    @ViewChild('branchModal') branchModal: BranchModalComponent;

    constructor(
        injector: Injector,
        private branchService: BranchServiceProxy,
        private asposeService: AsposeServiceProxy,
        private fileDownloadService: FileDownloadService,
        private changeDetector: ChangeDetectorRef,
        private _dynamicPrimeTable: DynamicPrimeTableServiceProxy,
    ) {
        super(injector);
        this.initFilter();
    }
    onReject(item: CM_BRANCH_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: CM_BRANCH_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    initDefaultFilter() {
        this.filterInput.top = 200;
    }

    initCombobox() {
        this.filterInput.top = 200;
    }

    async loadTable() {
        this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_GetAllData('branch-list')
            .subscribe((response) => {
                const options = this.handleGenerateOptions(response);
                this.options = options[0];
                console.log(this.options);
            })
    }

    handleSelect(event: any) {
        console.log(event)
    }

    ngOnInit(): void {
        this.loadTable();
        this.initCombobox();
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setRole('Branch', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }

    search(): void {
        this.showLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.branchService.cM_BRANCH_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading()))
            .subscribe(result => {
                this.dataTable.setDropdownItems('brancH_NAME', result.items);
                this.setRecords(result.items, result.totalCount);
                this.cdr.detectChanges();
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/branch-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_BRANCH_ENTITY): void {
        this.navigatePassParam('/app/main/branch-edit', { id: item.brancH_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: CM_BRANCH_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.brancH_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.branchService.cM_BRANCH_Del(item.brancH_ID)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0' && response.result != null && response.result != undefined) {
                                this.showErrorMessage(response.errorDesc);
                            } else if ((response.result === null || response.result === undefined) && (response.errorDesc === null || response.errorDesc === undefined)) {
                                this.showErrorMessage('Không thể xóa đơn vị này ! ')
                            } else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                                this.filterInputSearch.totalCount = 0;
                                this.reloadPage();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: CM_BRANCH_ENTITY): void {

    }

    onViewDetail(item: CM_BRANCH_ENTITY): void {
        this.navigatePassParam('/app/main/branch-view', { id: item.brancH_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {

    }

    onResetSearch(): void {
        this.filterInput = new CM_BRANCH_ENTITY();
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

        reportInfo.pathName = "/COMMON/BC_DONVI.xlsx";
        reportInfo.storeName = "rpt_BC_DONVI";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }
}
