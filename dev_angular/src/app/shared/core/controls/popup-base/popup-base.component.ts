import { AfterViewInit, Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { PopupFrameComponent } from '@app/shared/common/modals/popup-frames/popup-frame.component';
import { ListComponentBase } from '@app/utilities/list-component-base';
import { PrimeTableOption } from '../primeng-table/prime-table/primte-table.interface';

@Component({ template: '' })
export abstract class PopupBaseComponent<T> extends ListComponentBase<T> implements AfterViewInit {
    @Input() title: string;
    @Input() multiple: boolean = false;
    @Input() keyMember: string;
    @Input() hideFields: { [key: string]: boolean } = {};
    @Input() disableFields: { [key: string]: boolean } = {};

    @Output() onSelect: EventEmitter<any> = new EventEmitter<any>;

    @ViewChild('popupFrame') popupFrame: PopupFrameComponent;

    visible: boolean = false;
    loadedData: boolean = false;
    filterInput: T;
    options: PrimeTableOption<T>;
    @Input() initOnShow: boolean = false;

    get selectedItems(): T[] {
        return this.dataTable.selectedRecords;
    }

    set selectedItems(items: T[]) {
        this.dataTable.selectedRecords = items;
    }

    set setHideFields(hideFields: any) {
        if (!this.dataTable) return;
        this.dataTable.hideFields = hideFields
    }

    constructor(injector: Injector) {
        super(injector);
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.popupFrame.onCloseEvent.subscribe(() => {
            this.clearChecked();
        })
    }

    abstract getResult(checkAll: boolean): Promise<any>

    async search() {
        this.showLoading();
        await this.getResult(false);
    }

    show() {
        if (!this.loadedData) {
            this.runAfterViewed();
            this.loadedData = true;
        }
        this.visible = true;
        this.popupFrame.show();
    }

    close() {
        this.visible = false;
        this.popupFrame.close();
    }

    accept() {
        if (!this.multiple) {
            this.onSelect.emit(this.currentItem);
            this.close();
        } else {
            console.log(this.dataTable.selectedRecords)
            this.onSelect.emit(this.dataTable.selectedRecords);
            this.dataTable.selectedRecords = [];
            this.close();
        }
    }

    isChecked(record: T): boolean {
        return this.dataTable.selectedRecords.filter(x => x[this.keyMember] == record[this.keyMember]).length > 0;
    }

    onDoubleClick(record: T) {
        if (!this.multiple) {
            this.currentItem = record;
        } else {
            this.dataTable.selectedRecords.push(record);
        }
        this.accept();
    }

    selectRecord(record: T): void {
        if (!this.multiple) {
            this.currentItem = record;
        }
    }

    onSelectAllRecord(records: T[]): void {
        this.dataTable.selectedRecords = (records && records.length > 0) ? records : [];
    }

    onUnSelectRecord(record: T) {
        if (!record) return;
        if (this.dataTable.selectedRecords.length > 0 && this.dataTable.selectedRecords.includes(record)) {
            this.dataTable.selectedRecords.splice(this.dataTable.selectedRecords.indexOf(record), 1);
        }
    }

    onUpdate(item: T): void {
        throw new Error('Method not implemented.');
    }

    onViewDetail(item: T): void {
        throw new Error('Method not implemented.');
    }

    runAfterViewed(): void { }

    clearChecked(): void {
        this.dataTable.selectedRecords = []
    }
}
