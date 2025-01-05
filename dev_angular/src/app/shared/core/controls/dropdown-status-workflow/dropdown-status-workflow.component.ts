import { Component, ContentChild, EventEmitter, Injector, Input, OnInit, Output, TemplateRef, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { CM_PROCESS_STATUS_ENTITY, ProcessServiceProxy } from "@shared/service-proxies/service-proxies";
import { ControlComponent } from "../control.component";

@Component({
    selector: "dropdown-status-workflow",
    templateUrl: "./dropdown-status-workflow.component.html",
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => DropdownStatusWorkflowComponent),
        multi: true
    }],
})
export class DropdownStatusWorkflowComponent extends ControlComponent implements OnInit {
    @Input() processKey: string;

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

    items: CM_PROCESS_STATUS_ENTITY[] = [];

    _ngModel: string;
    get ngModel(): any {
        if (!this._ngModel) return '';
        return this._ngModel;
    }
    @Input() public set ngModel(value: any) {
        this.ngModelChange.emit(value);
        if (!value) {
            this._ngModel = '';
            return;
        }
        if (value && this._ngModel != value.toString()) {
            const val = this.getValue(value);
            if (val) this.onChange.emit(val);
        }
        this._ngModel = value + '';
    }

    filterValue: string | undefined = '';

    constructor(injector: Injector, private processService: ProcessServiceProxy) {
        super(injector);
        this.displayMember = 'statuS_NAME';
        this.valueMember = 'status';
        this.ngModel = '';
        if (!this.filterMember && this.displayMember) this.filterMember = this.displayMember.split('|');
    }

    ngOnInit(): void {
        this.refreshList();
    }

    refreshList() {

        const filter: CM_PROCESS_STATUS_ENTITY = new CM_PROCESS_STATUS_ENTITY();
        filter.procesS_KEY = this.processKey;

        this.processService.cM_PROCESS_GetStatusByProcess(filter).subscribe(response => {
            if (this.emptyText && response) {
                const first: CM_PROCESS_STATUS_ENTITY = new CM_PROCESS_STATUS_ENTITY();
                first.statuS_NAME = this.emptyText;
                first.status = '';
                response.unshift(first);
            }
            this.items = response;
            this.onSetListSuccess.emit(response);
            this.cdr.detectChanges();
        });
    }

    getValue(value: any) {
        return this.items.find(item => item[this.valueMember] == value)
    }

    writeValue(value: any) { this.ngModel = value; }

    emmitClick: any = (event: any) => {
        this.onClick.emit(event)
    };

    getDisplay(item: CM_PROCESS_STATUS_ENTITY) {
        if (!item[this.valueMember] || !this.displayMember) {
            return this.emptyText;
        }
        if (item[this.valueMember] == ' ') {
            return item[this.displayMember.substring(this.displayMember.indexOf('|') + 1)];
        }

        return this.displayMember.split('|').map(x => item[x]).join(' - ');
    }

    onClear() {
        this.ngModel = '';
    }
}