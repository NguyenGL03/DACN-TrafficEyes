import { AfterViewChecked, AfterViewInit, Component, EventEmitter, Injector, Input, Output, ViewEncapsulation } from '@angular/core';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { MenuItem } from 'primeng/api';


@Component({
    selector: 'import-button',
    templateUrl: './import-button.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ImportButtonComponent extends DefaultComponentBase {
    @Input() id: string = 'file-upload';
    @Input() items: MenuItem[];
    @Input() label: string;
    @Input() remoteFileDown: string;
    @Input() disabled: boolean = false;

    @Output() onImportExcel: EventEmitter<any> = new EventEmitter<any>();

    constructor(injector: Injector) {
        super(injector);
        this.items = [
            {
                label: this.l('DownloadSampleFile'),
                target: '_blank',
                icon: 'pi pi-fw pi-download',
                command: () => {
                    window.location.href = this.getRemoteFile(this.remoteFileDown)
                }
            }
        ];
    }

    handleOnClick() {
        const element: HTMLElement = document.querySelector(`input[type="file"]#${this.id}`) as HTMLElement;
        element?.click();
    }
} 