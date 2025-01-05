import { Component, ContentChild, EventEmitter, Input, Output, TemplateRef, ViewChild, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR, NgModel } from "@angular/forms";

import { ControlComponent } from "../control.component";

@Component({
    selector: "input-modal-control",
    templateUrl: "./input-modal-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InputModalControlComponent),
            multi: true
        }
    ]
})
export class InputModalControlComponent extends ControlComponent {
    @ContentChild('validation') validationTemplate: TemplateRef<any>;
    @ViewChild('formData') formData: NgModel;

    @Input() type: 'text' | 'number' = 'text';
    @Input() editable: boolean = false;
    @Input() showClear: boolean = true;
    @Input() inputModel: any;
    @Input() fieldsClear: string[];

    @Output() onOpenModal: EventEmitter<any> = new EventEmitter<any>();
    @Output() deleteModel: EventEmitter<any> = new EventEmitter<any>();

    ngModel: any;

    get isArray(): boolean {
        return Array.isArray(this.ngModel)
    }

    emmitChange: any = (value: any) => {
        this.ngModelChange.emit(value);
        this.onChange.emit(value);
    };

    openModal(): void {
        this.onOpenModal.emit();
    }

    delete(): void {
        this.ngModel = undefined;
        this.ngModelChange.emit(undefined);
        if (this.inputModel && this.fieldsClear) {
            this.fieldsClear.forEach(field => {
                this.inputModel[field] = undefined;
            })
        }
        this.deleteModel.emit();
    }

    writeValue(value: any) { this.ngModel = value; }

    removeItem(index) {
        this.isArray && this.ngModel.splice(index, 1)
    }
}
