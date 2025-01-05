import { AfterViewInit, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { BranchModalComponent } from '@app/shared/common/modals/branch-modal/branch-modal.component';
import { ReportTypeConsts } from '@app/shared/core/utils/consts/ReportTypeConsts';
import { ListComponentBase } from '@app/utilities/list-component-base';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AsposeServiceProxy, CM_HANGHOA_GROUP_ENTITY, HangHoaGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { finalize } from 'rxjs/operators';
import { ReportInfo } from '../../../../shared/service-proxies/service-proxies';



@Component({
    templateUrl: './hanghoa-group-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class HangHoaGroupListComponent extends ListComponentBase<CM_HANGHOA_GROUP_ENTITY> implements IUiAction<CM_HANGHOA_GROUP_ENTITY>, OnInit, AfterViewInit {

    hangHoaGroups: CM_HANGHOA_GROUP_ENTITY[]
    filterInput: CM_HANGHOA_GROUP_ENTITY = new CM_HANGHOA_GROUP_ENTITY();
    @ViewChild('branchModal') branchModal: BranchModalComponent;

    constructor(
        injector: Injector,
        private fileDownloadService: FileDownloadService,
        private hangHoaGroupService: HangHoaGroupServiceProxy,
        private asposeService: AsposeServiceProxy,
    ) {
        super(injector);
        this.initCombobox();
        this.initFilter();
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setUiAction(this);
        this.appToolbar.setRole('HangHoaType', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.cdr.detectChanges();
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'GoodsGroupCode', name: 'hH_GROUP_CODE', sortField: 'hH_GROUP_CODE', width: '200px' },
                { title: 'GoodsGroupName', name: 'hH_GROUP_NAME', sortField: 'hH_GROUP_NAME', width: '200px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '200px' },
                { title: 'RecordStatus', name: 'recorD_STATUS_NAME', sortField: 'recorD_STATUS_NAME', width: '200px' },
                { title: 'Branch', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '200px' },
                { title: 'AuthStatus', name: 'autH_STATUS_NAME', sortField: 'autH_STATUS_NAME', width: '200px' },
            ],
            config: {
                indexing: true
            }
        }
    }

    initCombobox() {
        let hangHoaGroup = new CM_HANGHOA_GROUP_ENTITY();
        this.hangHoaGroupService.cM_HANGHOA_GROUP_Search(hangHoaGroup).subscribe(res => {
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
            A1: this.l('CompanyReportHeader')
        });

        reportInfo.pathName = "/COMMON/BC_NHOMHANGHOA.xlsx";
        reportInfo.storeName = "CM_HANGHOA_GROUP_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }

    showBranchModal() {
        this.branchModal.show();
    }

    onSelectBranch(branch: CM_HANGHOA_GROUP_ENTITY) {
        this.filterInput.brancH_CODE = branch.brancH_CODE;
        this.filterInput.brancH_NAME = branch.brancH_NAME;
    }

    search(): void {
        this.showTableLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.hangHoaGroupService.cM_HANGHOA_GROUP_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
                this.appToolbar.setEnableForListPage();
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/hanghoagroup-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });

    }
    onUpdate(item: CM_HANGHOA_GROUP_ENTITY): void {
        this.navigatePassParam('/app/main/hanghoagroup-edit', { id: item.hH_GROUP_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }
    onViewDetail(item: CM_HANGHOA_GROUP_ENTITY): void {
        this.navigatePassParam('/app/main/hanghoagroup-view', { id: item.hH_GROUP_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }
    onDelete(item: CM_HANGHOA_GROUP_ENTITY): void {
        if (item.autH_STATUS == 'A') {
            this.showErrorMessage(this.l('DeleteFailed'));
            return;
        }
        this.message.confirm(
            this.l('DeleteWarningMessage', item.hH_GROUP_NAME),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.hangHoaGroupService.cM_HANGHOA_GROUP_Del(item.hH_GROUP_ID)
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
    onApprove(item: CM_HANGHOA_GROUP_ENTITY): void {

    }
    onSave(): void {

    }
    onResetSearch(): void {
        this.filterInput = new CM_HANGHOA_GROUP_ENTITY();
        this.changePage(0);
    }

    onReject(item: CM_HANGHOA_GROUP_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSendApp(item: CM_HANGHOA_GROUP_ENTITY): void {
        throw new Error('Method not implemented.');
    }

}
