import { Component, ContentChild, Input, TemplateRef, ViewEncapsulation, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { ControlComponent } from "../control.component";

@Component({
    selector: "input-control",
    templateUrl: "./input-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InputControlComponent),
            multi: true
        }
    ]
})
export class InputControlComponent extends ControlComponent implements ControlValueAccessor {

    @ContentChild('input') inputTemplate: TemplateRef<any>;
    @ContentChild('validation') validationTemplate: TemplateRef<any>;

    @Input() type: 'text' | 'number';
    @Input() min: number;
    @Input() max: number;
    @Input() minlength: number;
    @Input() maxlength: number;
    @Input() email: boolean = false;
    @Input() placeholder: string = "";

    ngModel: any;

    emmitChange: any = (value: any) => {
        this.ngModelChange.emit(value);
        this.onChange.emit(value);
    };

    writeValue(value: any) {
        this.ngModel = value;
    }
}
