import { Component, ViewChild } from '@angular/core';
import { CurrentProcessWorkflowComponent } from '@app/shared/core/controls/current-process-workflow/current-process-workflow.component';
import { ToolbarWorkFlowComponent } from '@app/shared/core/controls/toolbar-workflow/toolbar-workflow.component';
import { CM_PROCESS_ENTITY } from '@shared/service-proxies/service-proxies';
import { DefaultComponentBase } from './default-component-base';

@Component({ template: '' })
export abstract class DefaultComponentWorkflow extends DefaultComponentBase {
    @ViewChild('currentProcessView') currentProcessView: CurrentProcessWorkflowComponent; // Bước xử lý hiện tại

    get appToolbarWF(): ToolbarWorkFlowComponent {
        return this.appToolbar as ToolbarWorkFlowComponent;
    }

    // #region Event emitter AppToolbar 
    loadProcess(event: any): void { }
    rejectProcess(event: any): void { }
    getProcess(listProcess: CM_PROCESS_ENTITY[]): void { }
    getCurrentProcess(process: CM_PROCESS_ENTITY): void {
        if(!process) return;
		this.approveGroupComponent.process = process;
    }
    // #endregion Event emitter AppToolbar 

    public subscribeEventAppToolbar() {
        if (!this.appToolbarWF) return;

        this.appToolbarWF.loadProcess.subscribe(event => this.loadProcess(event));
        this.appToolbarWF.getProcess.subscribe(event => this.getProcess(event));
        this.appToolbarWF.getCurrentProcess.subscribe(event => this.getCurrentProcess(event));
        this.appToolbarWF.rejectProcess.subscribe(event => this.rejectProcess(event));
    }

    public subscribeEventApproveGroup() {
        if (!this.appToolbarWF) return;

        this.appToolbarWF.loadApproveGroup.subscribe(event => this.loadApproveGroup(event));
        this.approveGroupComponent.onUpdateProcess.subscribe(event => this.appToolbarWF.insertNextProcess(event.reqId, event.isDone, event.isReject));
        this.approveGroupComponent.onRejectProcess.subscribe(event => this.appToolbarWF.submitReject(event));
    }

    // Function load approve 
    loadApproveGroup(event: { isApproveGroup: boolean, listProcess: CM_PROCESS_ENTITY[] }) {
        // listProcess empty có 2 trường hợp:
        // TH 1: User hiện tại không có trong quy trình tiếp theo
        // TH 2: Phiếu đã hoàn tất
        this.approveGroupComponent.isApproveGroup = !!event.isApproveGroup && (event.listProcess?.length > 0);
        this.approveGroupComponent.updateView();
        this.approveGroupComponent.loadDataByProcess(event.listProcess);
    }

    approveSuccess() {
        super.approveSuccess

        if (this.appToolbarWF) return;

        this.appToolbarWF.setButtonApproveEnable(false);
        this.appToolbarWF.setButtonRejectEnable(false);
    }

    set saving(value: boolean) {
        if (value) abp.ui.setBusy();
        if (!value) abp.ui.clearBusy();

        if (this.appToolbarWF) this.appToolbarWF.setEnable(!value);
    }
}
