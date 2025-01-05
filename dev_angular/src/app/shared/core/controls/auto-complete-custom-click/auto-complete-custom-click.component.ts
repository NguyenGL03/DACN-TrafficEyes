import { Component, EventEmitter, Injector, Input, OnInit, Output } from "@angular/core";
import { ControlComponent, createCustomInputControlValueAccessor } from "../control.component-old";

@Component({
    selector: "auto-complete-custom-click",
    templateUrl: "./auto-complete-custom-click.component.html",
    providers: [createCustomInputControlValueAccessor(AutoCompleteCustomClickComponent)]
})

export class AutoCompleteCustomClickComponent extends ControlComponent implements OnInit {

    id : string;

    constructor(
        injector: Injector
    ) {
        super(injector);
        this.id = this.generateUUID();
    }

    @Output() onChangeValue: EventEmitter<any> = new EventEmitter<any>();

    //@Output() onSelectValue : EventEmitter<any> = new EventEmitter<any>();

    onChange() {
    }

    _ngModel: string;

    _checked: boolean;

    _list: Array<any>;

    _selectedItem : any;

    @Input() fieldName: string;

    @Input() disabled: string;


    @Input() inputCss: string = 'form-control';

    setNgModelValue(value) {
        this.ngModel = value || '';
    }

    updateControlView() {
        this.inputRef.nativeElement.value = this.ngModel;
    }

    public get list(): Array<any> {
        return this._list;
    }

    @Input() public set list(pList) {
        if (pList) {
            // let autoCompleteArray = pList.map((item) =>({...item, label:item[this.fieldName]}))
            // $(this.inputRef.nativeElement).autocomplete({
            //     // source: pList.map((x) => { return x[this.fieldName] }).filter(x => x),
            //     source: autoCompleteArray,
            //     // select: function(event, ui){
            //     //     this._selectedItem = ui;
            //     // }
            // });

            this._list = pList;
        }
    }

    @Output() @Input() public get ngModel(): string {
        return this._ngModel;
    }
    public set ngModel(value) {
        this._ngModel = value;
    }

    onChangeInput(evt) {
        this.onChangeCallback(evt.target.value);
        this.onChangeValue.emit(this._list.firstOrDefault(x => x[this.fieldName] == evt.target.value));
        //this.onChangeValue.emit(evt);

        //this.onSelectValue.emit(this._selectedItem);
    }

    afterViewInit() {

    }

    ngOnInit(): void {
    }

}
