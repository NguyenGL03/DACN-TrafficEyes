import { Component, Input, Output } from '@angular/core';
import { ActionWorkflow } from '@app/shared/core/controls/toolbar-workflow/toolbar-workflow.component';
import { CM_PROCESS_ENTITY } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'auth-status-workflow',
    templateUrl: './auth-status-workflow.component.html',
})
export class AuthStatusWorkflowComponent {
    @Input() authStatus: string;
    @Input() authStatusName: string;
    @Input() process: CM_PROCESS_ENTITY;

    action = ActionWorkflow;
    _listProcess: CM_PROCESS_ENTITY[];

    @Input() @Output() public set listProcess(listProcess: CM_PROCESS_ENTITY[]) {
        if (listProcess && listProcess.length > 0)
            this.process = listProcess.find(process => process.action != this.action.Create);
        this._listProcess = listProcess;
    }
}
