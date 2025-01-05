import { Component, ContentChild, Input, Output, TemplateRef, ViewEncapsulation, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import * as moment from "moment";
import { ControlComponent } from "../control.component";

@Component({
    selector: "input-date-control",
    templateUrl: "./input-date-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => InputDateControlComponent),
        multi: true
    }]
})
export class InputDateControlComponent extends ControlComponent implements ControlValueAccessor {

    @ContentChild('validation') validationTemplate: TemplateRef<any>;

    @Input() hasTime: boolean = false;
    @Input() inpCss: any

    inputModel: any;

    _ngModel: any;

    @Input() @Output() public get ngModel(): any {
        return this._ngModel;
    }
    public set ngModel(value: moment.Moment) {
        if (!value) return;
        this.inputModel = value.toDate();
    }

    formatDate(value: any): moment.Moment {
        if (!value) return;
        const date = moment(value, 'DD/MM/YYYY').toDate();
        if (!date) return;
        date.setTime(date.getTime() - (date.getTimezoneOffset() * 60000));
        return moment(date);
    }

    formatDateTime(value: any): moment.Moment {
        if (!value) return;
        const date = moment(value, 'DD/MM/YYYY hh:mm').toDate();
        if (!date) return;
        date.setTime(date.getTime() - (date.getTimezoneOffset() * 60000));
        return moment(date);
    }

    emmitChange: any = (value: any) => {
        const date = this.hasTime ? this.formatDateTime(value) : this.formatDate(value);
        if (!this.hasTime) this.ngModelChange.emit(date);
        this.onChange.emit(date);
    };

    writeValue(value: any) {
        this.ngModel = value;
    }
}
