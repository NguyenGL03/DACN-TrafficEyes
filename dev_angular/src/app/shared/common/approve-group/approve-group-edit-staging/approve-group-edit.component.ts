
import { ChangeDetectorRef, Component, EventEmitter, Injector, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { LazyDropdownResponse } from '@app/shared/core/controls/dropdown-lazy-control/dropdown-lazy-control.component';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { APPROVE_GROUP_ENTITY, ApproveGroupServiceProxy, CM_DEPARTMENT_ENTITY, CM_TITLE_ENTITY, SysParametersServiceProxy, TL_USER_ENTITY, TitleServiceProxy, TlUserServiceProxy } from '@shared/service-proxies/service-proxies';
import { lastValueFrom } from 'rxjs';
import { finalize } from 'rxjs/operators';

interface ApproveGroup {
    id: string;
    title?: string;
    selectedUser?: TL_USER_ENTITY;
    users: TL_USER_ENTITY[];
}

@Component({
    selector: 'approve-group-edit',
    templateUrl: './approve-group-edit.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ApproveGroupEditComponent extends AppComponentBase implements OnInit {

    @Output() titleChange: EventEmitter<string[]> = new EventEmitter<string[]>();
    @Output() onSendChange: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSendProfessional: EventEmitter<any> = new EventEmitter<any>();

    @Input() disabled: boolean;
    @Input() valueMember: string = 'tlnanme';
    @Input() displayMember: string = 'tlnanme|tlFullName';
    @Input() titleGroup: string = 'Nhóm duyệt';
    @Input() isTitle: boolean = false;
    @Input() titleType: string;

    @Input() isHiddenButton: boolean = false;
    @Input() isHiddenButtonAdd: boolean = false;
    @Input() isHiddenButtonSend: boolean = false;

    @Input() inputModel: any;
    @Input() reqId: string;
    @Input() authStatus: string;

    _title: string[];

    groups: ApproveGroup[] = [];
    tlUsers: TL_USER_ENTITY[] = [];
    departments: CM_DEPARTMENT_ENTITY[];
    titles: CM_TITLE_ENTITY[];
    loading: boolean[] = [];

    statusSendApprove: string[];
    statusHSApprove: string[];
    hiddenContent: boolean = false;

    @Input() @Output()
    public set title(value: any) {
        this.titleChange.emit(value);
        this._title = value || [];
    }

    public get title() {
        return this._title;
    }

    get isHiddenSendApp() {
        if (!this.reqId) return true;
        if (!this.statusSendApprove) return true;
        return !this.statusSendApprove.find(x => this.authStatus == x);
    }

    constructor(
        injector: Injector,
        private changeDetectorRef: ChangeDetectorRef,
        private tlUserService: TlUserServiceProxy,
        private titleService: TitleServiceProxy,
        private approveGroupService: ApproveGroupServiceProxy,
        private sysParametersService: SysParametersServiceProxy
    ) {
        super(injector);
        this.initDefault();
        this.getTitle();
        this.getUser();
    }

    ngOnInit() {
    }

    ngAfterViewInit() {
        this.changeDetectorRef.detectChanges();
    }

    initDefault() {
        // this.sysParametersService.sYS_PARAMETERS_ByParaKey('STATUS_HS_APPROVE').subscribe(response => {
        //     this.statusHSApprove = response.paraValue.split('|');
        // });

        // this.sysParametersService.sYS_PARAMETERS_ByParaKey('STATUS_SEND_APPROVE').subscribe(response => {
        //     this.statusSendApprove = response.paraValue.split('|');
        // });
    }

    getFillterForCombobox(): any {
        return {
            maxResultCount: -1,
            recorD_STATUS: RecordStatusConsts.Active,
            autH_STATUS: AuthStatusConsts.Approve
        };
    }

    // #endregion Xử lý người duyệt
    onSelectUser(data: TL_USER_ENTITY, index: number): void {
        if (!data || !this.groups[index] || !this.groups[index].users) return;
        if (this.groups[index].users.find(user => user.tlnanme == data.tlnanme)) return;
        const user: any = { ...data };
        this.groups[index].users.push(user);
    }

    removeUser(group: ApproveGroup, index: number) {
        group.users.splice(index, 1);
    }

    //TODO - Sử dụng lazy loading để giới hạn số lượng user được load
    getUser(data?: LazyDropdownResponse): void {
        if (!data) return;
        const filterInput: TL_USER_ENTITY = new TL_USER_ENTITY();
        filterInput.skipCount = data.state?.skipCount;
        filterInput.totalCount = data.state?.totalCount;
        filterInput.maxResultCount = data.state?.maxResultCount;
        filterInput.tlFullName = data.state?.filter;
        this.tlUserService.tL_USER_GET_List_v2(filterInput)
            .subscribe(result => {
                data.callback(result)
            })
    }
    // #endregion Xử lý người duyệt

    // #region Xử lý title nhóm duyệt
    addTitle() {
        if (this.disabled) return;
        if (!this.title) this.title = [];
        this.title.push('');
    }

    getTitle() {
        this.titleService.cM_TITLE_SEARCH(this.titleType).subscribe(response => {
            this.titles = response;
        })
    }

    onChangeTitle() {
        this.titleChange.emit(this._title);
    }
    // #endregion Xử lý title nhóm duyệt

    // #region Xử lý nhóm duyệt
    addGroup() {
        if (this.disabled) return;
        this.groups.push({
            id: (this.groups.length + 1) + '',
            title: '',
            users: [],
            selectedUser: null
        });
    }

    removeGroup(index: number) {
        if (this.disabled) return;
        if (this.isTitle) this.title.splice(index, 1)
        this.groups.splice(index, 1);
    }

    getGroups(): string[] {
        return this.groups.map(group => {
            return group.users.map(user => user.tlnanme).join(';')
        });
    }

    setGroups(users: string[]) {
        this.groups = [];
        users.forEach(user => {
            if (!user) return;

            const list: TL_USER_ENTITY[] = user.split(';').map(item => {
                const value = item.split(' - ');
                const tlUser: TL_USER_ENTITY = new TL_USER_ENTITY();
                tlUser.tlnanme = value[0];
                tlUser.tlFullName = value[1];
                return tlUser;
            })

            const group: ApproveGroup = {
                id: this.generateUUID(),
                users: list
            }
            this.groups.push(group);
        })
    }
    // #endregion Xử lý nhóm duyệt

    // #region Xử lý api nhóm duyệt
    getApproveGroup(reqId: string) {
        this.approveGroupService.cM_APPROVE_GROUP_BY_TITLE_ID(reqId).subscribe(response => {
            this.title = [...response];
        });
        this.approveGroupService.cM_APPROVE_GROUP_NEW_ById(reqId).subscribe(response => {
            this.setGroups(response);
        });
    }

    addApproveGroup(reqId: string) {
        const approveGroup = new APPROVE_GROUP_ENTITY();
        approveGroup.reQ_ID = reqId;
        approveGroup.grouP_APPROVE = this.getGroups();
        approveGroup.title = this._title;
        lastValueFrom(this.approveGroupService.cM_APPROVE_GROUP_NEW_Ins(approveGroup));
    }

    updateApproveGroup(reqId: string) {
        let approveGroup = new APPROVE_GROUP_ENTITY();
        approveGroup.reQ_ID = reqId;
        approveGroup.grouP_APPROVE = this.getGroups();
        approveGroup.title = this.title;
        lastValueFrom(this.approveGroupService.cM_APPROVE_GROUP_NEW_Upd(approveGroup))
    }

    sendApp(reqId: string) {
        this.approveGroupService.cM_APPROVE_GROUP_SEND_App(reqId)
            .pipe(finalize(() => { abp.ui.clearBusy(); }))
            .subscribe((response) => {
                if (response.result != '0') return;
                this.authStatus = response['attr1'];
                this.onSendChange.emit();
            });
    }

    onSendApp() {
        if (!this.reqId) return;
        this.message.confirm(
            this.l('ConfirmSendApprove'),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (!isConfirmed) return
                this.onSendProfessional.emit();
            }
        );
    }
    // #endregion Xử lý api nhóm duyệt
}
