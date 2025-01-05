import { ChangeDetectionStrategy, Component, Injector, Input, ViewEncapsulation } from "@angular/core";
import { ChangeDetectionComponent } from "@app/utilities/change-detection.component";

@Component({
    selector: "disabled-input",
    templateUrl: "./disabled-input.component.html",
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})

export class DisabledInputComponent extends ChangeDetectionComponent {
    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    _value: any;

    @Input() get value() {
        return this._value;
    }
    set value(val) {
        this._value = val;
        this.updateView();
    }
}
