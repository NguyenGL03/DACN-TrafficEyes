
import { ChangeDetectorRef, Component, EventEmitter, Injector, Input, OnInit, Output, ViewEncapsulation, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { LazyDropdownResponse, LazyDropdownState } from '@app/shared/core/controls/dropdown-lazy-control/dropdown-lazy-control.component';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { APPROVE_GROUP_ENTITY, ApproveGroupServiceProxy, CM_DEPARTMENT_ENTITY, CM_TITLE_ENTITY, DepartmentServiceProxy, SysParametersServiceProxy, TL_USER_ENTITY, TitleServiceProxy, TlUserServiceProxy } from '@shared/service-proxies/service-proxies';
import { lastValueFrom } from 'rxjs';
import { finalize } from 'rxjs/operators';

interface ApproveGroup {
    id: string;
    title?: string;
    selectedUser?: TL_USER_ENTITY;
    users: TL_USER_ENTITY[];
}

@Component({
    selector: 'approve-custom-group',
    templateUrl: './approve-custom-group.component.html',
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => ApproveCustomGroupComponent),
        multi: true
    }]
})
export class ApproveCustomGroupComponent extends AppComponentBase implements ControlValueAccessor, OnInit {

    _ngModel: string[];
    _title: string[];
    _depId: string;

    groups: ApproveGroup[] = [];
    tlUsers: TL_USER_ENTITY[] = [];
    departments: CM_DEPARTMENT_ENTITY[];
    titles: CM_TITLE_ENTITY[];
    loading: boolean[] = [];

    statusSendApprove: string[];
    statusHSApprove: string[];
    hiddenContent: boolean = false;
    lazyDropdownState: LazyDropdownState;

    @Input() reqId: string;
    @Input() authStatus: string;
    @Input() disabled: boolean;
    @Input() valueMember: string = 'tlnanme';
    @Input() displayMember: string = 'tlnanme|tlFullName';
    @Input() valueTitle: string = 'title_name';
    @Input() titleGroup: string = 'Nhóm duyệt';
    @Input() isTitle: boolean = false;
    @Input() isShowDept: boolean = true;
    @Input() isHiddenButton: boolean = false;
    @Input() isHiddenButtonSend: boolean = false;
    @Input() titleType: string;

    @Input() @Output()
    public set title(value: any) {
        this.titleChange.emit(value);
        this._title = value || [];
    }

    public get title() {
        return this._title;
    }

    @Input() @Output()
    public set ngModel(value) {
        this._ngModel = value || []; 
        this.setGroups(this._ngModel || []);
    }

    public get ngModel(): any {
        return this._ngModel;
    }

    @Input() @Output()
    public set depId(val: string) {
        this.depIdChange.emit(val);
        this._depId = val;
    }
    public get depId() {
        return this._depId;
    }

    @Output() titleChange: EventEmitter<string[]> = new EventEmitter<string[]>();
    @Output() depIdChange: EventEmitter<string> = new EventEmitter<string>();
    @Output() onSendChange: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSendProfessional: EventEmitter<any> = new EventEmitter<any>();

    get isStepCN() {
        if (!this.isShowDept) return false;
        if (this.reqId) {
            if (this.statusHSApprove) { //['A', 'R', 'S', 'O']
                //authStatus != 'A' && authStatus != 'R' && authStatus != 'S' && authStatus != 'O'
                return !this.statusHSApprove.find(x => this.authStatus == x);
            } else {
                return true;
            }
        }
        return true;
    }

    get isHiddenSendApp() {
        if (this.reqId) {
            if (this.statusSendApprove) {
                //['A', 'R'] authStatus != 'A' && authStatus != 'R'
                return !this.statusSendApprove.find(x => this.authStatus == x);
            } else {
                return true;
            }
        }
        return true;
    }

    constructor(
        injector: Injector,
        private changeDetectorRef: ChangeDetectorRef,
        private tlUserService: TlUserServiceProxy,
        private departmentService: DepartmentServiceProxy,
        private titleService: TitleServiceProxy,
        private approveGroupService: ApproveGroupServiceProxy,
        private sysParametersService: SysParametersServiceProxy
    ) {
        super(injector);
        this.getStatusHSApprove();
        this.getStatusSendApprove();
        this.getTitle();
        this.getUser();
        this.getDep();
    }

    ngOnInit() {
    }

    ngAfterViewInit() {
        this.changeDetectorRef.detectChanges();
    }

    onTouched: any = () => { };
    propagateChange = (_: any) => { };
    writeValue(value: any) { this.ngModel = value; }
    registerOnChange(fn: any) { this.propagateChange = fn; }
    registerOnTouched(fn: any) { this.onTouched = fn; }
    setDisabledState(isDisabled: boolean) { this.disabled = isDisabled; }

    getStatusHSApprove() {
        this.sysParametersService.sYS_PARAMETERS_ByParaKey('STATUS_HS_APPROVE').subscribe(response => {
            this.statusHSApprove = response.paraValue.split('|');
        });
    }

    getStatusSendApprove() {
        this.sysParametersService.sYS_PARAMETERS_ByParaKey('STATUS_SEND_APPROVE').subscribe(response => {
            this.statusSendApprove = response.paraValue.split('|');
        });
    }

    getFillterForCombobox(): any {
        return {
            maxResultCount: -1,
            recorD_STATUS: RecordStatusConsts.Active,
            autH_STATUS: AuthStatusConsts.Approve
        };
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

    getDep() {
        if (!this.isShowDept) return;
        this.departmentService.cM_DEPARTMENT_HS_List().subscribe(response => {
            this.departments = response;
        });

    }

    getTitle() {
        this.titleService.cM_TITLE_SEARCH(this.titleType).subscribe(response => {
            this.titles = response;
        })
    }

    onSelectUser(data: TL_USER_ENTITY, index: number): void {
        if (!data || !this.groups[index] || !this.groups[index].users) return;
        if (this.groups[index].users.find(user => user.tlnanme == data.tlnanme)) return;
        const user: any = { ...data };
        this.groups[index].users.push(user); 
    }

    //TODO - Thêm nhóm duyệt
    addGroup() {
        if (this.disabled) return;
        if (!this.ngModel) this.ngModel = [];

        const newGroup: ApproveGroup = {
            id: (this.groups.length + 1) + '',
            title: '',
            users: [],
            selectedUser: null
        }

        this.groups.push(newGroup); 
    }

    addTitle() {
        if (this.disabled) {
            return;
        }
        if (!this.title) {
            this.title = [];
        }
        let temp: string = '';
        this.title.push(temp);
    }

    removeGroup(index: number) {
        if (this.disabled) {
            return;
        }
        this.ngModel.splice(index, 1);
        if (this.isTitle) {
            this.title.splice(index, 1)
        } 
        this.groups.splice(index, 1);
    }

    removeUser(group: ApproveGroup, index: number) {
        group.users.splice(index, 1);
    }

    onSendApp() {
        if (this.reqId) {
            this.message.confirm(
                this.l('ConfirmSendApprove'),
                this.l('AreYouSure'),
                (isConfirmed) => {
                    if (isConfirmed) {
                        this.onSendProfessional.emit();
                    }
                }
            );
        }
    }

    sendApp() {
        abp.ui.setBusy();
        this.approveGroupService.cM_APPROVE_GROUP_SEND_App(this.reqId)
            .pipe(finalize(() => { abp.ui.clearBusy(); }))
            .subscribe((response) => {
                if (response.result != '0') {
                    // this.showErrorMessage(response.errorDesc);
                }
                else {
                    this.authStatus = response['attr1'];
                    // this.showSuccessMessage(this.l('Gửi duyệt thành công'));
                    this.onSendChange.emit();
                }
            });
    }

    onChangeDep() {
        this.depIdChange.emit(this._depId);
    }

    onChangeTitle() {
        this.titleChange.emit(this._title);
    }

    updateApproveGroup() {
        let approveGroup = new APPROVE_GROUP_ENTITY();
        this.populateNgModelWithTlnames();
        approveGroup.reQ_ID = this.reqId;
        approveGroup.grouP_APPROVE = this.ngModel;
        approveGroup.title = this.title;
        this.approveGroupService.cM_APPROVE_GROUP_NEW_Upd(approveGroup).subscribe(response => {
            if (response.result != '0') {
                // this.showErrorMessageApprove(response.errorDesc);
            } else {
                // this.removeMessageApprove();
            }
        });
    }

    getApproveGroup() {
        this.approveGroupService.cM_APPROVE_GROUP_BY_TITLE_ID(this.reqId).subscribe(response => {
            this.title = [];
            this.title.push(...response);
        });
        this.approveGroupService.cM_APPROVE_GROUP_NEW_ById(this.reqId).subscribe(response => {
            this._ngModel = [];
            this._ngModel.push(...response);
            this.setGroups(response);
        });
    }

    generateUUID() {
        var d = new Date().getTime();
        if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
            d += performance.now();
        }
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
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

    addApproveGroup() {
        const approveGroup = new APPROVE_GROUP_ENTITY();
        approveGroup.reQ_ID = this.reqId;
        approveGroup.grouP_APPROVE = this.getGroups();
        approveGroup.title = this._title;
        lastValueFrom(this.approveGroupService.cM_APPROVE_GROUP_NEW_Ins(approveGroup));
    }

    updApproveGroup() {
        let approveGroup = new APPROVE_GROUP_ENTITY();
        this.populateNgModelWithTlnames();
        approveGroup.reQ_ID = this.reqId;
        approveGroup.grouP_APPROVE = this.ngModel;
        approveGroup.title = this.title;
        lastValueFrom(this.approveGroupService.cM_APPROVE_GROUP_NEW_Upd(approveGroup))
    }

    populateNgModelWithTlnames(): void {
        const copiedGroup: ApproveGroup[] = [...this.groups];
        this.ngModel = copiedGroup
            .flatMap(group => group.users.map(user => user.tlnanme));
        this.groups = copiedGroup;
      }
}
