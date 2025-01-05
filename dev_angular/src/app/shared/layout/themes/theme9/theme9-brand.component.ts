import { Injector, Component, ViewEncapsulation, Inject, Input, OnInit } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DOCUMENT } from '@angular/common';

@Component({
    templateUrl: './theme9-brand.component.html',
    selector: 'theme9-brand',
    encapsulation: ViewEncapsulation.None,
})
export class Theme9BrandComponent extends AppComponentBase implements OnInit {

    @Input() imageClass = 'h-40px';

    skin = this.appSession.theme.baseSettings.layout.darkMode ? 'dark' : 'light';
    defaultLogo = AppConsts.appBaseUrl + '/assets/common/images/app-logo-on-' + this.skin + '-sm.svg';
    remoteServiceBaseUrl: string = AppConsts.remoteServiceBaseUrl;

    constructor(injector: Injector, @Inject(DOCUMENT) private document: Document) {
        super(injector);
    }

    ngOnInit(): void {
        this.defaultLogo = this.remoteServiceBaseUrl + this.s('gAMSProCore.WebLogo');
    }
}
