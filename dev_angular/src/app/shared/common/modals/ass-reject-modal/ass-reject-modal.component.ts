
import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { PrimeTableComponent } from '@app/shared/core/controls/primeng-table/prime-table/prime-table.component';
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CM_REJECT_LOG_ENTITY, CM_REJECT_PROCESS_ENTITY, RejectServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'ass-reject-modal',
    templateUrl: './ass-reject-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class AssRejectModalComponent extends DefaultComponentBase implements OnInit, AfterViewInit {

    @Input() id: string;
    @Input() trN_TYPE: string;
    @Input() stage: string;
    @Input() isShowEnterRejectContent: boolean = false;
    @Input() rejectMessage: string;
    @Input() buttonRejectKT: boolean = false;
    @Input() rejectHC: boolean = false;
    @Input() autH_STATUS: AuthStatusConsts;
    @Input() isSendMail: boolean = false;
    @Input() isShowButtonRejectPrevious: boolean = false
    @Output() rejectLabel: EventEmitter<any> = new EventEmitter<any>();

    isShowError: boolean = true;
    rejectLogInput: CM_REJECT_LOG_ENTITY = new CM_REJECT_LOG_ENTITY();
    rejectLogInput2: CM_REJECT_LOG_ENTITY = new CM_REJECT_LOG_ENTITY();
    rejectLog: CM_REJECT_LOG_ENTITY[];

    @ViewChild('rejectTable') rejectTable: PrimeTableComponent<CM_REJECT_LOG_ENTITY>;

    constructor(injector: Injector, private rejectService: RejectServiceProxy) {
        super(injector);
    }
    ngAfterViewInit(): void {
        this.getReject();
        console.log(this.isShowEnterRejectContent);
    }

    ngOnInit() {

    }

    // Trả về Giao dịch viên
    onReject() {
        // KT trả về cho nhân viên kế toán
        // HC trả về cho nhân viên hành chính
        // KT_HC trả về cho người tạo từ bên KT
        this.rejectLogInput.trN_TYPE = this.trN_TYPE;
        this.rejectLogInput.stage = this.stage;
        this.rejectLogInput.trN_ID = this.id;
        this.rejectLogInput.iS_SEND_MAIL = this.isSendMail

        this.rejectLogInput.loG_DT = moment();
        this.rejectLogInput.rejecteD_DT = moment();
        this.rejectLogInput.rejecteD_BY = this.appSession.user.userName;
        this.rejectLogInput.reason = this.rejectLogInput.reason;
        if (!this.rejectLogInput.reason) {
            this.showErrorMessage(this.l('PleaseTypeReasonReturn'));
            return;
        }
        this.showLoading();
        this.isShowError = false;
        this.rejectService.cM_REJECT_Ins(this.rejectLogInput)
            .pipe(finalize(() => { this.hideLoading(); }))
            .subscribe((res) => {
                if (res.Result != '0') {
                    this.showErrorMessage(res['ErrorDesc']);
                } else {
                    this.showSuccessMessage(res.ErrorDesc);
                    this.autH_STATUS = AuthStatusConsts.Reject;
                    this.isShowEnterRejectContent = false;

                    this.getReject();
                    this.rejectLabel.emit(null);
                }
            });
    }

    // Trả về cho người tạo
    onReject_KT_HC() {
        this.rejectLogInput.trN_TYPE = this.trN_TYPE;
        if (this.rejectHC == true) {
            this.rejectLogInput.stage = 'HC';
            this.rejectLogInput.iS_LATEST = 'Y'
        } else {
            this.rejectLogInput.stage = 'KT_HC';
        }
        this.rejectLogInput.trN_ID = this.id;

        this.rejectLogInput.loG_DT = moment();
        this.rejectLogInput.rejecteD_DT = moment();
        this.rejectLogInput.rejecteD_BY = this.appSession.user.userName;
        this.rejectLogInput.reason = this.rejectLogInput.reason;
        this.rejectLogInput.iS_SEND_MAIL = this.isSendMail
        if (!this.rejectLogInput.reason) {
            this.showErrorMessage(this.l('PleaseTypeReasonReturn'));
            return;
        }
        this.isShowError = false;
        this.showLoading();
        this.rejectService.cM_REJECT_Ins(this.rejectLogInput)
            .pipe(finalize(() => { this.hideLoading() }))
            .subscribe((res) => {
                if (res.Result != '0') {
                    this.showErrorMessage(res['ErrorDesc']);
                } else {
                    this.showSuccessMessage(res.ErrorDesc);
                    this.autH_STATUS = AuthStatusConsts.Reject;
                    this.isShowEnterRejectContent = false;
                    this.rejectLabel.emit(null);
                    this.getReject();
                }
            });
    }

    @ViewChild('tableProcessLog') tableProcessLog: PrimeTableComponent<CM_REJECT_PROCESS_ENTITY>;

    getReject(): void {
        this.rejectService.cM_REJECT_PROCESS_Search(this.id, 'REJECT', this.appSession.user.userName).subscribe(response => {

            if (response.length > 0) {
                let rejectLasted = response[0];
                this.rejectLogInput2.lasteD_REASON = rejectLasted.procesS_DESC;
                this.rejectLogInput2.rejecteD_BY = rejectLasted.checkeR_NAME;
            }

            // this.tableProcessLog?.setAllRecords(response);
        });
    }

}
