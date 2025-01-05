import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
 
@Component({
    selector: "reject-modal",
    templateUrl: "./reject-modal.component.html", 
    encapsulation: ViewEncapsulation.None
})
export class RejectModalComponent extends AppComponentBase {

    @ViewChild('rejectModal') modal: any;
    @ViewChild('editForm') editForm: ElementRef;

    @Output() onSubmitEvent : EventEmitter<string> = new EventEmitter<string>();
    @Output() onCancelEvent : EventEmitter<any> = new EventEmitter<any>(); 
    @Output() onRejectMSEvent : EventEmitter<any> = new EventEmitter<any>();
 
    @Input() isShowRejectButton : boolean = true;
    @Input() isShowRejectDMMSButton : boolean = false;
    @Input() isShowRejectTKTGDButton : boolean = false;
    @Input() isShowRejectTKButton : boolean = false;
    @Input() isShowRejectNVXLButton : boolean = false;
    @Input() isShowRejectCreateButton : boolean = false;
    @Input() isShowRejectGDVButton : boolean = false;

    notes: string;
    isShowError = false;

    waiting: boolean;
    active: boolean = false;

    onShowEvent : EventEmitter<any> = new EventEmitter<any>();

    constructor(injector: Injector) {
        super(injector);
    }

    show() {
        this.active = true;
        this.modal.show();
    }

    onShown() {
        this.onShowEvent.emit(null);
    }

    close() {
        this.active = false;
        this.modal.hide();
    }

    onSubmit(){
        if ((this.editForm as any).form.invalid || (this.notes === '')) {
            this.isShowError = true; 
            return;
        }
        this.onSubmitEvent.emit(this.notes);
        this.notes = '';
        this.isShowError = false;
        this.close();
    }

    onCancel(){
        this.onCancelEvent.emit(null);
        this.close();
    }

    //BAODNQ 19/8/2022 : xử lý trả về phần mua sắm
    onRejectMS(type : string) : void{
        if ((this.editForm as any).form.invalid || (this.notes === '')) {
            this.isShowError = true; 
            return;
        } 
        let emitData : any = {
            type : type,
            reason : this.notes
        };
        this.onRejectMSEvent.emit(emitData);
        this.notes = '';
        this.isShowError = false;
        this.close();
    }

    handleNotesChange(value){
        console.log("value: " + value);
        if(value != ''){
            this.isShowError = false; 
            return this.notes = value;
        }
    }
}