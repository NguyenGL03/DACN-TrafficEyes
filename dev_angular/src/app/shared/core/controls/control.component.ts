import { Component, ElementRef, EventEmitter, forwardRef, Injector, Input, Output, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ChangeDetectionComponent } from '@app/utilities/change-detection.component';
declare var $: JQueryStatic;
export function createCustomInputControlValueAccessor(extendedInputComponent: any) {
    return {
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => extendedInputComponent),
        multi: true
    };
}

@Component({ template: '' })
export abstract class ControlComponent extends ChangeDetectionComponent implements ControlValueAccessor {

    constructor(injector: Injector) { super(injector); }

    @ViewChild('control') inputRef: ElementRef;

    @Input() fieldName: string;
    @Input() label: string;
    @Input() value: string;
    @Input() name: string;
    @Input() disabled: boolean = false;
    @Input() required: boolean = false;
    @Input() readOnly: boolean = false;
    @Input() hiddenMessage: boolean = false;
    @Input() isShowError: boolean = false;
    @Input() pattern: string;
    @Input() maxLength: number;

    @Output() onClick: EventEmitter<any> = new EventEmitter<any>();
    @Output() onFocusOut: EventEmitter<any> = new EventEmitter<any>();
    @Output() onChange: EventEmitter<any> = new EventEmitter<any>();;
    @Output() ngModelChange: EventEmitter<any> = new EventEmitter<any>();

    abstract writeValue(value: any): void;

    onTouched: any = () => { };
    propagateChange = (_: any) => { };
    registerOnChange(fn: any) { this.propagateChange = fn; }
    registerOnTouched(fn: any) { this.onTouched = fn; }
    setDisabledState(isDisabled: boolean) { this.disabled = isDisabled; }

    generateUUID() {
        var d = new Date().getTime();
        if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
            d += performance.now();
        }
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    }

    toJSON: any = function () {
        let data = {};
        let scope = this;
        Object.keys(this).filter(x => x != "toJSON").forEach(function (k) {
            if (k) {
                data[k] = scope[k];
            }
        })
        return data;
    }

    l(key: string, ...args: any[]): string {
        args.unshift(key);
        args.unshift(this.localizationSourceName);
        return this.ls.apply(this, args);
    }
    addTextToImage(imagePath, text, finallyCallback?: (param) => void): any {
        finallyCallback(imagePath)
    }
    protected onChangeCallback: any;

}
