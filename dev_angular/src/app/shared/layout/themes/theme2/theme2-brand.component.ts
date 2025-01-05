import { Injector, Component, ViewEncapsulation, Inject, OnInit } from '@angular/core';

import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';

import { DOCUMENT } from '@angular/common';

@Component({
    templateUrl: './theme2-brand.component.html',
    selector: 'theme2-brand',
    encapsulation: ViewEncapsulation.None,
})
export class Theme2BrandComponent extends AppComponentBase implements OnInit{
    remoteServiceBaseUrl: string = AppConsts.remoteServiceBaseUrl;
    skin = this.currentTheme.baseSettings.layout.darkMode ? 'dark' : 'light';
    defaultLogo = '';
    defaultSmallLogo = '';

    constructor(injector: Injector, @Inject(DOCUMENT) private document: Document) {
        super(injector);
    }
    
    ngOnInit(): void {
        this.setLogoUrl();
    }

    setLogoUrl(): void {
        this.defaultLogo = this.remoteServiceBaseUrl + this.s('gAMSProCore.WebLogo');
        this.defaultSmallLogo = this.remoteServiceBaseUrl + this.s('gAMSProCore.SmallWebLogo');
    }
}
