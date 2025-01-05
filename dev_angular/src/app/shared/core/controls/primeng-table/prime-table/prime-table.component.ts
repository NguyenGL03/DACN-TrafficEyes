import { Component, ElementRef, EventEmitter, Injector, Input, Output, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { IListComponent } from '@app/utilities/list-component.interface';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Paginator } from 'primeng/paginator';
import { TableHeaderCheckboxToggleEvent, TableRowSelectEvent, TableRowUnSelectEvent } from 'primeng/table';
import { PrimeTableOption } from './primte-table.interface';

@Component({
    templateUrl: './prime-table.component.html',
    selector: 'prime-table',
    encapsulation: ViewEncapsulation.None
})
export class PrimeTableComponent<T> extends AppComponentBase {
    @Input() options: PrimeTableOption<T>;

    @Input() heading: string;
    @Input() pagingClient: boolean = false;
    @Input() multiple: boolean = false;
    @Input() tableName: string;
    @Input() isShowError: boolean = false;
    @Input() disableHeaderButton: boolean = false;
    @Input() columnTemplates: { [key: string]: TemplateRef<any> } = {};
    @Input() templateActOptions: TemplateRef<any>;
    @Input() hideFields: { [key: string]: boolean } = {};
    @Input() disableFields: { [key: string]: boolean } = {};

    @Input() downloadTemplate:any

    @Output() onReloadPage: EventEmitter<any> = new EventEmitter<any>();
    @Output() selectRecord: EventEmitter<any> = new EventEmitter<any>();
    @Output() unSelectRecord: EventEmitter<any> = new EventEmitter<any>();
    @Output() selectRecords: EventEmitter<any> = new EventEmitter<any>();
    @Output() selectAllRecord: EventEmitter<any> = new EventEmitter<any>();
    @Output() doubleClick: EventEmitter<any> = new EventEmitter<any>();
    @Output() onAddRecord: EventEmitter<any> = new EventEmitter<any>();
    @Output() onDeleteRecord: EventEmitter<any> = new EventEmitter<any>();
    @Output() onImportRecord: EventEmitter<any> = new EventEmitter<any>();
    @Output() onExportRecord: EventEmitter<any> = new EventEmitter<any>();
    
    @ViewChild('paginator') paginator: Paginator;
    @ViewChild('tableContainer') table: ElementRef;


    isLoading: boolean = false;
    currentItem: T
    selectedRecord: T;
    selectedRecords: T[] = [];
    allData: T[] = [];
    records: T[] = [];
    listComponent: IListComponent<T>;
    loading: boolean = false;
    first: number = 0;
    sorting: { field?: string, order?: 'asc' | 'desc' } = {};

    constructor(injector: Injector) {
        super(injector);
    }

    onSelectRow(event: TableRowSelectEvent, isMultiple: boolean = false): void {
        event.data['isChecked'] = true;
        if (isMultiple) {
            this.selectRecords.emit(event.data);
        } else {
            this.selectRecord.emit(event.data);
        }
    }

    onUnSelectRow(event: TableRowUnSelectEvent, isMultiple: boolean = false): void {
        event.data['isChecked'] = false;
        if (isMultiple) {
            this.unSelectRecord.emit(event.data);
        } else
            this.selectRecord.emit(undefined);
    }

    onDoubleClick(record: T): void {
        this.doubleClick.emit(record)
    }

    onPageChange(event: any) {
        this.paginator.first = event.first;
        if (this.pagingClient) {
            this.first = this.paginator.first;
            this.records = this.allData.slice(this.paginator.first, this.paginator.first + this.paginator.rows);
            return;
        }
        if (this.listComponent) {
            this.listComponent.changePage(event.page);
        }
        this.clearSelectedRecord();
    }

    onSort(field: string): void {
        if (this.sorting.field == field) {
            this.sorting.order = this.sorting.order == 'asc' ? 'desc' : 'asc';
        } else {
            this.sorting.field = field;
            this.sorting.order = 'asc';
        }
        if (this.onReloadPage) this.onReloadPage.emit(null);
    }

    setIsLoading(loading: boolean): void {
        this.loading = loading
    }

    getData() {
        if (this.paginator.totalRecords) this.onReloadPage.emit(null);
    }

    setRecords(records: T[], totalRecordsCount?: number): void {
        if (!records) return;
        this.allData = records;
        this.records = records;
        this.first = this.paginator.first;
        this.primengTableHelper.isLoading = false;
        if (!totalRecordsCount) totalRecordsCount = records.length;
        this.primengTableHelper.totalRecordsCount = totalRecordsCount;
    }

    setAllRecords(records: T[]) {
        if (!records) return;
        this.pagingClient = true;
        this.allData = records;
        this.first = this.paginator.first;
        this.records = this.allData.slice(this.first, this.paginator.rows);
        this.primengTableHelper.totalRecordsCount = records.length;
    }

    setList(records: T[]) {
        this.setAllRecords(records);
    }

    getSorting(): string {
        if (!this.sorting?.field) return null;
        return `${this.sorting.field} ${this.sorting.order}`;
    }

    addRecord(record?: T): T {
        const item = record || { key: this.primengTableHelper.totalRecordsCount } as T;
        item['toJSON'] = function () {
            let data = {};
            let scope = this;
            Object.keys(this).filter(x => x != "toJSON").forEach(function (k) {
                if (k) data[k] = scope[k];
            })
            return data;
        }

        this.allData.push(item);
        this.records = this.allData.slice(this.paginator.first, this.paginator.first + this.paginator.rows);
        this.primengTableHelper.totalRecordsCount = (this.primengTableHelper.totalRecordsCount || 0) + 1;
        return item
    }

    pushItems(records: T[]) {
        this.allData.push(...records);
        this.records = this.allData.slice(this.paginator.first, this.paginator.first + this.paginator.rows);
        this.primengTableHelper.totalRecordsCount = (this.primengTableHelper.totalRecordsCount || 0) + records.length;
    }

    removeAllCheckedRecord(): void {
        let number = 0;
        if (this.pagingClient) {
            this.allData = this.allData.filter(item => !item['isChecked'] && !this.selectedRecords.find(_item => _item == item))
            this.records = this.allData.slice(this.paginator.first, this.paginator.first + this.paginator.rows);
            this.primengTableHelper.totalRecordsCount -= this.selectedRecords.length;
        } else {
            this.records = this.records.filter(item => !item['isChecked'] && !this.selectedRecords.find(_item => _item == item))
            this.primengTableHelper.totalRecordsCount -= number;
        }
        this.clearSelectedRecord();
    }

    clearSelectedRecord(): void {
        this.selectedRecords = [];
    }

    onCheckAll(event: TableHeaderCheckboxToggleEvent) {
        if (event.checked) {
            this.selectedRecords = this.pagingClient ? this.allData : this.records;
            this.selectAllRecord.emit(this.records);
        } else {
            this.clearSelectedRecord();
            this.selectAllRecord.emit(undefined);
        }
    }

    // ====================================================== Fuction Supporter ======================================================

    getSum(fieldName: string): number {
        return this.allData.reduce((current, item) => current + item[fieldName], 0);
    }

    getSumByFn(fn: (record: T) => number): number {
        return this.allData.reduce((current, item) => current + fn(item), 0);
    }

    name(fieldName: string, index: number): string {
        if (!this.tableName) return `${fieldName}-${index}`
        return `${fieldName}-${index}-${this.tableName}`;
    }

    setConfig(member: string, value: any): void {
        if (!this.options.config) return;
        this.options.config[member] = value;
    }

    setColumn(title: string, member: string, value: any): void {
        if (!this.options.columns) return;
        this.options.columns[title][member] = value;
    }

    selectRow(record: T) {
        this.currentItem = record;
    }

    handleOnChange(column: any, event: any) {
        if (column?.onChange) column.onChange(event)
    }

    setDropdownItems(columnId: string, items: any[]) {
        const column = this.options.columns.find(column => column.columnId === columnId);
        if (column) column.items = JSON.parse(JSON.stringify(items));
    }

    getValidationMessage(): string | undefined {
        // console.log(this.table.nativeElement);

        const inputs = $(this.table.nativeElement).find('.form-control.ng-invalid')

        return inputs.length > 0 ? 'FormInvalid' : undefined
    }
}
