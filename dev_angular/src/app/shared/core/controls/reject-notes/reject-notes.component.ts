import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { } from "@shared/service-proxies/service-proxies";
import { PopupBaseComponent } from "../popup-base/popup-base.component-old";
import { ModalDirective } from "ngx-bootstrap/modal";
@Component({
    selector: "reject-notes",
    templateUrl: "./reject-notes.component.html",
    encapsulation: ViewEncapsulation.None
})
export class RejectNotesComponent extends PopupBaseComponent<String> {

    @ViewChild('rejectNotesModal') modal: ModalDirective;
    @ViewChild('editForm') editForm: ElementRef;

    @Output() onSubmitEvent : EventEmitter<any> = new EventEmitter<any>();
    @Output() onCancelEvent : EventEmitter<any> = new EventEmitter<any>();

    @Input() maxLength: number = 10000;
    @Input() title: string;
    @Input() isShowReturnType: boolean = false;
    public selectedReturnType: string;

    buttonContent: string=this.l("Return");
    notes: string="";
    isShowError = false;

    waiting: boolean;
    active: boolean = false;

    public InitAndShow(initVal:string = null){
            this.selectedReturnType = initVal;
        this.show();
    }


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
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.updateView();
            return;
        }
        var returnVal = {
            notes: this.notes,
            returnType: this.selectedReturnType
        }
        this.onSubmitEvent.emit(returnVal);
        this.notes = '';
        this.isShowError = false;
        this.close();
    }

    onCancel(){
        this.onCancelEvent.emit(null);
        this.close();
    }
    RefreshTitle(title:string)
    {
        this.title=title;
        this.updateView();
    }
    RefreshButtonContent(button:string)
    {
        this.buttonContent=button;
        this.updateView();
    }
}
