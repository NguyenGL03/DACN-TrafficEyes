import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewEncapsulation } from "@angular/core";
import { ControlComponent } from "../control.component";

@Component({
    selector: "input-select-link",
    templateUrl: "./input-select-link-control.component.html",
    encapsulation: ViewEncapsulation.None,
})

export class InputSelectLinkComponent extends ControlComponent implements OnInit {
    writeValue(value: any): void {
        throw new Error("Method not implemented.");
    }

    @Input() displayText: string;
    @Input() labelTitle: string;
    @Input() disableInput: boolean = false;
    @Input() name: string;

    @Input() proMode: boolean = false;


    @Output() showModal: EventEmitter<any> = new EventEmitter<any>();
    @Output() redirectLink: EventEmitter<any> = new EventEmitter<any>();
    @Output() deleteDisplayText: EventEmitter<any> = new EventEmitter<any>();
    // _ngModel: string;

    constructor(injector: Injector) {
        super(injector);
    }


    ngOnInit(): void {
        // TODO
    }
    // public get ngModel(): any {
    //     return this._ngModel;
    // }

    // @Input() @Output() public set ngModel(value) {
    //     this._ngModel = value;
    // }
    updateControlView() {
        this.inputRef.nativeElement.value = this.displayText;
    }

    // setNgModelValue(value) {
    //     this.ngModel = value;
    // }

    openModal(event) {
        this.showModal.emit(event);
    }


    onDeleteDisplayText(event) {
        this.deleteDisplayText.emit(event);
    }


    onClickDisplayText(event) {
        this.redirectLink.emit(event);
    }
}
