
import { Component, Injector, Input, OnChanges, OnInit, SimpleChanges, ViewChild, ViewEncapsulation } from '@angular/core';
import { PrimeTableComponent } from '@app/shared/core/controls/primeng-table/prime-table/prime-table.component';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ColumnType, PrimeTableOption } from '@app/shared/core/controls/primeng-table/prime-table/primte-table.interface';
import { ProcessServiceProxy, RequestProcessServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'process-history-view',
    templateUrl: './process-history-view.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class ProcessHistoryViewComponent extends DefaultComponentBase implements OnInit {
    
    @Input() reQ_ID: string;
    @Input() Type: string;
    @ViewChild('tableProcessLog') tableProcessLog: PrimeTableComponent<any>;

    options: PrimeTableOption<any>;

    constructor(
        injector: Injector,
        private requestProcessServiceProxy: RequestProcessServiceProxy
    ) {
        super(injector);
    }
 
    ngOnInit() {
        this.options = {
            columns: [
                { title: 'PlProcessPerson', name: 'checkeR_NAME', sortField: 'checkeR_NAME', width: '150px' },
                { title: 'PlProcessDate', name: 'approvE_DT', sortField: 'approvE_DT', width: '150px', type: ColumnType.DateTime24h },
                { title: 'PlProcessStep', name: 'procesS_DESC', sortField: 'procesS_DESC', width: '250px' },
                { title: 'PlProcessContent', name: 'notes', sortField: 'notes', width: '380px' },
            ],
            config: {
                indexing: true,
                checkbox: false,
            }
        }

        this.getHistoryProcess();
    }

    getHistoryProcess() {
        this.requestProcessServiceProxy.pROCESS_SEARCH(this.reQ_ID, this.Type, this.appSession.user.userName)
            .subscribe(response => {
                this.tableProcessLog.setAllRecords(response);
            });
    }
}
