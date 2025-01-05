
import { Component, Injector, Input, OnChanges, OnInit, SimpleChanges, ViewChild, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PrimeTableComponent } from '../primeng-table/prime-table/prime-table.component';
import { PrimeTableOption } from '../primeng-table/prime-table/primte-table.interface';
import { ProcessServiceProxy, REQUEST_PROCESS_ENTITY, RequestProcessServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'current-process-view',
    templateUrl: './current-process-view.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class CurrentProcessViewComponent extends AppComponentBase implements OnInit, OnChanges {


    @Input() reQ_ID: string;
    @Input() Type: string;
    @ViewChild('tableCurrentProcessLog') tableCurrentProcessLog: PrimeTableComponent<REQUEST_PROCESS_ENTITY>;
    tableCurrentProcessLogOptions: PrimeTableOption<REQUEST_PROCESS_ENTITY>;

    constructor(injector: Injector,
        private requestProcessServiceProxy:RequestProcessServiceProxy) {
        super(injector);
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.getCurrentProcess();
    }

    ngOnInit() {
        this.getCurrentProcess();
        this.tableCurrentProcessLogOptions = {
            columns: [
                { title: 'PlCurrentProcessStep', name: 'description', sortField: 'description', width: '180px' },
                { title: 'PlCurrentProcessDVDM', name: 'dvdM_NAME', sortField: 'dvdM_NAME', width: '180px' },
                { title: 'PlCurrentProcessPerson', name: 'tlFullName', sortField: 'tlFullName', width: '180px', },
            ],
            config: {
                indexing: true,
                checkbox: false
            }
        }
    }


    getCurrentProcess(): void {

        if (!this.reQ_ID) {
            return;
        }

        this.requestProcessServiceProxy.pROCESS_CURRENT_SEARCH(this.reQ_ID, this.Type, this.appSession.user.userName).subscribe(response => {
            let listProcess = new Array<REQUEST_PROCESS_ENTITY>();
            let index = 1;
            response.forEach(process => {
                if (listProcess.filter(x => x.dvdM_NAME == process.dvdM_NAME).length > 0) {
                    if (listProcess.firstOrDefault(x => x.dvdM_NAME == process.dvdM_NAME).tlFullName == undefined) {
                        listProcess.firstOrDefault(x => x.dvdM_NAME == process.dvdM_NAME).tlFullName = process.tlFullName;
                    }
                    else {
                        listProcess.firstOrDefault(x => x.dvdM_NAME == process.dvdM_NAME).tlFullName += "; " + process.tlFullName;
                    }

                } else {
                    listProcess.push(process);
                    index++;
                }
                //this.tableCurrentProcessLog.setList(listProcess);
            });
            this.tableCurrentProcessLog.setList(listProcess);

        });
    }
}
