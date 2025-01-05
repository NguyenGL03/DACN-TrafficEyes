import { Component, ContentChild, EventEmitter, Input, OnInit, Output, TemplateRef, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ControlComponent } from "../control.component";

@Component({
    selector: "dropdown-lazy-control",
    templateUrl: "./dropdown-lazy-control.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => DropdownLazyControlComponent),
        multi: true
    }]
})
export class DropdownLazyControlComponent<T> extends ControlComponent implements OnInit {
    @ContentChild('validation') validationTemplate: TemplateRef<any>;

    @Input() valueMember: string;
    @Input() displayMember: string;
    @Input() emptyText: string;
    @Input() filterBy: string;

    @Input() required: boolean = false;
    @Input() isShowError: boolean = false;
    @Input() editable: boolean = false;
    @Input() disabled: boolean = false;
    @Input() isShowClear: boolean = true;

    @Output() onLoad: EventEmitter<any> = new EventEmitter<any>();
    @Output() onChangeLoad: EventEmitter<any> = new EventEmitter<any>();

    @Input() state: LazyDropdownState;

    ngModel: any;

    private loadedEmptyValue: boolean = false;
    _items: T[] = [];
    @Input() public set items(items: T[]) {
        if (items && this.emptyText && !this.loadedEmptyValue) {
            const defaultValue = {} as T;
            defaultValue[this.displayMember] = this.emptyText;
            defaultValue[this.valueMember] = '';
            items.unshift(defaultValue);
            this.loadedEmptyValue = true;
        }
        this._items = items;
    }
    get items(): T[] {
        return this._items;
    }

    ngOnInit(): void {
        this.state = {
            maxResultCount: 20,
            skipCount: 0,
            totalCount: 0,
            filter: null,
            canLoad: true,
        }
        this.ngModel = '';
        if (!this.items || this.items.length <= 0)
            this.onLoad.emit({
                state: this.state,
                callback: ((result: any) => {
                    this.items = [...this.items, ...result.items];
                    this.state.totalCount = result.totalCount;
                    this.state.skipCount += this.state.maxResultCount;
                    this.state.canLoad = result.items.length > 0;
                })
            })
    }

    writeValue(value: any) { this.ngModel = value; }

    getValue() {
        return this.items.find(item => item[this.valueMember] == this.ngModel);
    }

    emmitChange: any = (item: any) => {
        this.onChange.emit(this.getValue());
        this.ngModelChange.emit(item.value);
    };


    addEvent(evt: any): void {
        const _this = this;

        const element: any = evt.element.querySelector('.p-dropdown-items-wrapper');

        element.style.maxWidth = element.style.minWidth;

        element.addEventListener('scroll', (event: any) => {
            if (element.scrollHeight - element.scrollTop <= 300) {
                _this.handleOnLoad(null);
            }
        })
    }

    handleOnLoad(event?: any): any {
        if (!this.state.canLoad) return;
        this.state.canLoad = false;
        this.onLoad.emit({
            state: this.state,
            callback: ((result: any) => {
                this.items = [...this.items, ...result.items];
                this.state.totalCount = result.totalCount;
                this.state.skipCount += this.state.maxResultCount;
                this.state.canLoad = result.items.length > 0;
                // this.onChangeLoad.emit({ state: this.state, items: this.items });
            })
        })
    }
    private timeout = null;
    filterInput: string = '';

    customFilterFunction(event: any): void {
        if (this.timeout) clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
            this.state = {
                maxResultCount: 20,
                skipCount: 0,
                totalCount: 0,
                filter: this.filterInput,
                canLoad: true,
            }
            this.onLoad.emit({
                state: this.state,
                callback: ((result: any) => {
                    this.items = result.items;
                    this.state.totalCount = result.totalCount;
                    this.state.skipCount += this.state.maxResultCount;
                    this.state.canLoad = result.items.length > 0;
                    // this.onChangeLoad.emit({ state: this.state, items: this.items });
                })
            })
        }, 500)
    }

    getDisplay(item: T) {
        if (!item[this.valueMember] || !this.displayMember) {
            return this.emptyText;
        }

        if (item[this.valueMember] == ' ') {
            return item[this.displayMember.substring(this.displayMember.indexOf('|') + 1)];
        }

        return this.displayMember.split('|').map(x => item[x]).join(' - ');
    }

    reset(): void {

    }

    get lastItem(): any {
        return this.items.pop();
    }
}

export interface LazyDropdownState {
    skipCount: number,
    maxResultCount: number,
    totalCount: number,
    filter: any,
    canLoad: boolean,
}


export interface LazyDropdownResponse {
    state: LazyDropdownState,
    callback: (result: any) => void
} 