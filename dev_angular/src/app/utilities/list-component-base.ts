import { Component, Injector, OnDestroy, ViewChild } from '@angular/core';
import { PrimeTableComponent } from '@app/shared/core/controls/primeng-table/prime-table/prime-table.component';
import { DefaultComponentBase } from './default-component-base';
import { IListComponent } from './list-component.interface';
import { PrimeTableOption } from '@app/shared/core/controls/primeng-table/prime-table/primte-table.interface';
import { Subject, takeUntil } from 'rxjs';

@Component({ template: '' })
export abstract class ListComponentBase<T> extends DefaultComponentBase implements IListComponent<T>, OnDestroy {
    private unsubscribe$ = new Subject<void>();

    @ViewChild('dataTable') dataTable: PrimeTableComponent<T>;
    options: PrimeTableOption<T>;
    pagingClient: boolean;
    clientData: T[];
    currentItem: T = (<T>{});
    shouldResetTable: boolean = false;
    shouldReloadPaging: boolean = false;

    selectRow(ev1, ev2) {

    }

    onSelectRow(item: T): void {
        this.appToolbar.onSelectRow(item);
    }

    onDblclick(record: any) {
        if (this.appToolbar.buttonUpdateVisible) {
            this.onUpdate(record);
        }
        else if (this.appToolbar.buttonViewDetailEnable) {
            this.onViewDetail(record);
        }
    }

    private _filterInputSearch: T;

    public get filterInputSearch(): T {
        return this._filterInputSearch;
    }

    public set filterInputSearch(value: T) {
        this._filterInputSearch = value;
    }

    constructor(injector: Injector) {
        super(injector);
        this.pagingClient = false;
    }
    onSetData(list: any) {
        throw new Error('Method not implemented.');
    }

    exportToExcel(): void { throw new Error("Method not implemented."); }
    search(): void { throw new Error("Method not implemented."); }
    abstract onUpdate(item: T): void;
    abstract onViewDetail(item: T): void;

    ngOnDestroy() {
        // this.dataTable.selectRecord.unsubscribe();
        // this.dataTable.selectRecords.unsubscribe();
        // this.dataTable.doubleClick.unsubscribe();
        // this.dataTable.selectAllRecord.unsubscribe();
        // this.dataTable.unSelectRecord.unsubscribe();

        // Emit a value to trigger the unsubscription
        this.unsubscribe$.next();
        // Complete the subject to clean up
        this.unsubscribe$.complete();
    }

    initDefault() {
        if (this.dataTable) this.dataTable.listComponent = this;
        const eventHandlers = {
            selectRecord: this.selectRecord.bind(this),
            selectRecords: this.selectRecords.bind(this),
            doubleClick: this.onDoubleClick.bind(this),
            selectAllRecord: this.onSelectAllRecord.bind(this),
            unSelectRecord: this.onUnSelectRecord.bind(this),
        };

        // Subscribe to events using a loop
        Object.keys(eventHandlers).forEach(event => {
            this.dataTable[event].pipe(takeUntil(this.unsubscribe$)).subscribe(
                (value: T | T[]) => eventHandlers[event](value)
            );
        });

        // this.dataTable.selectRecord.subscribe((value: T) => {
        //     this.selectRecord(value)
        // });
        // this.dataTable.selectRecords.subscribe((value: T) => {
        //     this.selectRecords(value);
        // });
        // this.dataTable.doubleClick.subscribe((value: T) => {
        //     this.onDoubleClick(value);
        // });
        // this.dataTable.selectAllRecord.subscribe((value: T[]) => {
        //     this.onSelectAllRecord(value);
        // });
        // this.dataTable.unSelectRecord.subscribe((value: T) => {
        //     this.onUnSelectRecord(value)
        // });
    }

    showTableLoading(): void {
        this.dataTable.setIsLoading(true);
        this.showLoading();
    }

    hideTableLoading(): void {
        this.dataTable.setIsLoading(false);
        this.hideLoading();
    }

    changePage(currentPage: number) {
        this.dataTable.getData();
    }

    onRouteBack() {
        // if (this.shouldResetTable && this['filterInput']) {
        //     this['filterInput']['totalCount'] = 0;
        //     this.search();
        //     this.shouldResetTable = false;
        // }
    }

    setSortingForFilterModel(filterInput: any) {
        if (!filterInput) return;
        filterInput.sorting = this.dataTable.getSorting();
        filterInput.maxResultCount = (this.dataTable.paginator.rows || 0);
        filterInput.skipCount = this.dataTable.paginator.first;
    }

    setRecords(records: T[], totalCount: number) {
        if (this.dataTable.pagingClient)
            this.dataTable.setAllRecords(records);
        else
            this.dataTable.setRecords(records, totalCount);
        if (this['filterInput']) this['filterInput'].totalCount = totalCount;
        this.cdr.detectChanges()
    }

    onDoubleClick(record: T) {
        this.appToolbar?.onSelectRow(record);
        if (this.appToolbar.buttonUpdateEnable) {
            this.onUpdate(record);
        } else if (this.appToolbar.buttonViewDetailEnable) {
            console.log(record)
            this.onViewDetail(record);
        }
    }

    selectRecord(item: T): void {
        this.appToolbar?.onSelectRow(item);
    }

    selectRecords(item: T): void {
        this.appToolbar?.onSelectRow(item);
    }

    onSelectAllRecord(items: T[]): void {
        items.forEach(item => {
            this.appToolbar?.onSelectRow(item);
        });
    }

    onUnSelectRecord(item: T) {

    }

    exportExcel() {
        this.filterInputSearch = { ...this['filterInput'] };
        this.filterInputSearch['top'] = 0;
        this.exportToExcel();
    }

    copyFilterInput() {
        this['filterInput']['totalCount'] = 0;
        this.filterInputSearch = { ...this['filterInput'] };
        this.filterInputSearch['skipCount'] = 0;
        const obj = this.filterInputSearch;
        obj['toJSON'] = function () { return obj; }
    }

    onSearch(): void {
        // if (this.dataTable.paginator.first != 0) {
        //     this.dataTable.paginator.changePage(0);
        // }
        // else {
        if (this.dataTable.paginator) this.dataTable.paginator.first = 0;
        this.copyFilterInput();
        this.search();
        // }
    }

    reloadPage(): void {
        this.dataTable.paginator.first = this.dataTable.paginator.paginatorState.rows * this.dataTable.paginator.getPage();
        this.dataTable.getData();
    }
}
