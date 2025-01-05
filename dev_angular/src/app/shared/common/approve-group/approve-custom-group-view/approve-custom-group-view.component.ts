
import { ChangeDetectorRef, Component, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ApproveGroupServiceProxy, CM_APPROVE_GROUP, CM_TEMPLATE_NOTE, TL_USER_ENTITY, TlUserServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { Moment } from 'moment';
import { finalize } from 'rxjs/operators';
import { ApproveModalComponent } from '../../modals/approve-modal/approve-modal.component';
import { ProcessHistoryViewComponent } from '../../process-history-view/process-history-view.component';
import { ApproveAuthorityModalComponent } from '../approve-authority-modal/approve-authority-modal.component';
import { ApproveRejectModalComponent } from '../approve-reject-modal/approve-reject-modal.component';

@Component({
    selector: 'approve-custom-group-view',
    templateUrl: './approve-custom-group-view.component.html',
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => ApproveCustomGroupViewComponent),
        multi: true
    }]
})
export class ApproveCustomGroupViewComponent extends AppComponentBase {
    @ViewChild('rejectModal') rejectModal: ApproveRejectModalComponent
    @ViewChild('approveModal') approveModal: ApproveModalComponent;
    @ViewChild('authorityModal') authorityModal: ApproveAuthorityModalComponent;

    disabled: boolean;
    isAddNote: boolean = false;
    showGroup: boolean = true;
    hiddenContent: boolean = false;
    title: string;
    note: string;
    tlUsers: TL_USER_ENTITY[];
    notes: CM_TEMPLATE_NOTE[];
    _reqId: string;
    _ngModel: CM_APPROVE_GROUP[];
    approveGroups: CM_APPROVE_GROUP[];
    titlE_NAME = [];
    loading: boolean[] = [];

    @Input() location: string;
    @Input() historyView: ProcessHistoryViewComponent;
    @Output() approval: EventEmitter<any> = new EventEmitter<any>();
    @Output() handleReject: EventEmitter<any> = new EventEmitter<any>();

    @Input() set reqId(value: string) {
        this._reqId = value;
    }

    @Input() @Output() public set ngModel(value) {
        if (value) {
            this._ngModel = value;
            this.approveGroups = [];
            // Lọc ra danh sách nhóm duyệt trừ người ủy quyền
            let numStep = Math.max(...value.map((o: { steP_LEVEL: any; }) => o.steP_LEVEL));
            for (let i = 1; i <= numStep; i++) {
                this.approveGroups.push(
                    value.filter((x: { steP_LEVEL: number; type: string; iS_AUTHORITY: string; }) => x.steP_LEVEL == i && x.type != 'AUT')
                );
                this.titlE_NAME.push(
                    value.filter((x: { steP_LEVEL: number; type: string; }) => x.steP_LEVEL == i && x.type != 'AUT').map((o: { titlE_NAME: any; }) => o.titlE_NAME).distinct()
                );
            }
            const now = new Date(moment().format('YYYY-MM-DD')).getTime();
            const item = value.find((x: {
                approvE_USERNAME: string;
                authoritY_NAME: string;
                done: boolean;
                procesS_STATUS: boolean;
                autH_FROM_DATE: Moment;
                autH_TO_DATE: Moment
            }) => {
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
        if (this._ngModel.find(x => x.approvE_USERNAME == this.appSession.user.userName)) {
            return true;
        }
        return false;
    }

    constructor(
        injector: Injector,
        private cdr: ChangeDetectorRef,
        private tlUserService: TlUserServiceProxy,
        private approveGroupService: ApproveGroupServiceProxy
    ) {
        super(injector);
        // this.getUser();
    }

    onApprove() {
        if (this.disabled) return;
        this.approveModal.item = "Đồng ý";
        this.approveModal.show();
    }

    approve(event: { item: string; }) {
        if (this._reqId) {
            let note = '';
            if (event.item) {
                note = event.item;
            }
            this.message.confirm(
                this.l('Xác nhận duyệt?'),
                this.l('AreYouSure'),
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        this.approveGroupService.cM_APPROVE_GROUP_App(this._reqId, this.location, note)
                            .pipe(finalize(() => { abp.ui.clearBusy(); }))
                            .subscribe((response) => {
                                if (response['Result'] != '0') {
                                    // this.showErrorMessage(response['ErrorDesc']);
                                } else {
                                    this.approval.emit(response);
                                    // this.showSuccessMessage(this.l('Phê duyệt thành công'));
                                    this.getGroupApprove();
                                    this.getProcessHistory();
                                }
                            });
                    }
                }
            );
        }
    }

    // getUser() {
    //     let filterCombobox = this.getFillterForCombobox();
    //     this.tlUserService.tL_USER_GET_List(filterCombobox).subscribe(response => {
    //         this.tlUsers = response;
    //     });
    // }

    onReject() {
        if (this.disabled) {
            return;
        }
        this.rejectModal.RefreshTitle(this.l('Nội dung từ chối phiếu'));
        this.rejectModal.RefreshButtonContent(this.l("Từ chối"));
        this.rejectModal.InitAndShow("Cancel");
        this.rejectModal.show();
    }

    reject(event: { notes: string; }) {
        if (this._reqId) {
            this.message.confirm(
                this.l('Xác nhận từ chối?'),
                this.l('AreYouSure'),
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        this.approveGroupService.cM_APPROVE_GROUP_Reject(this._reqId, event.notes || '')
                            .pipe(finalize(() => { abp.ui.clearBusy(); }))
                            .subscribe((response) => {
                                if (response.result != '0') {
                                    // this.showErrorMessage(response.errorDesc);
                                } else {
                                    console.log(this.handleReject)
                                    if (this.handleReject) this.handleReject.emit(response);
                                    // this.showSuccessMessage(this.l('Từ chối thành công'));
                                    this.getGroupApprove();
                                    this.getProcessHistory();
                                }
                            });
                    }
                }
            );
        }
    }

    onAuthority() {
        if (this.disabled) return;
        this.authorityModal.RefreshTitle(this.l('Người tiếp nhận ủy quyền phiếu'));
        this.authorityModal.RefreshButtonContent(this.l('Ủy quyền'));
        this.authorityModal.InitAndShow("Cancel");
        this.authorityModal.show();
    }

    authority(event: { item: string; }) {
        if (this._reqId) {
            this.message.confirm(
                this.l('Xác nhận gửi duyệt'),
                this.l('AreYouSure'),
                (isConfirmed) => {
                    if (isConfirmed) {
                        // abp.ui.setBusy();
                        this.approveGroupService.cM_APPROVE_GROUP_Authority(this._reqId, event.item)
                            .pipe(finalize(() => {
                                // abp.ui.clearBusy();
                            }))
                            .subscribe((response) => {
                                if (response.result != '0') {
                                    // this.showErrorMessage(response.errorDesc);
                                }
                                else {
                                    // this.showSuccessMessage(this.l('Ủy quyền thành công'));
                                    this.getGroupApprove();
                                    this.getProcessHistory();
                                }
                            });
                    }
                }
            );
        }
    }

    onShowGroup() {
        this.showGroup = !this.showGroup;
    }

    getGroupApprove() {
        this.approveGroupService.cM_APPROVE_GROUP_WORKFLOW_ById(this._reqId).pipe(finalize(() => { abp.ui.clearBusy(); })).subscribe(response => {
            this.ngModel = response;
        });
    }

    getNotes() {
        this.approveGroupService.cM_APPROVE_GROUP_NOTE_ById(this._reqId).subscribe(result => {
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
        if (!this.note) {
            // this.showErrorMessage(this.l('Nhập ghi chú trước khi lưu'));
            return;
        }
        if (!this._reqId) return;

        const inputNote = new CM_TEMPLATE_NOTE();
        // inputNote.templatE_NOTE_USERNAME = this.appSession.user.userName;
        inputNote.content = this.note;
        inputNote.reQ_ID = this._reqId;
        this.approveGroupService.cM_APPROVE_GROUP_NOTE_Ins(inputNote).subscribe((response) => {
            if (response.result != '0') {
                // this.showErrorMessage(response.errorDesc);
            } else {
                // this.showSuccessMessage(this.l('Thêm ghi chú thành công'));
                this.getNotes();
                this.isAddNote = false;
                this.note = '';
            }
        });

    }

    onHiddenOrShow() {
        this.hiddenContent = !this.hiddenContent;
    }

    getProcessHistory() {
        // if (this.historyView) this.historyView.getHistoryProcess();
    }

    getApproveGroupView() {
        this.loading.push(true);
        this.approveGroupService.cM_APPROVE_GROUP_WORKFLOW_ById(this._reqId).subscribe(response => {
            this.ngModel = [...response];
            this.loading.pop();
        });
    }

    isNullOrEmpty(value: string) {
        if (value === null || value === undefined || value === '') {
            return true;
        }
        else {
            return false;
        }
    }

    propagateChange = (_: any) => { };

    onTouched: any = () => { };

    writeValue(value: any) { this.ngModel = value; }
    registerOnChange(fn: any) { this.propagateChange = fn; }
    registerOnTouched(fn: any) { this.onTouched = fn; }
    setDisabledState(isDisabled: boolean) { this.disabled = isDisabled; }
}
