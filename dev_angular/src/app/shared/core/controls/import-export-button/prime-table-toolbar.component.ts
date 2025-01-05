import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';


@Component({
    templateUrl: './prime-table-toolbar.component.html',
    selector: 'prime-table-toolbar',
    encapsulation: ViewEncapsulation.None
})
export class PrimeTableToolbarComponent implements OnInit {
    @Input() showExportExcel: boolean = false;
    @Input() showImportExcel: boolean = false;
    @Input() showImportExcelTemplate: boolean = false;
    @Output() exportExcel: EventEmitter<any> = new EventEmitter<any>();
    @Output() importExcel: EventEmitter<any> = new EventEmitter<any>();

    constructor() {
    }

    ngOnInit(): void {
    }
    handleExportExcel(): void {
        this.exportExcel.emit();
    }

    handleImportExcel(): void {
        this.importExcel.emit();
    }
} 