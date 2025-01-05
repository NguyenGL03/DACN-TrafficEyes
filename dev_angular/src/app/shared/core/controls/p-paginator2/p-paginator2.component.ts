import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, NgZone, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { AbstractControl, FormControl } from "@angular/forms";
import { PrimeNGConfig } from "primeng/api";
import { Paginator } from "primeng/paginator";
@Component({
    selector: "p-paginator2",
    templateUrl: "./p-paginator2.component.html",
    encapsulation: ViewEncapsulation.None
})
export class Paginator2Component extends Paginator implements AfterViewInit {
    ngAfterViewInit(): void {
        this.zone.runOutsideAngular(() => {

            $(this.selectPageSize.nativeElement).change((e) => {
                let o1 = FormControl.prototype.updateValueAndValidity;
                let o2 = AbstractControl.prototype.updateValueAndValidity;
                FormControl.prototype.updateValueAndValidity = this.doNothing;
                AbstractControl.prototype.updateValueAndValidity = this.doNothing;
                this.onSelectRecordsChange(e);
                FormControl.prototype.updateValueAndValidity = o1;
                AbstractControl.prototype.updateValueAndValidity = o2;
            })
        })
    }

    doNothing() {

    }

    // currentPage: number;
    @ViewChild('selectPageSize') selectPageSize: ElementRef;
    @ViewChild('paginator') paginator: Paginator;

    @Output() onSelectRecordChange: EventEmitter<any> = new EventEmitter<any>();

    @Input() set page(p: number) {
        console.count();
        if (p != this.getPage()) {

            var pc = this.paginator?.getPageCount();
            if (p >= 0 && p < pc) {
                this.paginator.first = this.paginator.rows * p;
            }


        }
    }

    constructor(
        private zone: NgZone,
        private cdr: ChangeDetectorRef,
        private configs: PrimeNGConfig
    ) {
        super(cdr, configs);
    }

    changePage(p: number): void {
        this.paginator.changePage(p);
    }

    getPage(): number {
        return this.paginator?.getPage();
    }

    onSelectRecordsChange(evt) {
        this.changeRecordsPerPage(evt.target.value);
    }

    changeRecordsPerPage(rows) {
        this.paginator.rows = rows;
        this.paginator.paginatorState.rows = rows;
        this.onSelectRecordChange.emit(parseInt(rows));
    }

    pageChange(evt) {
        this.onPageChange.emit(evt);
    }
}
