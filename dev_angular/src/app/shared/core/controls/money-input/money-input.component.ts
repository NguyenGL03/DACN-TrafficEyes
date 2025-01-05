import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewEncapsulation } from "@angular/core";
import { ControlComponent, createCustomInputControlValueAccessor } from "../control.component";


declare var $: JQueryStatic;

@Component({
    selector: "money-input",
    templateUrl: "./money-input.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [createCustomInputControlValueAccessor(MoneyInputComponent)]
})

export class MoneyInputComponent extends ControlComponent implements OnInit {
    writeValue(value: any): void {

    }

    @Output() onMoneyValueChange: EventEmitter<number> = new EventEmitter<number>();
    @Output() focusout: EventEmitter<any> = new EventEmitter<any>();
    @Input() disabled = false;
    @Input() isNegative = false;
    @Input() isLargerZero = false;
    @Input() isDecimal = false;
    @Input() isDecimal_round2 = false;
    @Input() isDefault0 = false;
    @Input() isInt = false;

    @Input() inputCss: string = 'form-control';

    _ngModel: string;


    afterViewInit() {
    }

    moneyTextToNumber(moneyText) {
        if (this.isDecimal) {
            return super.moneyTextToNumberF(moneyText);
        }
        else if (this.isDecimal_round2) {
            return super.moneyTextToNumberFRound2(moneyText);
        }
        else {
            return super.moneyTextToNumber(moneyText);
        }
    }

    setNgModelValue(value) {
        this.ngModel = value;
    }

    updateControlView() {
        this.inputRef.nativeElement.value = this._ngModel;
    }

    public get ngModel(): any {
        return this.moneyTextToNumber(this._ngModel);
    }

    @Input() @Output() public set ngModel(value) {
        this._ngModel = this.formatMoney(value);
    }

    get valueSendOut() {
        const input = <HTMLInputElement>this.inputRef.nativeElement;
        return parseInt(input.value['replaceAll'](',', ''));
    }

    onKeyPress(event): boolean {
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
            this.onChangeCallback(this.ngModel);
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

    // onFocusOut(event) {
    //     // doanptt 120722 neu khong nhap gi thi mac dinh là 0
    //     if (this.isDefault0 == true) {
    //         if (event.target.value == null || event.target.value == undefined || event.target.value == '') {
    //             this.ngModel = 0;
    //             event.target.value = 0;
    //         }
    //         var value = parseInt(event.target.value.replaceAll(',', ''));
    //         if (value < 0 && !this.isNegative) {
    //             this.ngModel = 0;
    //             event.target.value = 0;
    //         }
    //         this._ngModel = event.target.value;
    //         this.onChangeCallback(this.ngModel);
    //         this.onMoneyValueChange.emit(event);
    //         this.focusout.emit(event);
    //     }
    //     else {
    //         var value = parseInt(event.target.value.replaceAll(',', ''));
    //         if (value < 0 && !this.isNegative) {
    //             this.ngModel = 0;
    //             event.target.value = 0;
    //         }
    //         this._ngModel = event.target.value;
    //         this.onChangeCallback(this.ngModel);
    //         this.onMoneyValueChange.emit(event);
    //         this.focusout.emit(event);
    //     }
    // }

    constructor(
        injector: Injector
    ) {
        super(injector)
    }

    ngOnInit(): void {
    }

}
