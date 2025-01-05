import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { TextAlign } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { CM_PROCESS_LIST_ENTITY, ProcessServiceProxy } from '@shared/service-proxies/service-proxies';
import { timer } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    templateUrl: './cm-process-concenter-list.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CmProcessConcenterListComponent extends ListComponentBase<CM_PROCESS_LIST_ENTITY> implements IUiAction<CM_PROCESS_LIST_ENTITY>, OnInit, AfterViewInit {
    filterInput: CM_PROCESS_LIST_ENTITY = new CM_PROCESS_LIST_ENTITY();

    constructor(
        injector: Injector,
        private processService: ProcessServiceProxy,
        private changeDetector: ChangeDetectorRef,
    ) {
        super(injector);
        this.initFilter();
    }

    onDelete(item: CM_PROCESS_LIST_ENTITY): void {
        this.message.confirm(
            this.l('DeleteWarningMessage', item.procesS_KEY),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.showLoading();
                    this.processService.cM_PROCESS_LIST_Del(item.procesS_KEY)
                        .pipe(finalize(() => {
                            this.hideLoading();
                        }))
                        .subscribe((response) => {
                            if (response.Result !== '0') {
                                this.showErrorMessage(response.errorDesc);
                            } else {
                                this.showSuccessMessage(this.l('SuccessfullyDeleted'));
                                timer(1500).subscribe(() => { this.removeMessage(); })
                                this.search();
                            }
                        });
                }
            }
        );
    }

    onReject(item: CM_PROCESS_LIST_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSendApp(item: CM_PROCESS_LIST_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'Mã quy trình', name: 'procesS_KEY', sortField: 'PROCESS_KEY', width: '100px', align: TextAlign.Left },
                { title: 'Tên quy trình', name: 'procesS_NAME', sortField: 'PROCESS_NAME', width: '150px', align: TextAlign.Left },
                { title: 'Mã quy trình bắt đầu', name: 'preV_PROCESS_KEY', sortField: 'preV_PROCESS_KEY', width: '100px', align: TextAlign.Left },
                { title: 'Tên quy trình bắt đầu', name: 'preV_PROCESS_NAME', sortField: 'preV_PROCESS_NAME', width: '150px', align: TextAlign.Left },
                { title: 'Số bước', name: 'procesS_COUNT', sortField: 'PROCESS_COUNT', width: '150px', align: TextAlign.Center },
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
        this.appToolbar.setRole('CmProcessConcenter', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.changeDetector.detectChanges();
    }

    search(): void {
        this.showLoading();
        this.setSortingForFilterModel(this.filterInputSearch);
        this.processService.cM_PROCESS_LIST_Search(this.filterInputSearch)
            .pipe(finalize(() => {
                this.hideLoading();
            }))
            .subscribe(result => {
                this.setRecords(result.items, result.totalCount);
            });
    }

    onAdd(): void {
        this.navigatePassParam('/app/admin/cm-process-concenter-add', null, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onUpdate(item: CM_PROCESS_LIST_ENTITY): void {
        this.navigatePassParam('/app/admin/cm-process-concenter-edit', { id: item.procesS_KEY }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onApprove(item: CM_PROCESS_LIST_ENTITY): void {

    }

    onViewDetail(item: CM_PROCESS_LIST_ENTITY): void {
        this.navigatePassParam('/app/admin/cm-process-concenter-view', { id: item.procesS_KEY }, { filterInput: JSON.stringify(this.filterInputSearch) });
    }

    onSave(): void {

    }

    onResetSearch(): void {
        this.filterInput = new CM_PROCESS_LIST_ENTITY();
        this.changePage(0);
    }
}
