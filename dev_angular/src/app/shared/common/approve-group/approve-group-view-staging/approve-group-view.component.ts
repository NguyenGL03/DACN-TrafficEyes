
import { Component, EventEmitter, forwardRef, Injector, Input, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ApproveGroupServiceProxy, CM_APPROVE_GROUP, CM_PROCESS_ENTITY, CM_TEMPLATE_NOTE, TL_USER_ENTITY } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { ApproveModalComponent } from '../../modals/approve-modal/approve-modal.component';
import { ProcessHistoryViewComponent } from '../../process-history-view/process-history-view.component';
import { ApproveAuthorityModalComponent } from '../approve-authority-modal/approve-authority-modal.component';
import { ApproveRejectModalComponent } from '../approve-reject-modal/approve-reject-modal.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { RejectModalComponentWf } from '@app/shared/core/controls/reject-modal-wf/reject-modal-wf.component';

@Component({
    selector: 'approve-group-view',
    templateUrl: './approve-group-view.component.html',
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => ApproveGroupViewComponent),
        multi: true
    }]
})
export class ApproveGroupViewComponent extends AppComponentBase {
    // @ViewChild('rejectModal') rejectModal: ApproveRejectModalComponent
    @ViewChild('approveModal') approveModal: ApproveModalComponent;
    @ViewChild('authorityModal') authorityModal: ApproveAuthorityModalComponent;
    @ViewChild('rejectModal') rejectModalWF: RejectModalComponentWf;

    @Input() reqId: string;
    @Input() location: string;
    @Input() historyView: ProcessHistoryViewComponent;
    @Input() process: CM_PROCESS_ENTITY;
    @Output() approval: EventEmitter<any> = new EventEmitter<any>();
    @Output() handleReject: EventEmitter<any> = new EventEmitter<any>();
    @Output() onUpdateProcess: EventEmitter<any> = new EventEmitter<any>();
    @Output() onRejectProcess: EventEmitter<any> = new EventEmitter<any>();

    disabled: boolean;
    isAddNote: boolean = false;
    showGroup: boolean = true;
    hiddenContent: boolean = false;
    title: string;
    note: string;
    tlUsers: TL_USER_ENTITY[];
    notes: CM_TEMPLATE_NOTE[];
    _ngModel: CM_APPROVE_GROUP[];
    approveGroups: CM_APPROVE_GROUP[];
    titlE_NAME = [];
    loading: boolean[] = [];

    @Input() @Output() public set ngModel(value) {
        if (value) {
            this._ngModel = value;
            this.approveGroups = [];
            // Lọc ra danh sách nhóm duyệt trừ người ủy quyền
            const numStep = Math.max(...value.map((o: { steP_LEVEL: any; }) => o.steP_LEVEL));
            for (let i = 1; i <= numStep; i++) {
                this.approveGroups.push(
                    value.filter((x: CM_APPROVE_GROUP) => x.steP_LEVEL == i && x.type != 'AUT')
                );
                this.titlE_NAME.push(
                    value
                        .filter((x: CM_APPROVE_GROUP) => x.steP_LEVEL == i && x.type != 'AUT')
                        .map((o: { titlE_NAME: any; }) => o.titlE_NAME)
                        .distinct()
                );
            }
            const now = new Date(moment().format('YYYY-MM-DD')).getTime();
            const item = value.find((x: CM_APPROVE_GROUP) => {
                return (
                    x.procesS_STATUS == true
                    && x.done == false
                    && (x.approvE_USERNAME == this.appSession.user.userName
                        || (
                            x.authoritY_NAME == this.appSession.user.userName
                            && (
                                (!x.autH_FROM_DATE && !x.autH_TO_DATE)
                                || (now >= new Date(x.autH_FROM_DATE.format('YYYY-MM-DD')).getTime()
                                    && now <= new Date(x.autH_TO_DATE.format('YYYY-MM-DD')).getTime())
                            )
                        )

                    )
                )
            })
            this.disabled = !item;
        } else {
            this._ngModel = [];
            this.approveGroups = [];
            this.disabled = false;
        }
    }

    public get ngModel(): any {
        return this._ngModel;
    }

    get isNoteEdit(): boolean {
        return !!this._ngModel.find(x => x.approvE_USERNAME == this.appSession.user.userName);
    }

    constructor(injector: Injector, private approveGroupService: ApproveGroupServiceProxy) {
        super(injector);
    }

    // #region Event Load data
    getApproveGroupView(reqId: string) {
        this.loading.push(true);
        this.approveGroupService.cM_APPROVE_GROUP_WORKFLOW_ById(reqId).subscribe(response => {
            this.ngModel = [...response];
            this.loading.pop();
        });
    }
    // #endregion Event Load data

    // #region Event Duyệt
    onApprove() {
        if (this.disabled) return;
        this.approveModal.item = "Đồng ý";
        this.approveModal.show();
    }

    approve(event: { item: string; }) {
        if (!this.reqId) return;

        const note = event.item || '';
        this.message.confirm(
            this.l('Xác nhận duyệt?'),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (!isConfirmed) return;

                this.approveGroupService.cM_APPROVE_GROUP_App(this.reqId, this.location, note)
                    .subscribe((response) => {
                        if (response['Result'] != '0') {
                            // this.showErrorMessage(response['ErrorDesc']);
                            return;
                        }
                        this.approval.emit(response);
                        this.onUpdateProcess.emit(response);
                        // this.showSuccessMessage(this.l('Phê duyệt thành công'));
                        this.getApproveGroupView(this.reqId);
                        this.getProcessHistory();
                    });
            }
        );
    }
    // #endregion Event Duyệt

    // #region Event Từ chối
    onReject() {
        if (this.disabled || !this.process) return;
        this.rejectModalWF.show(this.process, this.reqId);
    }

    reject(process: CM_PROCESS_ENTITY) {
        if (!this.reqId) return;

        this.message.confirm(
            this.l('Xác nhận từ chối?'),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (!isConfirmed) return;

                this.approveGroupService.cM_APPROVE_GROUP_Reject(this.reqId, '')
                    .subscribe((response) => {
                        if (response.result != '0') return;
                        this.handleReject.emit(response);
                        this.onRejectProcess.emit(process);
                        this.getApproveGroupView(this.reqId);
                        this.getProcessHistory();
                    });
            }
        );
    }
    // #endregion Event Từ chối

    // #region Event Ủy quyền
    onAuthority() {
        if (this.disabled) return;
        this.authorityModal.RefreshTitle(this.l('Người tiếp nhận ủy quyền phiếu'));
        this.authorityModal.RefreshButtonContent(this.l('Ủy quyền'));
        this.authorityModal.InitAndShow("Cancel");
        this.authorityModal.show();
    }

    authority(event: { item: string; }) {
        if (!this.reqId) return;

        this.message.confirm(
            this.l('Xác nhận gửi duyệt'),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (!isConfirmed) return;
                this.approveGroupService.cM_APPROVE_GROUP_Authority(this.reqId, event.item)
                    .subscribe((response) => {
                        if (response.result != '0') {
                            // this.showErrorMessage(response.errorDesc);
                            return;
                        }
                        this.getApproveGroupView(this.reqId);
                        this.getProcessHistory();
                    });
            }
        );
    }
    // #endregion Event Ủy quyền

    // #region Event Notes
    getNotes() {
        this.approveGroupService.cM_APPROVE_GROUP_NOTE_ById(this.reqId).subscribe(result => {
            this.notes = result;
        });
    }

    showAddNote() {
        this.isAddNote = true;
    }

    hideAddNote() {
        this.isAddNote = false;
    }

    addNote() {
        if (!this.note || !this.reqId) return;

        const inputNote = new CM_TEMPLATE_NOTE();
        inputNote.content = this.note;
        inputNote.reQ_ID = this.reqId;
        this.approveGroupService.cM_APPROVE_GROUP_NOTE_Ins(inputNote).subscribe((response) => {
            if (response.result != '0') {
                // this.showErrorMessage(response.errorDesc);
                return;
            }
            this.getNotes();
            this.isAddNote = false;
            this.note = '';
        });
    }
    // #endregion Event Notes

    onShowGroup() {
        this.showGroup = !this.showGroup;
    }

    onHiddenOrShow() {
        this.hiddenContent = !this.hiddenContent;
    }

    getProcessHistory() {
        // if (this.historyView) this.historyView.getHistoryProcess();
    }

    isNullOrEmpty(value: string): boolean {
        return value === null || value === undefined || value === '';
    }
}
