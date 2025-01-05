import { Component, Input, ViewEncapsulation } from "@angular/core";
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';

@Component({
    selector: "record-status-badge",
    templateUrl: './record-status-badge.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class RecordStatusBadgeComponent<T> {
    @Input() item: T;
    recordStatusConsts = RecordStatusConsts;
}
