import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { ComponentBase } from "@app/utilities/component-base-old";
import { ModalDirective } from "ngx-bootstrap/modal";

@Component({
    selector: "popup-frame-old",
    templateUrl: "./popup-frame.component.html",
    encapsulation: ViewEncapsulation.None
})
export class PopupFrameComponent extends ComponentBase {

    @ViewChild('popupFrameModal') modal: ModalDirective;

    @Output() onSelectEvent : EventEmitter<any> = new EventEmitter<any>();
    @Output() onCancelEvent : EventEmitter<any> = new EventEmitter<any>();
    @Output() onSearchEvent : EventEmitter<any> = new EventEmitter<any>();

    @Output() onPrintReportEvent : EventEmitter<any> = new EventEmitter<any>();
    @Output() onCloseEvent :  EventEmitter<any> = new EventEmitter<any>();

    @Input() title: string;

    waiting: boolean;
    active: boolean = false;

    @Input() showSearchButton : boolean = true;
    @Input() showAcceptButton : boolean = true;
    @Input() showCancelButton : boolean = true;

    @Input() showPrintReportButton : boolean = false;
    @Input() showSaveButton : boolean = false;
    @Input() printTextBtn : string = this.l('PrintReport');

    onShowEvent : EventEmitter<any> = new EventEmitter<any>();

    ;

    constructor(injector: Injector,
        public ref : ElementRef) {
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

    onSelect(){
        this.onSelectEvent.emit(null);
        this.close();
    }

    //BAODNQ 6/8/2022
    onSave(){
        this.onSelectEvent.emit(null);
    }

    onSearch(){
        this.onSearchEvent.emit(null);
    }

    onCancel(){
        this.onCancelEvent.emit(null);
        this.close();
    }

    onPrintReport() {
        this.onPrintReportEvent.emit(null);
    }

    onClose() {
        this.onCloseEvent.emit(null);
        this.close();
    }
}
