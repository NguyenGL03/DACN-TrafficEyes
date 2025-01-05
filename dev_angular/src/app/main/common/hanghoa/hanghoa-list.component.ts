import { CM_HANGHOA_ENTITY, CM_HANGHOA_GROUP_ENTITY, CM_HANGHOA_TYPE_ENTITY, HangHoaGroupServiceProxy, HangHoaServiceProxy, HangHoaTypeServiceProxy, ReportInfo } from '@shared/service-proxies/service-proxies';
import { AsposeServiceProxy } from '@shared/service-proxies/service-proxies';
import { Injector, Component, OnInit, ViewEncapsulation, AfterViewInit, ViewChild } from "@angular/core";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs/operators";
import { BranchModalComponent } from '@app/shared/common/modals/branch-modal/branch-modal.component';
import { ListComponentBase } from '@app/utilities/list-component-base';
import { IUiAction } from '@app/utilities/ui-action';
import { ReportTypeConsts } from '@app/shared/core/utils/consts/ReportTypeConsts';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { ColumnType } from '@app/shared/core/controls/primeng-table/prime-table/primte-table.interface';

@Component({
    templateUrl: './hanghoa-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class HangHoaListComponent extends ListComponentBase<CM_HANGHOA_ENTITY> implements IUiAction<CM_HANGHOA_ENTITY>, OnInit, AfterViewInit {

    @ViewChild('branchModal') branchModal: BranchModalComponent;

    filterInput: CM_HANGHOA_ENTITY = new CM_HANGHOA_ENTITY();
    hangHoaGroups: CM_HANGHOA_GROUP_ENTITY[];
    hangHoaTypes: CM_HANGHOA_TYPE_ENTITY[];

    constructor(
        injector: Injector,
        private fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private hangHoaGroupService: HangHoaGroupServiceProxy,
        private hangHoaTypeService: HangHoaTypeServiceProxy,
        private hanghoaService: HangHoaServiceProxy
    ) {
        super(injector);
        this.initCombobox();
        this.initFilter();
    }


    initCombobox() {
        this.hangHoaGroupService.cM_HANGHOA_GROUP_GetAll().subscribe(res => {
            this.hangHoaGroups = res;
        });

        this.hangHoaTypeService.cM_HANGHOA_TYPE_GetAll().subscribe(res => {
            this.hangHoaTypes = res;
        });
    }


    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'GoodsCode', name: 'hH_CODE', sortField: 'hH_CODE', width: '200px' },
                { title: 'GoodsName', name: 'hH_NAME', sortField: 'hH_NAME', width: '200px' },
                { title: 'GoodsTypeCode', name: 'hH_TYPE_CODE', sortField: 'hH_TYPE_CODE', width: '200px' },
                { title: 'Description', name: 'description', sortField: 'description', width: '200px' },
                { title: 'SupplierCode', name: 'suP_CODE', sortField: 'suP_CODE', width: '200px' },
                { title: 'Branch', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '200px' },
                { title: 'Price', name: 'price', sortField: 'price', width: '200px', type: ColumnType.Curency },
                { title: 'UnitName', name: 'uniT_NAME', sortField: 'uniT_NAME', width: '200px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '200px' },
                { title: 'RecordStatus', name: 'recorD_STATUS_NAME', sortField: 'recorD_STATUS_NAME', width: '200px' },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS_NAME', width: '200px' },
            ],
            config: {
                indexing: true
            }
        }
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        this.appToolbar.setRole('HangHoa', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.cdr.detectChanges();
    }

    initDefaultFilter() {
        this.filterInput.top = 1000;
    }

    exportToExcel() {
        let reportInfo = new ReportInfo();
        reportInfo.typeExport = ReportTypeConsts.Excel;

        let reportFilter = { ...this.filterInputSearch };
        if (!this.filterInput.brancH_CODE) {
            this.filterInput.brancH_CODE = this.appSession.user.branch.brancH_CODE;
            this.filterInput.brancH_NAME = this.appSession.user.branch.brancH_NAME;
        }
        reportFilter.maxResultCount = -1;

        reportInfo.parameters = this.GetParamsFromFilter(reportFilter)

        reportInfo.values = this.GetParamsFromFilter({
            A1: this.l('CompanyReportHeader')
        });

        reportInfo.pathName = "/COMMON/BC_HANGHOA.xlsx";
        reportInfo.storeName = "CM_HANGHOA_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }
    showBranchModal() {
        this.branchModal.show();
    }

    onSelectBranch(branch: CM_HANGHOA_ENTITY) {
        this.filterInput.brancH_CODE = branch.brancH_CODE;
        this.filterInput.brancH_NAME = branch.brancH_NAME;
    }
    search(): void {
        this.showTableLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.hanghoaService.cM_HANGHOA_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
                this.appToolbar.setEnableForListPage();
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/hanghoa-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_HANGHOA_ENTITY): void {
        this.navigatePassParam('/app/main/hanghoa-edit', { id: item.hH_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: CM_HANGHOA_ENTITY): void {
        if (item.autH_STATUS == AuthStatusConsts.Approve) {
            this.showErrorMessage("Xóa thất bại! Chỉ được xóa hàng hóa có trạng thái duyệt là 'chờ duyệt'");
            return;
        }

        this.message.confirm(
            this.l('DeleteWarningMessage', item.hH_CODE),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.hanghoaService.cM_HANGHOA_Del(item.hH_ID, this.appSession.user.userName)
                        .pipe(finalize(() => { this.hideLoading(); }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                                this.filterInputSearch.totalCount = 0;
                                this.reloadPage();
                            }
                        });
                }
            }
        );
    }

    onApprove(item: CM_HANGHOA_ENTITY): void {

    }

    onViewDetail(item: CM_HANGHOA_ENTITY): void {
        this.navigatePassParam('/app/main/hanghoa-view', { id: item.hH_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {

    }

    onResetSearch(): void {
        this.filterInput = new CM_HANGHOA_ENTITY();
        this.changePage(0);
    }

    onReject(item: CM_HANGHOA_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSendApp(item: CM_HANGHOA_ENTITY): void {
        throw new Error('Method not implemented.');
    }
}
