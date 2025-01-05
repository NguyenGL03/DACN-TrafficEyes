import { Component, ContentChild, Input, TemplateRef, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ControlComponent } from "../control.component";

@Component({
    selector: "textarea-control",
    templateUrl: "./textarea-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TextAreaControlComponent),
            multi: true
        }
    ]
})
export class TextAreaControlComponent extends ControlComponent {
 
    @ContentChild('validation') validationTemplate: TemplateRef<any>;

    @Input() row: number = 3;
 
    ngModel: any;

    emmitChange: any = (value: any) => {
        this.ngModelChange.emit(value);
        this.onChange.emit(value);
    };
    
    writeValue(value: any) { this.ngModel = value; }
}
