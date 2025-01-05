import { AfterViewInit, Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { PopupFrameComponent } from '@app/shared/common/modals/popup-frames/popup-frame.component';
import { DefaultComponentBase } from '@app/utilities/default-component-base';

@Component({ template: '' })
export abstract class PopupBaseDefaultComponent extends DefaultComponentBase   {
    @Input() title: string; 

    @Output() onSelect: EventEmitter<any> = new EventEmitter<any>;

    @ViewChild('popupFrame') popupFrame: PopupFrameComponent;

    visible: boolean = false;
    loadedData: boolean = false;

    constructor(injector: Injector) {
        super(injector);
    }  
}
