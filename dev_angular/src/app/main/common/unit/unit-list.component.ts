import { AfterViewInit, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AsposeServiceProxy, CM_SUPPLIER_TYPE_ENTITY, CM_UNIT_ENTITY, UnitServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from "rxjs";

@Component({
    templateUrl: './unit-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class UnitListComponent extends ListComponentBase<CM_UNIT_ENTITY> implements IUiAction<CM_UNIT_ENTITY>, OnInit, AfterViewInit {
    filterInput: CM_UNIT_ENTITY = new CM_UNIT_ENTITY();
    options: PrimeTableOption<CM_UNIT_ENTITY>;
    editPageState: EditPageState;
    supplierTypes: CM_SUPPLIER_TYPE_ENTITY[] = [];

    constructor(injector: Injector,
        private fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private unitService: UnitServiceProxy
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


    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'UnitCode', name: 'uniT_CODE', sortField: 'uniT_CODE', width: '120px' },
                { title: 'UnitName', name: 'uniT_NAME', sortField: 'uniT_NAME', width: '200px' },
                { title: 'Notes', name: 'notes', sortField: 'notes', width: '200px' },
                { title: 'IsActive', name: 'recorD_STATUS_NAME', sortField: 'recorD_STATUS_NAME', width: '100px' },
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
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setRole('Unit', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.appToolbar.setUiAction(this);
        this.cdr.detectChanges();
    }

    search(): void {
        this.showLoading();

        this.setSortingForFilterModel(this.filterInputSearch);

        this.unitService.cM_UNIT_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading()))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/main/unit-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_UNIT_ENTITY): void {
        this.navigatePassParam('/app/main/unit-edit', { id: item.uniT_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onDelete(item: CM_UNIT_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.uniT_CODE),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.unitService.cM_UNIT_Del(item.uniT_ID)
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

    onApprove(item: CM_UNIT_ENTITY): void {

    }

    onViewDetail(item: CM_UNIT_ENTITY): void {
        this.navigatePassParam('/app/main/unit-view', { id: item.uniT_ID }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onReject(item: CM_UNIT_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSendApp(item: CM_UNIT_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSave(): void {

    }

    onResetSearch(): void {
        this.filterInput = new CM_UNIT_ENTITY();
        this.changePage(0);
    }
}
