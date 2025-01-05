import { Component, Input, Output, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ControlComponent } from "../control.component";

@Component({
    selector: "checkbox-control",
    templateUrl: "./checkbox-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CheckboxControlComponent),
            multi: true
        }
    ]
})
export class CheckboxControlComponent extends ControlComponent {

    @Input() trueValue: any = 1;
    @Input() falseValue: any = 0;
    @Input() classList:string = '';

    @Input() checked: any

    _checked: boolean;
    _ngModel: boolean;

    @Output() @Input() public get ngModel(): string {
        if (this._ngModel) {
            return this.trueValue;
        } else {
            return this.falseValue;
        }
    }

    public set ngModel(value) {
        this._ngModel = (value == this.trueValue);
    }

    ngOnInit(): void { }

    writeValue(value: any) { this.ngModel = value; }

    emmitChange: any = (event: any) => {
        this.ngModelChange.emit(event.target.checked ? this.trueValue : this.falseValue);
        this.onChange.emit(event.target.checked ? this.trueValue : this.falseValue);
    };

}
