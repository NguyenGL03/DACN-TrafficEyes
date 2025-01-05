import { Component, ElementRef, Injector, Input, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { UiFileUploaderImageComponent } from './ui-file-uploader.component';
import { HostSettingsEditDto } from '@shared/service-proxies/service-proxies';
import { HostSettingsServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './ui-customization-extend.component.html',
    animations: [appModuleAnimation()],
    encapsulation: ViewEncapsulation.None,
    selector: 'ui-customization-extend'
})
export class UiCustomizationExtendComponent extends AppComponentBase {
    @Input() hostSettings: HostSettingsEditDto;
    remoteServiceBaseUrl: string;
    appBaseUrl: string;

    constructor(injector: Injector, private _hostSettingService: HostSettingsServiceProxy) {
        super(injector);
        this.remoteServiceBaseUrl = AppConsts.remoteServiceBaseUrl;
        this.appBaseUrl = AppConsts.appBaseUrl;
    }

    onImgError(event: any) {
        event.target.src = this.appBaseUrl + '/assets/common/images/Image_not_available.png'
    }

    changeColorIuColorPicker(name, value){
        this.hostSettings.commonThemeSettings[name] = value
    }

    saveHostSettings(): void {
        this.notify.success(this.l('SavedSuccessfully'));
        this._hostSettingService.updateAllSettings(this.hostSettings)
            .subscribe((result) => {
                setTimeout(() => window.location.reload(), 1000);
            });
    }
}
