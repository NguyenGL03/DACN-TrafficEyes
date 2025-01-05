import { AfterViewInit, Component, ContentChild, Injector, Input, OnDestroy, TemplateRef, ViewChild, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { Dropdown } from "primeng/dropdown";
import { ControlComponent } from "../control.component";

@Component({
    selector: "dropdown-control",
    templateUrl: "./dropdown-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DropdownControlComponent),
            multi: true
        }
    ]
})
export class DropdownControlComponent<T> extends ControlComponent implements AfterViewInit, OnDestroy {

    @ContentChild('validation') validationTemplate: TemplateRef<any>;

    @Input() dataKey: string = undefined;
    @Input() valueMember: string = undefined;
    @Input() displayMember: string;
    @Input() filterMember: string[];
    @Input() emptyText: string;
    @Input() filterBy: string;

    @Input() editable: boolean = false;
    @Input() autoSelect: boolean = false;
    @Input() showClear: boolean = false;

    isHidden: boolean = false;
    filterValue: string | undefined = '';

    _ngModel: string;
    get ngModel(): any {
        if (!this._ngModel) return '';
        return this._ngModel;
    }
    @Input() public set ngModel(value: any) {
        //FIXME - Fix lỗi cập nhật lại ngModel trong PrimeTable
        //NOTE - Chưa đánh giá hết vấn đề có thể xảy ra
        if (!this.items || value == null || value == undefined) {
            return;
        }
        this.ngModelChange.emit(value);
        if (this._ngModel != value.toString()) {
            const val = this.getValue(value);
            if (val) this.onChange.emit(val);
        }
        this._ngModel = value;
    }

    _items: T[] = []
    @Input() public set items(items: T[]) {
        if (!items) return;
        this._items = JSON.parse(JSON.stringify(items)); // Deep clone
        if (this._items && this.emptyText) {
            const defaultValue = {} as T;
            defaultValue[this.displayMember] = this.emptyText;
            defaultValue[this.valueMember] = '';
            this._items.unshift(defaultValue);
        }
    }
    get items(): T[] {
        return this._items;
    }

    constructor(injector: Injector) {
        super(injector);
        this.ngModel = '';
    }

    ngAfterViewInit(): void {
        if (!this.filterMember && this.displayMember)
            this.filterMember = this.displayMember.split('|');
        this.cdr.detectChanges();
    }

    getValue(value: any) {
        return this.items.find(item => {
            return item[this.valueMember] == value
        })
    }

    writeValue(value: any) { this.ngModel = value; }

    emmitClick: any = (event: any) => {
        this.onClick.emit(event)
    };

    emmitOnChange: any = (event: any) => {
        if (!event?.value) return;
        const val = this.getValue(event.value);
        this.onChange.emit(val);
    };

    getDisplay(item: T) {
        if (!item[this.valueMember] || !this.displayMember) {
            return this.emptyText;
        }
        if (item[this.valueMember] == ' ') {
            return item[this.displayMember.substring(this.displayMember.indexOf('|') + 1)];
        }
        return this.displayMember.split('|').map(x => item[x]).join(' - ');
    }

    onClear() {
        this.ngModel = null;
    }

    getTypeof(value: any) {
        return typeof value;
    }

}
