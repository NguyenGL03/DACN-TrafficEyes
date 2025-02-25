import { AfterViewInit, Component, Injector, Input, ViewEncapsulation } from "@angular/core";
import { ChangeDetectionComponent } from "@app/utilities/change-detection.component";

@Component({
    selector: "display-result",
    templateUrl: "./display.component.html",
    encapsulation: ViewEncapsulation.None
})

export class DisplayComponent extends ChangeDetectionComponent implements AfterViewInit {
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

    ngAfterViewInit(): void {
        // COMMENT: this.stopAutoUpdateView();
    }
}
