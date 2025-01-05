import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { PopupBaseDefaultComponent } from "@app/shared/core/controls/popup-base/popup-base-default.component";


@Component({
    selector: "approve-reject-modal",
    templateUrl: "./approve-reject-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class ApproveRejectModalComponent extends PopupBaseDefaultComponent { 

    @ViewChild('editForm') editForm: ElementRef;

    @Output() onSubmitEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onCancelEvent: EventEmitter<any> = new EventEmitter<any>();

    @Input() maxLength: number = 10000;
    @Input() title: string;
    @Input() isShowReturnType: boolean = false;
    public selectedReturnType: string;

    buttonContent: string = this.l("Return");
    notes: string = "";
    isShowError = false;

    waiting: boolean;
    active: boolean = false;

    public InitAndShow(initVal: string = null) {
        this.selectedReturnType = initVal;
        this.show();
    }


    onShowEvent: EventEmitter<any> = new EventEmitter<any>();

    constructor(injector: Injector) {
        super(injector);
    }

    show() {
        this.active = true;
        this.visible = true;
    }

    onShown() {
        this.onShowEvent.emit(null);
    }

    close() {
        this.active = false;
        this.visible = false;
    }

    onSubmit() {
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
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

    onCancel() {
        this.onCancelEvent.emit(null);
        this.close();
    }

    RefreshTitle(title: string) {
        this.title = title;
    }

    RefreshButtonContent(button: string) {
        this.buttonContent = button;
    }
}