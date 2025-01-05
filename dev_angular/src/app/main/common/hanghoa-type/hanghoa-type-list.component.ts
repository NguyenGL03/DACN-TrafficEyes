import { HangHoaTypeServiceProxy, ReportInfo } from '../../../../shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AfterViewInit, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { AsposeServiceProxy, CM_HANGHOA_GROUP_ENTITY, CM_HANGHOA_TYPE_ENTITY, HangHoaGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { finalize } from 'rxjs/operators';
import { ListComponentBase } from '@app/utilities/list-component-base';
import { IUiAction } from '@app/utilities/ui-action';
import { BranchModalComponent } from '@app/shared/common/modals/branch-modal/branch-modal.component';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { ReportTypeConsts } from '@app/shared/core/utils/consts/ReportTypeConsts';



@Component({
    templateUrl: './hanghoa-type-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class HangHoaTypeListComponent extends ListComponentBase<CM_HANGHOA_TYPE_ENTITY> implements IUiAction<CM_HANGHOA_TYPE_ENTITY>, OnInit, AfterViewInit {

    hangHoaGroups: CM_HANGHOA_GROUP_ENTITY[]
    filterInput: CM_HANGHOA_TYPE_ENTITY = new CM_HANGHOA_TYPE_ENTITY();
    @ViewChild('branchModal') branchModal: BranchModalComponent;

    constructor(injector: Injector,
        private fileDownloadService: FileDownloadService,
        private hangHoaGroupService: HangHoaGroupServiceProxy,
        private hangHoaTypeService: HangHoaTypeServiceProxy,
        private asposeService: AsposeServiceProxy,) {
        super(injector);
        this.initCombobox();
        this.initFilter();
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'GoodsTypeCode', name: 'hH_TYPE_CODE', sortField: 'hH_TYPE_CODE', width: '200px' },
                { title: 'GoodsTypeName', name: 'hH_TYPE_NAME', sortField: 'hH_TYPE_NAME', width: '200px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '200px' },
                { title: 'RecordStatus', name: 'recorD_STATUS_NAME', sortField: 'recorD_STATUS_NAME', width: '200px' },
                { title: 'Branch', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '200px' },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS_NAME', width: '200px' },
  
            ],
            config: {
                indexing: true,
            }
        }
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        this.appToolbar.setRole('HangHoaType', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.cdr.detectChanges();
    }

    initCombobox() {
        var hhGroup = this.getFillterForCombobox();
        this.hangHoaGroupService.cM_HANGHOA_GROUP_Search(hhGroup).subscribe(res => {
            this.hangHoaGroups = res.items;
        });
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
            A1: this.l('Ngân hàng TMCP Xuất nhập khẩu Việt Nam').toUpperCase()
        });

        reportInfo.pathName = "/COMMON/BC_LOAIHANGHOA.xlsx";
        reportInfo.storeName = "CM_HANGHOA_TYPE_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }

    showBranchModal() {
        this.branchModal.show();
    }

    onSelectBranch(branch: CM_HANGHOA_TYPE_ENTITY) {
        this.filterInput.brancH_CODE = branch.brancH_CODE;
        this.filterInput.brancH_NAME = branch.brancH_NAME;
    }

    search(): void {
        this.showTableLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.hangHoaTypeService.cM_HANGHOA_TYPE_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
                this.appToolbar.setEnableForListPage();
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/hanghoatype-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });

    }
    onUpdate(item: CM_HANGHOA_TYPE_ENTITY): void {
        this.navigatePassParam('/app/main/hanghoatype-edit', { id: item.hH_TYPE_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }
    onViewDetail(item: CM_HANGHOA_TYPE_ENTITY): void {
        this.navigatePassParam('/app/main/hanghoatype-view', { id: item.hH_TYPE_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }
    onDelete(item: CM_HANGHOA_TYPE_ENTITY): void {
        if (item.autH_STATUS == AuthStatusConsts.Approve) {
            this.showErrorMessage("Xóa thất bại! Chỉ được xóa loại hàng hóa có trạng thái duyệt là 'chờ duyệt'");
            return;
        }
        this.message.confirm(
            this.l('DeleteWarningMessage', item.hH_TYPE_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.hangHoaTypeService.cM_HANGHOA_TYPE_Del(item.hH_TYPE_ID)
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
    onApprove(item: CM_HANGHOA_TYPE_ENTITY): void {

    }
    onSave(): void {

    }
    onResetSearch(): void {
        this.filterInput = new CM_HANGHOA_TYPE_ENTITY();
        this.changePage(0);
    }
    onReject(item: CM_HANGHOA_TYPE_ENTITY): void {
        throw new Error('Method not implemented.');
    }
    onSendApp(item: CM_HANGHOA_TYPE_ENTITY): void {
        throw new Error('Method not implemented.');
    }
}
