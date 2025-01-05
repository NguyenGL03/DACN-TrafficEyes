import { HttpHandler } from '@angular/common/http';
import { Component, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { ComponentBase } from '@app/utilities/component-base';
import { HttpClient } from '@microsoft/signalr';
import moment from 'moment';
import * as XLSX from 'xlsx';


@Component({
    selector: 'import-excel',
    templateUrl: './import-excel.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ImportExcelComponent extends ComponentBase {

    constructor(injector: Injector) {
        super(injector);
    }

    handler: HttpHandler;
    http: HttpClient;
    arrayBuffer: any;
    @Input() inputCss: string;
    @Input() customStyle: string;
    @Input() hidden: boolean = false;
    @Input() disable: boolean = false;
    @Input() id: string = 'file';
    @Input() startPosition: string = '';
    @Input() workSheetName: string = '';
    @Input() fileExtension: string = '.xlsx,.xls';

    file: File;

    @Input() validateImportExcelMessage: string = null

    @Output() toObjects: EventEmitter<any> = new EventEmitter<any>();
    @Output() toArrayObject: EventEmitter<any> = new EventEmitter<any>();
    @Output() fileInfo: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('fileControl') fileControl: any


    fileInputClick(event: any) {
        if (this.validateImportExcelMessage) {
            this.showWarningMessage(this.validateImportExcelMessage)
            event.preventDefault();
            event.stopPropagation();
            return
        }

        this.fileControl.value = null
    }



    onUploadFile(fileInput: any) {

        this.setLoadingUI(true);

        if (fileInput.target.files && fileInput.target.files[0]) {
            if (this.checkUploadedFile(fileInput)) {
                this.file = fileInput.target.files[0];
                this.readUploadedFile();
            }
            else {
                this.showErrorMessage(this.l('FileNotCorrect'));
            }
        }
    }

    checkUploadedFile(fileInput: any): boolean {
        var ext: string[] = fileInput.target.files[0].name.split('.');
        return !!this.fileExtension.split(',').find(x => x == '.' + ext[ext.length - 1]);
    }

    readUploadedFile() {
        setTimeout(() => {
            try {
                let fileReader = new FileReader();
                fileReader.readAsArrayBuffer(this.file);
                fileReader.onload = () => {
                    this.arrayBuffer = fileReader.result;
                    var data = new Uint8Array(this.arrayBuffer);
                    let arr = new Array();
                    for (let i = 0; i != data.length; ++i)
                        arr[i] = String.fromCharCode(data[i]);
                    let bstr = arr.join("");
                    console.time('reading');
                    let workbook = XLSX.read(bstr, { type: 'binary', cellFormula: false, cellHTML: false, cellText: false });
                    console.timeEnd('reading');

                    let sheetName = this.workSheetName ? this.workSheetName : workbook.SheetNames[0];
                    let worksheet = workbook.Sheets[sheetName];

                    let datas: IterableIterator<Object> = this.iiCreateIteratorWithStartPosition(this.getObjectKey(worksheet), worksheet, this.startPosition);

                    this.toArrayObject.emit(this._toIterableArrObject(datas));

                    this.fileInfo.emit({ data: this._toIterableArrObject(datas), file: this.file });
                };
            }
            catch (err) {
                this.showErrorMessage(err);
            }
            finally {
                this.clearInputFile();
            }
        });
    }

    getObjectKey(obj: Object): string[] {
        let arr = Object.keys(obj).filter(x => !x.startsWith('!'));
        if (arr.length == 0) {
            return [];
        }
        let rowsLast = this.splitRowIndex(arr[arr.length - 1]);
        let rowsFirst = this.splitRowIndex(arr[0]);
        let columns = arr.map(x => x.replace(this.splitRowIndex(x).toString(), '')).filter((value, index, self) => self.indexOf(value) === index);

        columns = columns.sort(function (x, y) {
            if (x.length > y.length) {
                return 1;
            }

            if (x.length < y.length) {
                return -1;
            }

            return x > y ? 1 : -1;
        });

        var result: string[] = [];
        for (let rowI: any = rowsFirst; rowI <= rowsLast; rowI++) {
            for (let column of columns) {
                result.push(column + rowI);
            }
        }
        return result;
    }


    *_toIterableArrObject(datas: IterableIterator<Object>): IterableIterator<Array<Object>> {
        if (!datas)
            return null;

        let curIterator = datas.next();
        if (curIterator.done)
            return null

        let _obj: Object[] = [];

        let oldInt: Number = 0;
        oldInt = this.splitRowIndex(Object.keys(curIterator.value)[0]);; // old value of row index


        let newInt: Number = 0;

        while (!curIterator.done) {
            newInt = this.splitRowIndex(Object.keys(curIterator.value)[0]);

            if (newInt == oldInt) {
                _obj.push(curIterator.value);
            }
            else {
                oldInt = newInt; // update oldInt
                yield _obj
                _obj = [curIterator.value]; // reset _obj array for pushing new object
            }
            curIterator = datas.next();

            if (curIterator.done) {
                yield _obj
                break;
            }

        }
    }

    _toArrayObject(datas: IterableIterator<Object>): Object[][] {
        if (!datas)
            return null;

        let curIterator = datas.next();
        if (curIterator.done)
            return null

        let __obj: Object[][] = [];
        let _obj: Object[] = [];



        let oldInt: Number = 0;
        oldInt = this.splitRowIndex(Object.keys(curIterator.value)[0]);; // old value of row index


        let newInt: Number = 0;

        while (true) {
            newInt = this.splitRowIndex(Object.keys(curIterator.value)[0]);

            if (newInt == oldInt) {
                _obj.push(curIterator.value);
            }
            else {
                oldInt = newInt; // update oldInt
                __obj.push(_obj);
                _obj = [curIterator.value]; // reset _obj array for pushing new object
            }
            curIterator = datas.next();

            if (curIterator.done) {
                __obj.push(_obj);
                break;
            }

        }

        return __obj;
    }

    private splitRowIndex(key: string): Number {
        return Number.parseInt((key.length == 2) ? key[1] : key.match(/(\d+)/)[0]);
        // if (key.length == 2)
        //     return Number.parseInt(key.slice(1));
        // return Number.parseInt(key.match(/(\d+)/)[0]);
    }

    //khangth,1/11/2019, replace generator function for createNewObject BEGIN
    *iiCreateNewObject(keys: string[], obj: Object): IterableIterator<Object> {
        for (let key of keys) {
            let o: Object = {};
            o[key] = obj[key] ? obj[key]["v"] : undefined;
            yield o
        }
    }
    createNewObject(keys: string[], obj: Object): Object[] {

        let _obj: Object[] = []
        let iterable = this.iiCreateNewObject(keys, obj)
        let it = iterable.next()
        while (!it.done) {
            _obj.push(it.value)
            it = iterable.next()
        }

        return _obj;
    }

    *iiCreateIteratorWithStartPosition(keys: string[], obj: Object, startPosition: string): IterableIterator<Object> {
        if (keys.length == 0)
            return null;
        if (!startPosition)
            return this.createNewObject(keys, obj)[0];

        let indexOfStartPos = keys.findIndex(key => key == startPosition);
        if (indexOfStartPos < 0)
            return this.createNewObject(keys, obj)[0];


        for (let i = indexOfStartPos; i < keys.length; i++) {
            let o: Object = {};
            let value = obj[keys[i]] ? obj[keys[i]]["v"] : undefined;
            if (value instanceof Date) {
                value = moment(value);
            }
            o[keys[i]] = value;
            yield o
        }
    }

    createNewObjectWithStartPosition(keys: string[], obj: Object, startPosition: string): Object[] {
        let _obj: Object[] = []
        let iterable = this.iiCreateIteratorWithStartPosition(keys, obj, startPosition)
        let it = iterable.next()
        while (!it.done) {
            _obj.push(it.value)
            it = iterable.next()
        }
        return _obj;
    }

    clearInputFile(): void {
        if ((<HTMLInputElement>document.getElementById('file-upload')) == null)
            return;
        (<HTMLInputElement>document.getElementById('file-upload')).value = '';
    }

    setLoadingUI(bool: boolean = true, loadingText: string = ''): void {
        // if (bool)
        //     abp.ui.setBusy(undefined, loadingText, undefined);
        // else
        //     abp.ui.clearBusy();
    }
} 