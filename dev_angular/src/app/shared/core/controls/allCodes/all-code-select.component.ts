import { Component, ContentChild, EventEmitter, Inject, Injector, Input, OnInit, Output, SimpleChanges, TemplateRef, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { AllCodeServiceProxy, CM_ALLCODE_ENTITY } from "@shared/service-proxies/service-proxies";
import { ControlComponent } from "../control.component";
import { AllCodeSelectStateService } from "./all-code-select-state.service";

@Component({
    selector: "all-code-select",
    templateUrl: "./all-code-select.component.html",
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => AllCodeSelectComponent),
        multi: true
    }],
})
export class AllCodeSelectComponent extends ControlComponent implements OnInit {
    @Input() cdName: string;
    @Input() cdType: string;

    @Output() onSetListSuccess: EventEmitter<any> = new EventEmitter<any>();
    @ContentChild('validation') validationTemplate: TemplateRef<any>;

    @Input() dataKey: string = undefined;
    @Input() valueMember: string = undefined;
    @Input() displayMember: string;
    @Input() filterMember: string[];
    @Input() emptyText: string;
    @Input() filterBy: string;
    @Input() appendToComponent: boolean = false;

    @Input() editable: boolean = false;
    @Input() withOption: boolean = false;
    @Input() autoSelect: boolean = false;
    @Input() showClear: boolean = false;

    items: CM_ALLCODE_ENTITY[] = [];

    _ngModel: string;
    get ngModel(): any {
        if (!this._ngModel) return '';
        return this._ngModel;
    }
    @Input() public set ngModel(value: any) {
        // this.ngModelChange.emit(value);
        console.log('ngModel: ' + this._ngModel);
        if (!value) {
            this._ngModel = '';
            return;
        }
        if (value && this._ngModel != value.toString()) {
            console.log('value: ' + value + '| ngModel: ' + this._ngModel);
            const val = this.getValue(value);
            if (val) this.onChange.emit(val);
        }
        // this._ngModel = value + '';
        if (value !== this._ngModel) {
            console.log('ngModelChanged: ', value);
            this._ngModel = value.toString();
            this.ngModelChange.emit(value);
            this.stateService.ngModel = this._ngModel;
        } else {
            console.log('ngModel did not change, skipping emit');
        }
    }

    filterValue: string | undefined = '';

    constructor(injector: Injector, private allCodeService: AllCodeServiceProxy,
        @Inject(AllCodeSelectStateService) private stateService: AllCodeSelectStateService) {
        console.log('constructor');
        super(injector);
        this.displayMember = 'content';
        this.valueMember = 'cdval';
        if (this.stateService.items.length > 0) {
            this.items = this.stateService.items;
            this.ngModel = this.stateService.ngModel;
        } else {
            this.ngModel = '247';
        }
        
        if (!this.filterMember && this.displayMember) this.filterMember = this.displayMember.split('|');
        
    }

    ngOnInit(): void {
        console.log('ngOnInit');
        if (this.stateService.items.length > 0) {
            this.items = this.stateService.items;
            this.ngModel = this.stateService.ngModel;
        } else {
            this.refreshList();
        }
        
    }

    // ngOnChanges(changes: SimpleChanges) {
    //     if (changes['ngModel'] && !changes['ngModel'].isFirstChange()) {
    //         // Logic xử lý khi ngModel thay đổi, tránh reset lại dropdown
    //         console.log('ngModel has changed, handling the update');
    //         this.refreshList(); // Gọi refreshList nếu cần, nhưng phải kiểm tra điều kiện hợp lý
    //     }
    // }
      

    refreshList() {
        console.log('refreshList');
        this.allCodeService.cM_ALLCODE_GetByCDNAME(this.cdName, this.cdType).subscribe(response => {
            if (this.emptyText && response) {
                const first: CM_ALLCODE_ENTITY = new CM_ALLCODE_ENTITY();
                first.content = this.emptyText;
                first.cdname = null;
                first.cdval = '';
                response.unshift(first);
            }
            this.items = response;
            if (this.items.length > 0) {
                console.log('call setter for ngModel');
                this.ngModel = this.items[0][this.valueMember];
            }
            this.stateService.items = this.items;
            this.stateService.ngModel = this.ngModel;
            this.onSetListSuccess.emit(response);
            this.cdr.detectChanges();
        });
    }

    getValue(value: any) {
        console.log('getValue' + value);
        return this.items.find(item => item[this.valueMember] == value)
    }

    writeValue(value: any) {
        console.log('writeValue' + value);
        this.ngModel = value; 
    }

    emmitClick: any = (event: any) => {
        console.log('emmitClick');
        this.onClick.emit(event)
    };

    getDisplay(item: CM_ALLCODE_ENTITY) {
        // console.log('getDisplay');
        if (!item[this.valueMember] || !this.displayMember) {
            return this.emptyText;
        }
        if (item[this.valueMember] == ' ') {
            return item[this.displayMember.substring(this.displayMember.indexOf('|') + 1)];
        }

        return this.displayMember.split('|').map(x => item[x]).join(' - ');
    }

    onClear() {
        console.log('onClear');
        this.ngModel = '';
    }
}