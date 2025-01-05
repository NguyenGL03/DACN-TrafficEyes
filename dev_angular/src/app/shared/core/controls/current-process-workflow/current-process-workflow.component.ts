
import { Component, Injector, Input, OnChanges, OnInit, SimpleChanges, ViewChild, ViewEncapsulation } from '@angular/core';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CM_PROCESS_ENTITY, ProcessServiceProxy } from '@shared/service-proxies/service-proxies';
import { PrimeTableComponent } from '../primeng-table/prime-table/prime-table.component';
import { PrimeTableOption, TextAlign } from '../primeng-table/prime-table/primte-table.interface';

@Component({
    selector: 'current-process-workflow',
    templateUrl: './current-process-workflow.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class CurrentProcessWorkflowComponent extends DefaultComponentBase implements OnInit, OnChanges {


    @Input() reQ_ID: string;
    @Input() status: string;
    @Input() procesS_ID: string;
    @Input() Branch_id: string
    @Input() Dept_id: string
    @ViewChild('tableCurrentProcessLog') tableCurrentProcessLog: PrimeTableComponent<CM_PROCESS_ENTITY>;
    options: PrimeTableOption<CM_PROCESS_ENTITY>
    constructor(injector: Injector,
        private processService: ProcessServiceProxy) {
        super(injector);
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.getCurrentProcess();
    }

    ngOnInit() {

        this.options = {
            columns: [{ title: 'PlCurrentProcessStep', name: 'describe', sortField: 'describe', width: '250px', align: TextAlign.Center },
            { title: 'PlCurrentProcessDVDM', name: 'dvdM_NAME', sortField: 'dvdM_NAME', width: '250px', align: TextAlign.Center },
            { title: 'PlCurrentProcessPerson', name: 'tlFullName', sortField: 'tlFullName', width: '250px', align: TextAlign.Center },
            ],
            config: {
                indexing: true,
                checkbox: false
            }
        }

        this.getCurrentProcess();
    }


    getCurrentProcess(): void {

        if (!this.reQ_ID) {
            return;
        }
        
        let filterInput = new CM_PROCESS_ENTITY
        filterInput.conditioN_STATUS = this.status
        filterInput.reQ_ID = this.reQ_ID
        filterInput.procesS_ID = this.procesS_ID
        filterInput.brancH_ID = this.Branch_id
        filterInput.deP_ID = this.Dept_id
        if (this.status) {
            this.processService.cM_PROCESS_Current_Search(filterInput).subscribe(response => {
                let listProcess = new Array<CM_PROCESS_ENTITY>();
                // let index = 1;
                listProcess = response.items.filter(proc => proc.describe != null);
                this.tableCurrentProcessLog.setList(listProcess);
            });
        }
    }
}
