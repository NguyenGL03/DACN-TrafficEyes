import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";

@Component({
    selector: "approve-modal",
    templateUrl: "./approve-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class ApproveModalComponent extends AppComponentBase {
 
    @ViewChild('editForm') editForm: ElementRef;

    @Output() onSubmitEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onCancelEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onRejectEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onShowEvent: EventEmitter<any> = new EventEmitter<any>();

    @Input() title: string;
    @Input() isShowReturnType: boolean = false;
    @Input() visibleIsAuthority: boolean = false;
    @Input() isAuthority: boolean;
    public selectedReturnType: string;

    visible: boolean = false;
    buttonContent: string = this.l("Approve");
    item: string = "";
    isShowError = false;
    enableSubmit = true;
    waiting: boolean;
    loadingSubmit: boolean = false;
    loadingCancel: boolean = false;

    constructor(injector: Injector) {
        super(injector);
    }

    show() {
        this.visible = true;
    }

    close() {
        this.visible = false;
    }

    onShown() {
        this.onShowEvent.emit(null);
    }

    onSubmit() {
        this.loadingSubmit = true;
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            return;
        }
        const returnVal = {
            item: this.item,
            returnType: this.selectedReturnType,
            isAuthority: this.isAuthority
        }
        this.onSubmitEvent.emit(returnVal);
        this.item = '';
        this.isShowError = false;
        this.close();
    }

    onReject() {
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            return;
        }
        const returnVal = {
            item: this.item,
            returnType: this.selectedReturnType
        }
        this.onRejectEvent.emit(returnVal);
        this.item = '';
        this.isShowError = false;
        this.close();
    }

    onCancel() {
        this.onCancelEvent.emit(null);
        this.close();
    }

    refreshTitle(title: string) {
        this.title = title;
    }

    refreshButtonContent(button: string) {
        this.buttonContent = button;
    }

    refreshItem(item: string) {
        this.item = item;
    }

    onKeyUp() {
        this.enableSubmit = /^(?!\s*$).+/.test(this.item);
    }

}
