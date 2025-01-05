import { ChangeDetectorRef, Component, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { NgForm } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CM_PROCESS_ENTITY, ProcessServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";

@Component({
    selector: "reject-modal-wf",
    templateUrl: "./reject-modal-wf.component.html",
    encapsulation: ViewEncapsulation.None
})
export class RejectModalComponentWf extends AppComponentBase {

    @ViewChild('rejectModal') modal: ModalDirective;
    @ViewChild('editForm') editForm: NgForm;

    @Output() onSubmitEvent: EventEmitter<CM_PROCESS_ENTITY> = new EventEmitter<CM_PROCESS_ENTITY>();
    @Output() onCancelEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onShowEvent: EventEmitter<any> = new EventEmitter<any>();

    @Input() title: string;
    @Input() reqId: string;

    notes: string = '';
    isShowError = false;
    waiting: boolean;
    active: boolean = false;
    list = [];


    constructor(injector: Injector, private processService: ProcessServiceProxy, private cdr: ChangeDetectorRef) {
        super(injector);
    }

    ngAfterViewInit() {
        this.editForm.statusChanges.subscribe(() => {
            this.editForm.dirty ? this.cdr.detectChanges() : !this.editForm.dirty;
        })
        this.editForm.valueChanges.subscribe(currentValue => {
            (JSON.stringify(currentValue) == '' || JSON.stringify(currentValue) == null || JSON.stringify(currentValue) == undefined) ? this.editForm.dirty : !this.editForm.dirty;
        })
    };

    show(process: CM_PROCESS_ENTITY, reQ_ID: string) {
        this.active = true;
        this.getListReject(process.conditioN_STATUS, process.procesS_ID, process.procesS_KEY, reQ_ID)
    }

    onShown(process: CM_PROCESS_ENTITY) {
    }

    close() {
        this.active = false;
        this.modal.hide();
    }

    onSubmit(process: CM_PROCESS_ENTITY) {
        if ((this.editForm.form.invalid || (this.notes === ''))) {
            this.isShowError = true;
            return;
        }
        process.notes = this.notes
        this.onSubmitEvent.emit(process);
        this.notes = '';
        this.isShowError = false;
        this.close();
    }

    getListReject(conditioN_STATUS: string, procesS_ID: string, procesS_KEY: string, reQ_ID: string) {
        let filterInput: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY();
        filterInput.action = 'ACCESS'
        filterInput.procesS_ID = procesS_ID
        filterInput.conditioN_STATUS = conditioN_STATUS
        filterInput.procesS_KEY = procesS_KEY
        filterInput.reQ_ID = this.reqId;
        this.processService.cM_PROCESS_Reject(filterInput).subscribe(response => {
            this.list = response.items.filter(proc => proc.reQ_ID == reQ_ID)
            this.modal.show();
        });
    }

    onCancel() {
        this.onCancelEvent.emit(null);
        this.close();
    }
}
