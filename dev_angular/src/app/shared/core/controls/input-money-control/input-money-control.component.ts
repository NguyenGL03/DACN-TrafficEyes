import { Component, ContentChild, EventEmitter, Input, Output, TemplateRef, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ControlComponent } from "../control.component";

@Component({
    selector: "input-money-control",
    templateUrl: "./input-money-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => InputMoneyControlComponent),
        multi: true
    }]
})
export class InputMoneyControlComponent extends ControlComponent {
    @ContentChild('validation') validationTemplate: TemplateRef<any>;

    // Định dạng
    @Input() isNegative = false;
    @Input() isLargerZero = false;
    @Input() isDecimal = false;
    @Input() isDecimal_round2 = false;
    @Input() isDefault0 = false;
    @Input() isInt = false;

    @Output() onMoneyValueChange: EventEmitter<number> = new EventEmitter<number>();

    _ngModel: any;

    public get ngModel(): any {
        return this._ngModel;
    }

    @Input() @Output() public set ngModel(value) {
        this._ngModel = this.formatMoney(value);
    }

    writeValue(value: any) {
        this.ngModel = value;
    }

    emmitChange: any = (value: any) => {
        const newValue = this.moneyTextToNumber(value);
        this.ngModelChange.emit(newValue);
        this.onChange.emit(newValue);
    };

    moneyTextToNumber(moneyText: any) {
        if (this.isDecimal) {
            return super.moneyTextToNumberF(moneyText);
        }
        if (this.isDecimal_round2) {
            return super.moneyTextToNumberFRound2(moneyText);
        }
        return super.moneyTextToNumber(moneyText);
    }

    onKeyPress(event: any): boolean {
        if (event.which == 13) {
            var value = parseInt(event.target.value.replaceAll(',', ''));
            if (value < 0 && !this.isNegative) {
                this.ngModel = 0;
                event.target.value = 0;
            }
            if (value <= 0 && this.isLargerZero) {
                this.ngModel = 1;
                event.target.value = 1;
            }
            this._ngModel = event.target.value;
            return true;
        } else if (this.isInt && !this.isNegative) {
            let patt = /^([0-9])$/;
            let result = patt.test(event.key);
            return result;
        } else if (this.isInt && this.isNegative) {
            let patt = /^([0-9-])$/;
            let result = patt.test(event.key);
            return result;
        } else {
            return true;
        }
    }

    focusOut(event: any) {
        if (this.isDefault0 == true) {
            if (event.target.value == null || event.target.value == undefined || event.target.value == '') {
                this.ngModel = 0;
                event.target.value = 0;
            }
            var value = parseInt(event.target.value.replaceAll(',', ''));
            if (value < 0 && !this.isNegative) {
                this.ngModel = 0;
                event.target.value = 0;
            }
            this._ngModel = event.target.value;
            this.onMoneyValueChange.emit(event);
            this.onFocusOut.emit(event);
        }
        else {
            var value = parseInt(event.target.value.replaceAll(',', ''));
            if (value < 0 && !this.isNegative) {
                this.ngModel = 0;
                event.target.value = 0;
            }
            this._ngModel = event.target.value;
            this.onMoneyValueChange.emit(event);
            this.onFocusOut.emit(event);
        }
    }
}