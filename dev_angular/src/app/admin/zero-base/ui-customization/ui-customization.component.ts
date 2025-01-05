import { Component, ViewEncapsulation, Injector, OnInit } from '@angular/core';
import { AppNavigationService } from '@app/shared/layout/nav/app-navigation.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ThemeSettingsDto, UiCustomizationSettingsServiceProxy, HostSettingsServiceProxy, HostSettingsEditDto } from '@shared/service-proxies/service-proxies';
import { sortBy as _sortBy } from 'lodash-es';

@Component({
    templateUrl: './ui-customization.component.html',
    styleUrls: ['./ui-customization.component.less'],
    animations: [appModuleAnimation()],
    encapsulation: ViewEncapsulation.None,
})
export class UiCustomizationComponent extends AppComponentBase implements OnInit {
    themeSettings: ThemeSettingsDto[];
    public hostSettings: HostSettingsEditDto;
    currentThemeName = '';
    selectedTheme
    constructor(
        injector: Injector,
        private _uiCustomizationService: UiCustomizationSettingsServiceProxy,
        private _hostSettingsService: HostSettingsServiceProxy,
        private appNavigation: AppNavigationService
    ) {
        super(injector);
    }

    getLocalizedThemeName(str: string): string {
        return this.l('Theme ' + abp.utils.toPascalCase(str));
    }

    ngOnInit(): void {
        this.currentThemeName = this.currentTheme.baseSettings.theme;
        this.selectedTheme = this.currentTheme.baseSettings;
        this._uiCustomizationService.getUiManagementSettings().subscribe((settingsResult) => {
            this.themeSettings = _sortBy(settingsResult, (setting) => {
               setting.theme = setting.theme.replace('theme', '')
               return setting.theme === 'default' ? 0 : parseInt(setting.theme.replace('theme', ''))
            });
        });
        this
        // this._hostSettingsService.getAllSettings().subscribe(result => {
        //     this.hostSettings = result;
        // })
        this.appNavigation.getAllSetting().subscribe(result => {
            this.hostSettings = result;
        })
    }
    onSelect(value) {
        this.selectedTheme = value
    }

    saveHostSettings(): void {
        this.notify.success(this.l('SavedSuccessfully'));
        this._hostSettingsService.updateAllSettings(this.hostSettings)
            .subscribe((result) => {
                setTimeout(() => window.location.reload(), 1000);
            });
    }
}
