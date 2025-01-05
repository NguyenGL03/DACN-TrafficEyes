import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewEncapsulation, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ControlComponent } from '@app/shared/core/controls/control.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    templateUrl: './ui-color-picker.component.html',
    animations: [appModuleAnimation()],
    encapsulation: ViewEncapsulation.None,
    selector: 'ui-color-picker',
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => UiColorPickerComponent),
        multi: true
    }]
})
export class UiColorPickerComponent extends ControlComponent {
    @Input() label: string;
    @Input() settingName: string;
    @Input() color: string;
    ngModel: any

    @Output() emitValue: EventEmitter<any> = new EventEmitter<any>()

    colorHex: string;
    transparentPercent: number = 100

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {
        this.rgbaToHex(this.color)
    }

    writeValue(value: any): void {
        this.ngModel = value;
    }

    emitOnChange(value: any) {
        this.emitValue.emit(this.hexToRgba(this.colorHex, this.transparentPercent))
    }

    hexToRgba(hex: string, alpha: number = 1): string {
        hex = hex.replace(/^#/, '');
        let r = 0, g = 0, b = 0;

        if (hex.length === 6) {
            r = parseInt(hex.substring(0, 2), 16);
            g = parseInt(hex.substring(2, 4), 16);
            b = parseInt(hex.substring(4, 6), 16);
        }

        return `rgba(${r}, ${g}, ${b}, ${alpha / 100})`;
    }

    rgbaToHex(rgba: string) {
        if(rgba){
            const result = rgba.match(/rgba?\((\d+),\s*(\d+),\s*(\d+),\s*(\d*\.?\d+)\)/);
            if (result) {
                const r = parseInt(result[1], 10);
                const g = parseInt(result[2], 10);
                const b = parseInt(result[3], 10);
                const a = Math.round(parseFloat(result[4]) * 255);
                const toHex = (n: number): string => n.toString(16).padStart(2, '0');
                
                this.colorHex = `#${toHex(r)}${toHex(g)}${toHex(b)}`
                this.transparentPercent = Math.floor((a / 255) * 100)
            }
            else {
                this.colorHex = rgba
            }
        }
    }
}
