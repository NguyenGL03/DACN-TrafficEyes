import { Injector, Component, ViewEncapsulation, Inject, Input, OnInit } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DOCUMENT } from '@angular/common';

@Component({
    templateUrl: './theme13-brand.component.html',
    selector: 'theme13-brand',
    encapsulation: ViewEncapsulation.None,
})
export class Theme13BrandComponent extends AppComponentBase implements OnInit {
    @Input() anchorClass = '';
    @Input() imageClass = 'custom-logo logo'; 

    defaultLogo = AppConsts.appBaseUrl + '/assets/common/images/app-logo-on-dark.svg';

    remoteServiceBaseUrl: string = AppConsts.remoteServiceBaseUrl;

    constructor(injector: Injector, @Inject(DOCUMENT) private document: Document) {
        super(injector);
    }

    ngOnInit(): void {
        this.defaultLogo = this.remoteServiceBaseUrl + this.s('gAMSProCore.WebLogo'); 
    }

    triggerAsideToggleClickEvent(): void {
        abp.event.trigger('app.kt_aside_toggler.onClick');
    }
}
