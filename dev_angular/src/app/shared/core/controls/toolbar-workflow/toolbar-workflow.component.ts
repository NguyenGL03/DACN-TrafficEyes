import { ChangeDetectorRef, Component, EventEmitter, Injector, Input, Output, ViewChild } from "@angular/core";
import { ActionRole } from "@app/utilities/enum/action-role";
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { IUiActionWorkflow } from "@app/utilities/ui-action-workflow";
import { CM_PROCESS_ENTITY, ProcessServiceProxy } from "@shared/service-proxies/service-proxies";
import { RejectModalComponentWf } from "../reject-modal-wf/reject-modal-wf.component";
import { ToolbarComponent } from "../toolbar/toolbar.component";

interface ApproveGroupEventEmitter {
    isApproveGroup: boolean;
    listProcess: CM_PROCESS_ENTITY[];
}
@Component({
    selector: 'app-toolbar-workflow',
    templateUrl: './toolbar-workflow.component.html',
    styleUrls: ['./toolbar.component.css']
})
export class ToolbarWorkFlowComponent extends ToolbarComponent {

    @ViewChild("rejectModal") rejectModalWF: RejectModalComponentWf;

    @Output() getProcess: EventEmitter<CM_PROCESS_ENTITY[]> = new EventEmitter<CM_PROCESS_ENTITY[]>();
    @Output() getCurrentProcess: EventEmitter<CM_PROCESS_ENTITY> = new EventEmitter<CM_PROCESS_ENTITY>();
    @Output() loadProcess: EventEmitter<any> = new EventEmitter<any>();
    @Output() rejectProcess: EventEmitter<any> = new EventEmitter<any>();
    @Output() loadApproveGroup: EventEmitter<ApproveGroupEventEmitter> = new EventEmitter<ApproveGroupEventEmitter>();

    @Input() REQ_ID: string;

    buttonRejectEnable: boolean;
    buttonRejectVisible: boolean;
    buttonRejectHidden: boolean;
    buttonAccessEnable: boolean;
    buttonAccessVisible: boolean;
    buttonAccessHidden: boolean;

    item: any;
    process: CM_PROCESS_ENTITY;
    listProcess: CM_PROCESS_ENTITY[];
    isEnd: boolean = false;
    isApproveGroup: boolean = true;
    processKey: string[] = [];

    constructor(injector: Injector, private processService: ProcessServiceProxy, private cdrWorkflow: ChangeDetectorRef) {
        super(injector, cdrWorkflow);
    }

    get uiActionWorkFlow(): IUiActionWorkflow<any> {
        return this.uiAction as IUiActionWorkflow<any>;
    }

    setProcessKey(...processKey: string[]) {
        this.processKey = processKey;
    }

    setRole(funct: string, add: boolean, edit: boolean, update: boolean, del: boolean, view: boolean, search: boolean, approve: boolean, resetSearch: boolean, reject?: boolean, access?: boolean, sendApp?: boolean) {
        this.funct = funct;
        this.setButtonAddVisible(add
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Create)
        );
        this.setButtonUpdateVisible(edit
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Edit)
        );
        this.setButtonSaveVisible(update
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Update)
        );
        this.setButtonDeleteVisible(del
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Delete)
        );
        this.setButtonViewDetailVisible(view
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.View)
        );
        this.setButtonSearchVisible(search
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Search)
        );
        this.setButtonApproveVisible(approve
            && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Approve)
        );
        this.setButtonResetSearchVisible(true);
        if (reject) {
            this.setButtonRejectVisible(reject
                && this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Approve)
            );
        }
        if (sendApp) {
            this.setButtonSendAppVisible(sendApp
                && (this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Update)
                    || this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.Create)
                    || this.permission.isGranted(this.prefix + '.' + funct + '.' + ActionRole.View)));
        }
    }

    public setButtonApproveEnable(enable: boolean): void {
        super.setButtonApproveEnable(enable);
    }

    public setEnableForListPage(): void {
        super.setEnableForEditPage();
        this.setButtonRejectEnable(false);
        this.setButtonAccessEnable(true);
    }

    public setEnableForAddAction(): void {
        super.setEnableForEditPage();
        this.setButtonRejectEnable(false);
        this.setButtonAccessEnable(false);
        this.setButtonApproveEnable(false);
    }

    public setEnableForUpdateAction(): void {
        super.setEnableForEditPage();
        this.setButtonRejectEnable(false);
        this.setButtonAccessEnable(false);
        this.setButtonApproveVisible(false);
        this.buttonApproveHidden = true;
        this.setButtonApproveEnable(false);
    }

    public setEnableForAccessAction(): void {
        super.setEnableForEditPage();
        this.setButtonRejectEnable(false);
        this.setButtonAccessEnable(true);
        this.setButtonApproveEnable(false);
        this.setButtonSendAppEnable(true);
    }

    public setEnableForApproveAction(): void {
        super.setEnableForViewDetailPage();
        this.setButtonRejectEnable(true);
        this.setButtonAccessEnable(false);
        this.setButtonApproveVisible(true);
        this.setButtonApproveEnable(true);
    }

    public setEnableForRejectAction(): void {
        this.setButtonSaveEnable(true);
    }
    // reject
    setButtonRejectEnable(enable: boolean): void {
        // if (!this.buttonRejectVisible) {
        //     enable = false;
        // }
        this.buttonRejectEnable = enable;
    }

    setButtonRejectVisible(visible: boolean): void {
        this.buttonRejectVisible = visible;
    }

    setButtonRejecHidden(hidden: boolean): void {
        this.buttonRejectHidden = hidden;
    }

    // #region Func button toolbar

    // Function lưu 
    save(): void {
        super.save()
        let process = this.listProcess.find(x => x.action == ActionWorkflow.Create)
        if (process) {
            this.process = process
        }
    }

    // Function duyệt 
    approve(btnElmt?: any) {
        super.approve(btnElmt)
        let process = this.listProcess.find(x => x.action == ActionWorkflow.Approve)
        if (process) {
            this.process = process
        }
    }

    // Function gửi duyệt
    access(): void {
        if (!this.uiActionWorkFlow) return;
        let process = this.listProcess.find(x => x.action == ActionWorkflow.Access)
        this.process = process
        this.uiActionWorkFlow.onAccess(this.item, process);
    }

    // Function từ chối
    reject(): void {
        if (!this.uiActionWorkFlow) return;
        this.uiActionWorkFlow.onReject(this.item);
    }

    // #endregion Func button toolbar

    setButtonAccessEnable(enable: boolean): void {
        /*if (!this.buttonAccessVisible) {
            enable = false;
        }*/
        this.buttonAccessEnable = enable;
    }

    setButtonAccessVisible(visible: boolean): void {
        this.buttonAccessVisible = visible;
    }


    setButtonAccessHidden(hidden: boolean): void {
        this.buttonAccessHidden = hidden;
    }


    disableAllButton() {
        this.setButtonSaveEnable(false)
        this.setButtonSendAppEnable(false)
        this.setButtonSaveEnable(false)
        this.setButtonRejectEnable(false)
        this.setButtonApproveEnable(false)
    }


    // #region Modal Reject

    showRejectModal() {
        this.rejectModalWF.show(this.process, this.REQ_ID)
    }

    submitReject(process: CM_PROCESS_ENTITY) {
        this.processService.cM_PROCESS_DT_Reject(process.id, this.appSession.user.userName, this.REQ_ID, process.status, process.order, true, process.notes)
            .subscribe((response) => {
                if (response["Result"] != '0') {
                    this.rejectProcess.emit(false);
                } else {
                    this.rejectProcess.emit(true);
                    this.setButtonRejectEnable(false);
                    this.setButtonSaveEnable(false);
                    this.setButtonAccessEnable(false);
                    this.setButtonApproveEnable(false);
                }
            });
    }

    // #endregion Modal Reject


    // #region Func cập nhật quy trình

    insertNextProcess(reqId: string, isDone: boolean, isReject: boolean) {
        if (!this.process?.procesS_ID) return;
        this.showLoading();
        this.processService.cM_PROCESS_DT_Insert(this.process.procesS_ID.toString(), this.appSession.user.userName, reqId)
            .subscribe((response) => {
                if (response["Result"] != '0') return;
                this.disableAllButton();
                this.loadProcess.emit(true);
                this.hideLoading();
            });
    }

    insertUpdateProcess(reqId: string, isReject: boolean = false) {
        if (!this.process?.procesS_ID) return;
        this.showLoading();

        this.processService.cM_PROCESS_DT_Update(this.process.procesS_ID.toString(), this.appSession.user.userName, reqId, isReject || false)
            .subscribe((response) => {
                if (response["Result"] != '0') return;
                this.setButtonRejectEnable(false);
                this.loadProcess.emit(true);
                this.hideLoading();
            });
    }

    // #endregion Func cập nhật quy trình


    // #region Get Workflow

    /**
    * Lấy thông tin quy trình tiếp theo.
    * Dựa vào thông tin process trả về để hiển thị giao diện.
    * Table: CM_PROCESS, CM_REQUEST_PROCESS
    *
    * @param {string} processKey - ProcessKey trong table CM_PROCESS
    * @param {string} userName - UserName
    * @param {string} status - Trạng thái hiện tại
    * @param {string} reqId - Thông tin ID phiếu
    * @param {EditPageState} editPageState - Trang hiện tại. Example: Add, Edit, ViewDetail
    */
    getWorkFlow(processKey: string, userName: string, status: string, reqId: string, editPageState: EditPageState) {
        if (!reqId) {
            this.getWorkFlowCreatPage(processKey, userName, editPageState)
            return;
        }

        const filterInput: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY();

        const statusMap = {
            [EditPageState.edit]: 'EDIT',
            [EditPageState.viewDetail]: 'VIEW'
        };

        filterInput.froM_STATUS = statusMap[editPageState] || filterInput.froM_STATUS;

        filterInput.tlname = userName;
        filterInput.procesS_KEY = processKey;
        filterInput.conditioN_STATUS = status;
        filterInput.reQ_ID = reqId;

        if (status == 'R') this.isEnd = true;

        this.processService.cM_PROCESS_Search(filterInput).subscribe(response => {
            this.listProcess = response.items;
            this.setAction(this.listProcess, editPageState);
        });
    }

    /**
    * Lấy thông tin quy trình bắt đầu.
    * Sử dụng tại trang thêm mới.
    * Table: CM_PROCESS
    *
    * @param {string} processKey - ProcessKey trong table CM_PROCESS
    * @param {string} userName - UserName 
    * @param {EditPageState} editPageState - Trang hiện tại. Example: Add
    */
    getWorkFlowCreatPage(processKey: string, userName: string, editPageState: EditPageState) {
        const filterInput: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY();
        filterInput.froM_STATUS = 'ADD';
        filterInput.tlname = userName;
        filterInput.procesS_KEY = processKey;
        this.processService.cM_PROCESS_Check_create(filterInput).subscribe(response => {
            this.listProcess = response.items;
            this.process = this.listProcess[0];
            this.setAction(this.listProcess, editPageState);
        });
    }

    // #endregion Get Workflow


    // #region Function Set Action

    /**
    * Function lấy danh sách field cần hidden khỏi giao diện theo quy trình hiện tại.
    * Cập nhật buttons toolbar.
    * Emmit events getProcess.
    * @param {CM_PROCESS_ENTITY[]} listProcess - Danh sách quy trình tiếp theo có thể thực hiện
    * @param {EditPageState} editPageState - Trang hiện tại. Example: Add, Edit, ViewDetail
    */
    public setAction(listProcess: CM_PROCESS_ENTITY[], editPageState: EditPageState): void {
        let process = this.process = null;
        this.isApproveGroup = true;
        listProcess = listProcess.filter(p => this.hasProcessKey(p.procesS_KEY));

        switch (editPageState) {
            case EditPageState.viewDetail:
                process = listProcess.find(x => x.action === ActionWorkflow.Approve)
                if (process && !this.isEnd) {
                    this.setEnableForApproveAction();
                    break;
                }
                process = listProcess.find(x => x.action === ActionWorkflow.ApproveGroup || x.action === ActionWorkflow.Done)
                if (process) {
                    this.isApproveGroup = false;
                    this.disableAllButton();
                    break;
                }
                process = listProcess.find(x => x.action === ActionWorkflow.Reject);
                if (process) {
                    this.setEnableForRejectAction();
                    break;
                }
                break;
            case EditPageState.add:
                process = listProcess.find(x => x.action === ActionWorkflow.Create);
                if (process) {
                    this.setEnableForAddAction();
                    break;
                }
                process = listProcess.find(x => x.action === ActionWorkflow.Access);
                if (process) {
                    this.buttonSendAppVisible = true;
                    this.setEnableForAccessAction();
                    break;
                }
                break;
            case EditPageState.edit:
                process = listProcess.find(x => x.action === ActionWorkflow.Access);
                if (process) {
                    this.setEnableForAccessAction();
                    break;
                }
                process = listProcess.find(x => x.action === ActionWorkflow.Reject);
                if (process) {
                    this.setEnableForRejectAction();
                    break;
                }
                break;
            default:
                break;
        }
        this.process = process;
        this.loadApproveGroup.emit({ isApproveGroup: this.isApproveGroup, listProcess: this.listProcess });
        this.getProcess.emit(this.listProcess);
        this.getCurrentProcess.emit(this.process);
        //this.loadField(this.process);
    }

    // #endregion Function Set Action


    // #region Utils Function

    /**
    * Function lấy danh sách field cần hidden khỏi giao diện theo quy trình hiện tại
    *
    * @param {CM_PROCESS_ENTITY} process - Quy trình tiếp theo
    */
    loadField(process: CM_PROCESS_ENTITY) {
        if (process) {
            this.hiddenField(process.hiddeN_FIELD);
        } else {
            this.processService.cM_PROCESS_GetHiddenField(this.appSession.user.userName, this.REQ_ID)
                .subscribe(response => {
                    if (response != null) {
                        this.hiddenField(response.result);
                    }
                });
        }
    }

    /**
    * Function xử lý Hidden fields khỏi giao diện
    *
    * @param {CM_PROCESS_ENTITY} process - Quy trình tiếp theo
    */
    hiddenField(listField: string) {
        if (listField != null) {
            let listFields = listField.split(',')
            listFields.forEach(x => {
                const btn = document.getElementById(x);
                if (btn != null) {
                    btn.setAttribute('style', 'display:none !important');
                }
                //btn.style.display='none';

            })
        }
    }

    hasProcessKey(processKey: string): boolean {
        return this.processKey?.length <= 0 || !!this.processKey.find(p => processKey === p);
    }
    // #endregion Utils Function
}

export enum ActionWorkflow {
    Create = 'CREATE',
    Access = 'ACCESS',
    Approve = 'APPROVE',
    ApproveGroup = 'APPROVE_GROUP',
    Done = 'DONE',
    Reject = 'REJECT',
} 
