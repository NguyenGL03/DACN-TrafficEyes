import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";


@Component({
    selector: "popup-frame",
    templateUrl: "./popup-frame.component.html",
    encapsulation: ViewEncapsulation.None
})
export class PopupFrameComponent extends AppComponentBase {

    @Output() onSelectEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onCancelEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSearchEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onCloseEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onShowEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onPrintReportEvent: EventEmitter<any> = new EventEmitter<any>();

    @Input() title: string;
    @Input() showSearchButton: boolean = true;
    @Input() showAcceptButton: boolean = true;
    @Input() showCancelButton: boolean = true;
    @Input() showPrintReportButton: boolean = false;
    @Input() showSaveButton: boolean = false;
    @Input() printTextBtn: string = this.l('PrintReport');

    visible: boolean = false;

    constructor(injector: Injector, public ref: ElementRef) {
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

    onSelect() {
        this.onSelectEvent.emit(null);
        this.close();
    }

    onSave() {
        this.onSelectEvent.emit(null);
    }

    onSearch() {
        this.onSearchEvent.emit(null);
    }

    onCancel() {
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