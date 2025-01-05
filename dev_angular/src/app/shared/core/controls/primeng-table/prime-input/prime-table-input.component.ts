import { Component, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';
import { ColumnPrimeTableOption, DataSelection, InputType } from '../prime-table/primte-table.interface';

@Component({
    templateUrl: './prime-table-input.component.html',
    selector: 'prime-table-input',
    encapsulation: ViewEncapsulation.None
})
export class PrimeTableInputComponent<T> {

    @Input() record: T = {} as T;
    @Input() items: any[];
    @Input() inputType: InputType;
    @Input() name: string;
    @Input() fieldName: string;
    @Input() valueMember: string;
    @Input() displayMember: string;
    @Input() emptyText: string;
    @Input() disabled: boolean = false;
    @Input() required: boolean = false;
    @Input() editable: boolean = true;
    @Input() isShowError: boolean = true;

    @Output() onChange: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSelect: EventEmitter<DataSelection<T>> = new EventEmitter<DataSelection<T>>();
    @Output() onFocusOut: EventEmitter<any> = new EventEmitter<any>();
    @Output() onOpenModal: EventEmitter<any> = new EventEmitter<any>();
    @Output() onDeleteModel: EventEmitter<any> = new EventEmitter<any>();

    _column: ColumnPrimeTableOption<any>;

    @Input() set column(column: ColumnPrimeTableOption<any>) {
        this.items = column.items;
        this.valueMember = column.valueMember;
        this.displayMember = column.displayMember;
        this.inputType = column.inputType;
        this.emptyText = column.emptyText;
        this.fieldName = column.name;
        this.required = column.required;
        this.editable = column.editable;
        this._column = column;
    }

    get column() {
        return this._column
    }

    get type(): typeof InputType {
        return InputType;
    }

    handleOnOpenModal(event: any) {
        if (this.column.onOpenModal) this.onOpenModal.emit(this.record);
    }

    handleOnDeleteModel(event: any) {
        if (this.column.onDeleteModel) this.onDeleteModel.emit(this.record);
    }

    handleOnChange(event: any) {
        if (this.column.onChange) this.onChange.emit({ record: this.record, value: event });
    }

    handleOnSelect(event: any) {
        if (this.column.onSelect) this.onSelect.emit({ record: this.record, selected: event });
    }
}
