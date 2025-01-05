import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from "@angular/core";
import { BranchModalComponent } from "@app/shared/common/modals/branch-modal/branch-modal.component";
import { PrimeTableOption, TextAlign } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { ReportTypeConsts } from "@app/shared/core/utils/consts/ReportTypeConsts";
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AsposeServiceProxy, CM_SUPPLIER_ENTITY, CM_SUPPLIER_TYPE_ENTITY, ReportInfo, SupplierServiceProxy, SupplierTypeServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs";

@Component({
    templateUrl: './supplier-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class SupplierListComponent extends ListComponentBase<CM_SUPPLIER_ENTITY> implements IUiAction<CM_SUPPLIER_ENTITY>, OnInit, AfterViewInit {
    filterInput: CM_SUPPLIER_ENTITY = new CM_SUPPLIER_ENTITY();
    options: PrimeTableOption<CM_SUPPLIER_ENTITY>;
    editPageState: EditPageState;
    supplierTypes: CM_SUPPLIER_TYPE_ENTITY[] = [];

    @ViewChild('branchModal') branchModal: BranchModalComponent;

    constructor(
        injector: Injector,
        private fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private changeDetector: ChangeDetectorRef,
        private supplierService: SupplierServiceProxy,
        private supplierTypeService: SupplierTypeServiceProxy
    ) {
        super(injector);
        this.initFilter();
    }

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    initDefaultFilter() {
        this.filterInput.top = 200;
    }

    initCombobox() {
        this.supplierTypeService.cM_SUPPLIERTYPE_Search(this.getFillterForCombobox())
            .subscribe(response => {
                this.supplierTypes = response.items;
            })
    }

    ngOnInit(): void {
        this.initCombobox();
        this.generateTable();
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.generateTable();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setRole('AssAddNew', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }

    generateTable() {
        this.options = {
            columns: [
                { title: 'SupplierCode', name: 'suP_CODE', sortField: 'suP_CODE', width: '150px' },
                { title: 'SupplierName', name: 'suP_NAME', sortField: 'suP_NAME', width: '200px' },
                { title: 'SupplierType', name: 'suP_TYPE_NAME', sortField: 'suP_TYPE_NAME', width: '200px' },
                { title: 'Address', name: 'addr', sortField: 'addr', width: '150px' },
                { title: 'Email', name: 'email', sortField: 'email', width: '200px' },
                { title: 'TaxNo', name: 'taX_NO', sortField: 'taX_NO', width: '200px' },
                { title: 'Tel', name: 'tel', sortField: 'tel', width: '120px' },
                { title: 'ContactPerson', name: 'contacT_PERSON', sortField: 'contacT_PERSON', width: '150px', },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS', width: '150px' },
                { title: 'Maker', name: 'makeR_ID', sortField: 'makeR_ID', width: '150px' },
                { title: 'Branch', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '150px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '150px' },
            ],
            config: {
                indexing: true,
                checkbox: false
            }
        }
    }

    exportToExcel() {
        let reportInfo = new ReportInfo();
        reportInfo.typeExport = ReportTypeConsts.Excel;

        let reportFilter = { ...this.filterInputSearch };

        reportFilter.maxResultCount = -1;

        reportInfo.parameters = this.GetParamsFromFilter(reportFilter)
        if (!this.filterInput.brancH_CODE) {
            // this.filterInput.brancH_CODE = this.appSession.user.branch.brancH_CODE;
            // this.filterInput.brancH_NAME = this.appSession.user.branch.brancH_NAME;
        }

        reportInfo.values = this.GetParamsFromFilter({
            A1: this.l('CompanyReportHeader')
        });

        reportInfo.pathName = "/COMMON/BC_NCC.xlsx";
        reportInfo.storeName = "rpt_BC_NHA_CC";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }

    showBranchModal() {
        this.branchModal.show();
    }

    onSelectBranch(branch: CM_SUPPLIER_ENTITY) {
        this.filterInput.brancH_CODE = branch.brancH_CODE;
        this.filterInput.brancH_NAME = branch.brancH_NAME;
    }

    deleteSelectedBranch() {
        this.filterInput.brancH_CODE = undefined;
        this.filterInput.brancH_NAME = undefined;
    }

    search(): void {
        this.showTableLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.supplierService.cM_SUPPLIER_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/supplier-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_SUPPLIER_ENTITY): void {
        this.navigatePassParam('/app/main/supplier-edit', { id: item.suP_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: CM_SUPPLIER_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.suP_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.supplierService.cM_SUPPLIER_Del(item.suP_ID, this.appSession.user.userName)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                                this.filterInputSearch.totalCount = 0;
                                // this.reloadPage();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: CM_SUPPLIER_ENTITY): void {

    }

    onViewDetail(item: CM_SUPPLIER_ENTITY): void {
        this.navigatePassParam('/app/main/supplier-view', { id: item.suP_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {

    }

    onResetSearch(): void {
        this.filterInput = new CM_SUPPLIER_ENTITY();
        this.changePage(0);
    }

    onReject(item: CM_SUPPLIER_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: CM_SUPPLIER_ENTITY): void {
        throw new Error("Method not implemented.");
    }
}
