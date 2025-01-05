import { ChangeDetectorRef, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CM_PROCESS_ENTITY } from '@shared/service-proxies/service-proxies';
import { ApproveGroupEditComponent } from '../approve-group-edit-staging/approve-group-edit.component';
import { ApproveGroupViewComponent } from '../approve-group-view-staging/approve-group-view.component';
import { ActionWorkflow } from '@app/shared/core/controls/toolbar-workflow/toolbar-workflow.component';

@Component({
    selector: 'approve-group',
    templateUrl: './approve-group.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ApproveGroupComponent extends AppComponentBase {

    @Input() process: CM_PROCESS_ENTITY;
    @Input() reqId: string;
    @Input() inputModel: any;
    @Input() isApproveGroup: boolean;
    @Input() editGroupConfig: IApproveGroupEditConfig = new ApproveGroupEditConfig();
    @Input() viewGroupConfig: IApproveGroupViewConfig = new ApproveGroupViewConfig();

    @Output() onUpdateProcess: EventEmitter<{ reqId: string, isDone: boolean, isReject: boolean }> = new EventEmitter<{ reqId: string, isDone: boolean, isReject: boolean }>();
    @Output() onRejectProcess: EventEmitter<CM_PROCESS_ENTITY> = new EventEmitter<CM_PROCESS_ENTITY>();

    @ViewChild('approveGroupEdit') editGroup: ApproveGroupEditComponent;
    @ViewChild('approveGroupView') viewGroup: ApproveGroupViewComponent;

    isLoadingScreen: boolean = false;
    isLoadingData: boolean = false;

    constructor(injector: Injector, private cdr: ChangeDetectorRef) {
        super(injector)
    }

    updateView() {
        this.cdr.detectChanges();
    }

    /**
    * Function call api lấy dữ liệu nhóm duyệt theo process.
    * Call api theo isApproveGroup. 
    * True sử dụng giao diện edit nhóm duyệt.
    * False sử dụng giao diện view nhóm duyệt.
    * 
    * @param {CM_PROCESS_ENTITY[]} listProcess - Danh sách quy trình tiếp theo có thể thực hiện 
    */
    loadDataByProcess(listProcess: CM_PROCESS_ENTITY[]) {
        this.setActionApproveGroup(listProcess);
        // Trường hợp đang tạo hoặc không
        if (this.isApproveGroup) this.editGroup.getApproveGroup(this.reqId);
        else this.viewGroup.getApproveGroupView(this.reqId);
    }

    /**
    * Function cập nhật hành động cho nhóm duyệt theo process.
    * 
    * @param {CM_PROCESS_ENTITY[]} listProcess - Danh sách quy trình tiếp theo có thể thực hiện 
    */
    setActionApproveGroup(listProcess: CM_PROCESS_ENTITY[]) {
        if (!listProcess || listProcess.length <= 0) {
            this.editGroupConfig.disabled = true;
            this.editGroupConfig.isHiddenButton = true;
            return;
        }
        // Quy trình hiện tại không phải Create hoặc Access
        // Disable và ẩn các nút tương tác nhóm duyệt
        if (!listProcess.find(process => process.action === ActionWorkflow.Create || process.action === ActionWorkflow.Access)) {
            this.editGroupConfig.disabled = true;
            this.editGroupConfig.isHiddenButton = true;
            return;
        }
    }

    handleUpdateProcess(event: any) {
        if (!event || !event['IS_UPDATE_PROCESS']) return;
        this.onUpdateProcess.emit({ reqId: this.reqId, isDone: false, isReject: false });
    }

    handleRejectProcess(event: CM_PROCESS_ENTITY) {
        if (!event) return;
        this.onRejectProcess.emit(event);
    }
}


interface IApproveGroupEditConfig {
    disabled?: boolean;
    isTitle?: boolean;
    isHiddenButton?: boolean;
    isHiddenButtonAdd?: boolean;
    isHiddenButtonSend?: boolean;
    titleType: string;
}

interface IApproveGroupViewConfig {
    disabled?: boolean;
}

class ApproveGroupEditConfig implements IApproveGroupEditConfig {
    disabled?: boolean;
    isTitle?: boolean;
    isHiddenButton?: boolean;
    isHiddenButtonAdd?: boolean;
    isHiddenButtonSend?: boolean;
    titleType: string;

    constructor() {
        this.disabled = false;
        this.isTitle = false;
        this.isHiddenButton = false;
        this.isHiddenButtonAdd = false;
        this.isHiddenButtonSend = true;
    }
}

class ApproveGroupViewConfig implements IApproveGroupViewConfig {
    disabled?: boolean;
    isTitle?: boolean;
    isHiddenButton?: boolean;
    isHiddenButtonAdd?: boolean;
    isHiddenButtonSend?: boolean;

    constructor() {
        this.disabled = true;
        this.isTitle = true;
        this.isHiddenButton = true;
        this.isHiddenButtonAdd = true;
        this.isHiddenButtonSend = true;
    }
}
