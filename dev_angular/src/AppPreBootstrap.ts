import { CompilerOptions, Injector, NgModuleRef, Type } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppAuthService } from '@app/shared/common/auth/app-auth.service';
import { AppConsts } from '@shared/AppConsts';
import { DynamicResourcesHelper } from '@shared/helpers/DynamicResourcesHelper';
import { SubdomainTenancyNameFinder } from '@shared/helpers/SubdomainTenancyNameFinder';
import { XmlHttpRequestHelper } from '@shared/helpers/XmlHttpRequestHelper';
import { LocaleMappingService } from '@shared/locale-mapping.service';
import { CookieTenantResolver } from '@shared/multi-tenancy/tenant-resolvers/cookie-tenant-resolver';
import { QueryStringTenantResolver } from '@shared/multi-tenancy/tenant-resolvers/query-string-tenant-resolver';
import { SubdomainTenantResolver } from '@shared/multi-tenancy/tenant-resolvers/subdomain-tenant-resolver';
import {
    AccountServiceProxy,
    HostSettingsServiceProxy,
    IsTenantAvailableInput,
    IsTenantAvailableOutput,
    TenantAvailabilityState,
} from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import { merge as _merge } from 'lodash-es';
import { DateTime, Settings } from 'luxon';
import { environment } from './environments/environment';
import { UrlHelper } from './shared/helpers/UrlHelper';
import { ThemeSettingConsts } from '@shared/ThemeSettingConsts';
import { CustomNotifyService } from '@shared/common/ui/custom-notify.service';

export class AppPreBootstrap {
    static run(appRootUrl: string, injector: Injector, callback: () => void, resolve: any, reject: any): void {
        AppPreBootstrap.getApplicationConfig(appRootUrl, injector, () => {
            if (UrlHelper.isInstallUrl(location.href)) {
                AppPreBootstrap.loadAssetsForInstallPage(callback);
                return;
            }

            const queryStringObj = UrlHelper.getQueryParameters();

            if (queryStringObj.redirect && queryStringObj.redirect === 'TenantRegistration') {
                if (queryStringObj.forceNewRegistration) {
                    new AppAuthService().logout();
                }

                location.href = AppConsts.appBaseUrl + '/account/select-edition';
            } else if (queryStringObj.impersonationToken) {
                if (queryStringObj.userDelegationId) {
                    AppPreBootstrap.delegatedImpersonatedAuthenticate(
                        queryStringObj.userDelegationId,
                        queryStringObj.impersonationToken,
                        queryStringObj.tenantId,
                        () => {
                            AppPreBootstrap.getUserConfiguration(callback);
                        }
                    );
                } else {
                    AppPreBootstrap.impersonatedAuthenticate(
                        queryStringObj.impersonationToken,
                        queryStringObj.tenantId,
                        () => {
                            AppPreBootstrap.getUserConfiguration(callback);
                        }
                    );
                }
            } else if (queryStringObj.switchAccountToken) {
                AppPreBootstrap.linkedAccountAuthenticate(
                    queryStringObj.switchAccountToken,
                    queryStringObj.tenantId,
                    () => {
                        AppPreBootstrap.getUserConfiguration(callback);
                    }
                );
            } else {
                AppPreBootstrap.getUserConfiguration(callback);
            }
        });
    }

    static bootstrap<TM>(
        moduleType: Type<TM>,
        compilerOptions?: CompilerOptions | CompilerOptions[]
    ): Promise<NgModuleRef<TM>> {
        return platformBrowserDynamic().bootstrapModule(moduleType, compilerOptions);
    }

    public static resolveTenancyName(appBaseUrl): string {
        let subdomainTenantResolver = new SubdomainTenantResolver();
        let tenancyName = subdomainTenantResolver.resolve(appBaseUrl);
        if (tenancyName) {
            return tenancyName;
        }

        let queryStringTenantResolver = new QueryStringTenantResolver();
        tenancyName = queryStringTenantResolver.resolve();
        if (tenancyName) {
            abp.utils.setCookieValue('abp_tenancy_name', tenancyName);
            return tenancyName;
        }

        let cookieTenantResolver = new CookieTenantResolver();
        tenancyName = cookieTenantResolver.resolve();

        return tenancyName;
    }

    private static getApplicationConfig(appRootUrl: string, injector: Injector, callback: () => void) {
        let type = 'GET';
        let url = appRootUrl + 'assets/' + environment.appConfig;
        let customHeaders = [
            {
                name: abp.multiTenancy.tenantIdCookieName,
                value: abp.multiTenancy.getTenantIdCookie() + '',
            },
        ];

        XmlHttpRequestHelper.ajax(type, url, customHeaders, null, (result) => {

            AppConsts.localeMappings = result.localeMappings;
            AppConsts.appBaseUrlFormat = result.appBaseUrl;
            AppConsts.remoteServiceBaseUrlFormat = result.remoteServiceBaseUrl;

            let tenancyName = AppPreBootstrap.resolveTenancyName(result.appBaseUrl);
            AppPreBootstrap.configureAppUrls(tenancyName, result.appBaseUrl, result.remoteServiceBaseUrl);

            if (AppConsts.PreventNotExistingTenantSubdomains) {
                let subdomainTenancyNameFinder = new SubdomainTenancyNameFinder();
                if (subdomainTenancyNameFinder.urlHasTenancyNamePlaceholder(result.remoteServiceBaseUrl)) {
                    const message = abp.localization.localize('ThereIsNoTenantDefinedWithName{0}', AppConsts.localization.defaultLocalizationSourceName);
                    abp.message.warn(abp.utils.formatString(message, tenancyName));
                    document.location.href = result.remoteServiceBaseUrl.replace(
                        AppConsts.tenancyNamePlaceHolderInUrl + '.',
                        ''
                    );
                    return;
                }
            }

            if (tenancyName == null) {
                callback();
            } else {
                AppPreBootstrap.ConfigureTenantIdCookie(injector, tenancyName, callback);
            }
        });
    }

    private static ConfigureTenantIdCookie(injector, tenancyName: string, callback: () => void) {

        let accountServiceProxy: AccountServiceProxy = injector.get(AccountServiceProxy);
        let input = new IsTenantAvailableInput();
        input.tenancyName = tenancyName;

        accountServiceProxy.isTenantAvailable(input).subscribe((result: IsTenantAvailableOutput) => {
            if (result.state === TenantAvailabilityState.Available) {
                abp.multiTenancy.setTenantIdCookie(result.tenantId);
            }

            callback();
        });
    }

    private static configureAppUrls(tenancyName: string, appBaseUrl: string, remoteServiceBaseUrl: string): void {
        if (tenancyName == null) {
            AppConsts.appBaseUrl = appBaseUrl.replace(AppConsts.tenancyNamePlaceHolderInUrl + '.', '');
            AppConsts.remoteServiceBaseUrl = remoteServiceBaseUrl.replace(
                AppConsts.tenancyNamePlaceHolderInUrl + '.',
                ''
            );
        } else {
            AppConsts.appBaseUrl = appBaseUrl.replace(AppConsts.tenancyNamePlaceHolderInUrl, tenancyName);
            AppConsts.remoteServiceBaseUrl = remoteServiceBaseUrl.replace(
                AppConsts.tenancyNamePlaceHolderInUrl,
                tenancyName
            );
        }
    }

    private static getCurrentClockProvider(currentProviderName: string): abp.timing.IClockProvider {
        if (currentProviderName === 'unspecifiedClockProvider') {
            return abp.timing.unspecifiedClockProvider;
        }

        if (currentProviderName === 'utcClockProvider') {
            return abp.timing.utcClockProvider;
        }

        return abp.timing.localClockProvider;
    }

    private static getRequetHeadersWithDefaultValues() {
        const cookieLangValue = abp.utils.getCookieValue('Abp.Localization.CultureName');

        let requestHeaders = {
            '.AspNetCore.Culture': 'c=' + cookieLangValue + '|uic=' + cookieLangValue,
            [abp.multiTenancy.tenantIdCookieName]: abp.multiTenancy.getTenantIdCookie(),
        };

        if (!cookieLangValue) {
            delete requestHeaders['.AspNetCore.Culture'];
        }

        return requestHeaders;
    }

    private static impersonatedAuthenticate(impersonationToken: string, tenantId: number, callback: () => void): void {
        abp.multiTenancy.setTenantIdCookie(tenantId);
        let requestHeaders = AppPreBootstrap.getRequetHeadersWithDefaultValues();

        XmlHttpRequestHelper.ajax(
            'POST',
            AppConsts.remoteServiceBaseUrl +
            '/api/TokenAuth/ImpersonatedAuthenticate?impersonationToken=' +
            impersonationToken,
            requestHeaders,
            null,
            (response) => {
                let result = response.result;
                abp.auth.setToken(result.accessToken);
                AppPreBootstrap.setEncryptedTokenCookie(result.encryptedAccessToken, () => {
                    callback();
                    location.search = '';
                });
            }
        );
    }

    private static delegatedImpersonatedAuthenticate(
        userDelegationId: number,
        impersonationToken: string,
        tenantId: number,
        callback: () => void
    ): void {
        abp.multiTenancy.setTenantIdCookie(tenantId);
        let requestHeaders = AppPreBootstrap.getRequetHeadersWithDefaultValues();

        XmlHttpRequestHelper.ajax(
            'POST',
            AppConsts.remoteServiceBaseUrl +
            '/api/TokenAuth/DelegatedImpersonatedAuthenticate?userDelegationId=' +
            userDelegationId +
            '&impersonationToken=' +
            impersonationToken,
            requestHeaders,
            null,
            (response) => {
                let result = response.result;
                abp.auth.setToken(result.accessToken);
                AppPreBootstrap.setEncryptedTokenCookie(result.encryptedAccessToken, () => {
                    callback();
                    location.search = '';
                });
            }
        );
    }

    private static linkedAccountAuthenticate(switchAccountToken: string, tenantId: number, callback: () => void): void {
        abp.multiTenancy.setTenantIdCookie(tenantId);
        let requestHeaders = AppPreBootstrap.getRequetHeadersWithDefaultValues();

        XmlHttpRequestHelper.ajax(
            'POST',
            AppConsts.remoteServiceBaseUrl +
            '/api/TokenAuth/LinkedAccountAuthenticate?switchAccountToken=' +
            switchAccountToken,
            requestHeaders,
            null,
            (response) => {
                let result = response.result;
                abp.auth.setToken(result.accessToken);
                AppPreBootstrap.setEncryptedTokenCookie(result.encryptedAccessToken, () => {
                    callback();
                    location.search = '';
                });
            }
        );
    }

    private static getUserConfiguration(callback: () => void): any {
        const token = abp.auth.getToken();

        let requestHeaders = AppPreBootstrap.getRequetHeadersWithDefaultValues();

        if (token) {
            requestHeaders['Authorization'] = 'Bearer ' + token;
        }

        return XmlHttpRequestHelper.ajax(
            'GET',
            AppConsts.remoteServiceBaseUrl + '/AbpUserConfiguration/GetAll',
            requestHeaders,
            null,
            (response) => {
                let result = response.result;

                _merge(abp, result);

                abp.clock.provider = this.getCurrentClockProvider(result.clock.provider);

                AppPreBootstrap.configureLuxon();

                abp.event.trigger('abp.dynamicScriptsInitialized');

                AppConsts.recaptchaSiteKey = abp.setting.get('Recaptcha.SiteKey');
                AppConsts.subscriptionExpireNootifyDayCount = parseInt(
                    abp.setting.get('App.TenantManagement.SubscriptionExpireNotifyDayCount')
                );
                DynamicResourcesHelper.loadResources(callback);
                this.setTheme();
            }
        );
    }
    static _hostSettingsService: HostSettingsServiceProxy;
    static customNotifyService: CustomNotifyService;
    static initializeServices(injector: Injector) {
        this.customNotifyService = injector.get(CustomNotifyService);
    }

    static setTheme() {
        const currentTheme = abp.setting.get('App.UiManagement.Theme');
        // const darkMode = abp.setting.get(currentTheme + '.App.UiManagement.DarkMode');
        // const isDarkMode = JSON.parse(darkMode.toLowerCase());

        // const themeElement = document.querySelector(`[data-bs-theme="${isDarkMode ? 'dark' : 'light'}"]`) as HTMLElement;
        // if (!themeElement) return;

        // const width = abp.setting.get(ThemeSettingConsts.WebLogoWidth);
        // const height = abp.setting.get(ThemeSettingConsts.WebLogoHeight);

        // themeElement.style.setProperty('--w-logo', width ? width + 'px' : 'auto');
        // themeElement.style.setProperty('--h-logo', height ? height + 'px' : 'auto');
        // themeElement.style.setProperty('--bs-bg-logo', abp.setting.get(ThemeSettingConsts.WebLogoBackground) || 'transparent');

        const token = abp.auth.getToken();
        let requestHeaders = AppPreBootstrap.getRequetHeadersWithDefaultValues();
        if (token) {
            requestHeaders['Authorization'] = 'Bearer ' + token;

            return XmlHttpRequestHelper.ajax(
                'GET',
                AppConsts.remoteServiceBaseUrl + '/api/services/app/HostSettings/GetAllSettings',
                requestHeaders,
                null,
                (response) => {
                    let result = response.result;
                    const darkMode = abp.setting.get(currentTheme + '.App.UiManagement.DarkMode');
                    const isDarkMode = JSON.parse(darkMode.toLowerCase());
                    // setting theo cai dat giao dien
                    // #region start
                    if (isDarkMode) {
                        // main
                        this.setPrimaryColor('--bs-app-border-color', result.commonThemeSettings.darkBorderColor)
                        this.setPrimaryColor('--border-color', result.commonThemeSettings.darkBorderColor)
                        this.setPrimaryColor('.toolbar-component__button svg path', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--svg-color', result.commonThemeSettings.darkSvgColor)
                        this.setPrimaryColor('--text-color', result.commonThemeSettings.darkTextColor)
                        this.setPrimaryColor('--bs-text-btn-color', result.commonThemeSettings.darkTextColor)
                        this.setPrimaryColor('--bs-text-color', result.commonThemeSettings.darkTextColor)
                        this.setPrimaryColor('--bs-text-sub-color', result.commonThemeSettings.darkTextColor)

                        this.setPrimaryColor('--bs-primary', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-primary-active', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--primary-color', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-prismjs-btn-color-hover', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-text-primary', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-component-active-bg', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-component-hover-color', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-component-checked-bg', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-hover', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-show', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-here', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-active', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-ribbon-label-bg', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--bs-link-color', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--kt-progress-bar-bg', result.commonThemeSettings.darkPrimaryColor)
                        this.setPrimaryColor('--web-bg-color', result.commonThemeSettings.darkWebBackground)

                        // sidebar
                        this.setPrimaryColor('--bs-app-sidebar-light-menu-link-color', result.commonThemeSettings.darkSidebarColorText)
                        this.setPrimaryColor('--bs-app-sidebar-light-menu-link-color-active', result.commonThemeSettings.darkSidebarColorTextActive)
                        this.setPrimaryColor('--sidebar-child-bg-color', result.commonThemeSettings.darkSideBarChildBgColor)

                        this.setPrimaryColor('--primary-bg', this.rgbaToHex(result.commonThemeSettings.darkPrimaryColor));

                        this.customNotifyService.setDynamicStyle('aside-menu .menu .menu-item .menu-link.active ', result.commonThemeSettings.darkPrimaryColor);

                        this.customNotifyService.setDynamicStyle('custom-success-toast', result.commonThemeSettings.darkSuccessColor);
                        this.customNotifyService.setDynamicStyle('custom-warn-toast', result.commonThemeSettings.darkWarningColor);
                        this.customNotifyService.setDynamicStyle('custom-error-toast', result.commonThemeSettings.darkDangerColor);

                        this.customNotifyService.setDynamicStyle('header-brand', result.commonThemeSettings.darkSidebarBackground);
                        this.customNotifyService.setDynamicStyle('aside', result.commonThemeSettings.darkSidebarBackground);
                        this.customNotifyService.setDynamicStyle('app-sidebar-menu', result.commonThemeSettings.darkSidebarBackground);
                        this.customNotifyService.setDynamicStyle('aside-logo', result.commonThemeSettings.darkSidebarBackground);
                        this.customNotifyService.setDynamicStyle('app-sidebar-logo', result.commonThemeSettings.darkSidebarBackground);

                        this.customNotifyService.setDynamicStyle('header', result.commonThemeSettings.darkHeaderBackground);
                        // this.customNotifyService.setDynamicStyle('app-header', result.commonThemeSettings.darkHeaderBackground);

                        // card
                        this.customNotifyService.setDynamicStyle('card.card-custom', result.commonThemeSettings.darkCardBackground);
                        this.setPrimaryColor('--card-bg-color', result.commonThemeSettings.darkCardBackground)
                        // header
                        this.setPrimaryColor('--bs-app-header-base-bg-color', result.commonThemeSettings.darkHeaderBackground)
                        this.setPrimaryColor('--bs-app-header-border', result.commonThemeSettings.darkHeaderBorderColor)
                        this.setPrimaryColor('--bs-app-header-logo-border', result.commonThemeSettings.darkHeaderLogoBorderColor)
                        this.setPrimaryColor('--bs-app-header-logo-bg', result.commonThemeSettings.darkHeaderLogoBgColor)
                        this.setPrimaryColor('--bs-app-header-w-border', result.commonThemeSettings.darkHeaderWrapperBorderColor)
                        this.setPrimaryColor('--bs-app-header-w-bg', result.commonThemeSettings.darkHeaderWrapperBgColor)
                        this.setPrimaryColor('--bs-title-name-color', result.commonThemeSettings.darkHeaderColorText)
                        //table
                        this.setPrimaryColor('--table-head-bg-color', result.commonThemeSettings.darkTableHeadBgColor)
                        this.setPrimaryColor('--table-body-odd-bg-color', result.commonThemeSettings.darkTableBodyOddBgColor)
                        this.setPrimaryColor('--table-body-even-bg-color', result.commonThemeSettings.darkTableBodyEvenBgColor)
                        this.setPrimaryColor('--table-main-color', result.commonThemeSettings.darkTableMainColor)
                        this.setPrimaryColor('--table-body-text-color', result.commonThemeSettings.darkTableBodyTextColor)
                        this.setPrimaryColor('--table-border-color', result.commonThemeSettings.darkTableBorderColor)
                        this.setPrimaryColor('--table-highlight-color', result.commonThemeSettings.darkTableHighlightColor)
                        //input
                        this.setPrimaryColor('--input-label-color', result.commonThemeSettings.darkInputLabelColor)
                        this.setPrimaryColor('--input-border-color', result.commonThemeSettings.darkInputBorderColor)
                        this.setPrimaryColor('--input-text-color', result.commonThemeSettings.darkInputTextColor)
                        this.setPrimaryColor('--input-icon-color', result.commonThemeSettings.darkInputIconColor)
                        this.setPrimaryColor('--input-bg-color', result.commonThemeSettings.darkInputBgcolor)
                        //toolbar
                        this.setPrimaryColor('--btn-toolbar-bg-color', result.commonThemeSettings.darkBtnToolbarBgColor)
                        this.setPrimaryColor('--btn-toolbar-text-color', result.commonThemeSettings.darkBtnToolbarTextColor)
                        this.setPrimaryColor('--btn-toolbar-border-color', result.commonThemeSettings.darkBtnToolbarBorderColor)
                    }
                    else {
                        // main
                        this.setPrimaryColor('--bs-app-border-color', result.commonThemeSettings.borderColor)
                        this.setPrimaryColor('--svg-color', result.commonThemeSettings.svgColor)
                        this.setPrimaryColor('--text-color', result.commonThemeSettings.textColor)
                        this.setPrimaryColor('--bs-text-btn-color', result.commonThemeSettings.textColor)
                        this.setPrimaryColor('--bs-text-color', result.commonThemeSettings.textColor)
                        this.setPrimaryColor('--bs-text-sub-color', result.commonThemeSettings.textColor)

                        this.setPrimaryColor('--bs-primary', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--primary-color', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-primary-active', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-prismjs-btn-color-hover', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-text-primary', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-component-active-bg', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-component-hover-color', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-component-checked-bg', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-hover', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-show', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-here', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-menu-link-color-active', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-ribbon-label-bg', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--bs-link-color', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--kt-progress-bar-bg', result.commonThemeSettings.primaryColor)
                        this.setPrimaryColor('--web-bg-color', result.commonThemeSettings.webBackground)

                        // sidebar
                        this.setPrimaryColor('--bs-app-sidebar-light-menu-link-color', result.commonThemeSettings.sidebarColorText)
                        this.setPrimaryColor('--bs-app-sidebar-light-menu-link-color-active', result.commonThemeSettings.sidebarColorTextActive)
                        this.setPrimaryColor('--sidebar-child-bg-color', result.commonThemeSettings.sideBarChildBgColor)


                        this.setPrimaryColor('--primary-bg', this.rgbaToHex(result.commonThemeSettings.primaryColor));

                        this.customNotifyService.setDynamicStyle('aside-menu .menu .menu-item .menu-link.active ', result.commonThemeSettings.primaryColor);


                        this.customNotifyService.setDynamicStyle('wrapper', result.commonThemeSettings.webBackground);

                        this.customNotifyService.setDynamicStyle('custom-success-toast', result.commonThemeSettings.successColor);
                        this.customNotifyService.setDynamicStyle('custom-warn-toast', result.commonThemeSettings.warningColor);
                        this.customNotifyService.setDynamicStyle('custom-error-toast', result.commonThemeSettings.dangerColor);


                        this.setPrimaryColor('--bs-card-color', result.commonThemeSettings.cardBackground)

                        this.customNotifyService.setDynamicStyle('header-brand', result.commonThemeSettings.sidebarBackground);
                        this.customNotifyService.setDynamicStyle('aside', result.commonThemeSettings.sidebarBackground);
                        this.customNotifyService.setDynamicStyle('app-sidebar-menu', result.commonThemeSettings.sidebarBackground);
                        this.customNotifyService.setDynamicStyle('aside-logo', result.commonThemeSettings.sidebarBackground);
                        this.customNotifyService.setDynamicStyle('app-sidebar-logo', result.commonThemeSettings.sidebarBackground);

                        this.customNotifyService.setDynamicStyle('header', result.commonThemeSettings.headerBackground);
                        // this.customNotifyService.setDynamicStyle('app-header', result.commonThemeSettings.headerBackground);

                        // card
                        this.customNotifyService.setDynamicStyle('card.card-custom', result.commonThemeSettings.cardBackground);
                        this.setPrimaryColor('--card-bg-color', result.commonThemeSettings.cardBackground)
                        // header
                        this.setPrimaryColor('--bs-app-header-base-bg-color', result.commonThemeSettings.headerBackground)
                        this.setPrimaryColor('--bs-app-header-border', result.commonThemeSettings.headerBorderColor)
                        this.setPrimaryColor('--bs-app-header-logo-border', result.commonThemeSettings.headerLogoBorderColor)
                        this.setPrimaryColor('--bs-app-header-logo-bg', result.commonThemeSettings.headerLogoBgColor)
                        this.setPrimaryColor('--bs-app-header-w-border', result.commonThemeSettings.headerWrapperBorderColor)
                        this.setPrimaryColor('--bs-app-header-w-bg', result.commonThemeSettings.headerWrapperBgColor)
                        this.setPrimaryColor('--bs-title-name-color', result.commonThemeSettings.headerColorText)
                        //table
                        this.setPrimaryColor('--table-head-bg-color', result.commonThemeSettings.tableHeadBgColor)
                        this.setPrimaryColor('--table-body-odd-bg-color', result.commonThemeSettings.tableBodyOddBgColor)
                        this.setPrimaryColor('--table-body-even-bg-color', result.commonThemeSettings.tableBodyEvenBgColor)
                        this.setPrimaryColor('--table-main-color', result.commonThemeSettings.tableMainColor)
                        this.setPrimaryColor('--table-body-text-color', result.commonThemeSettings.tableBodyTextColor)
                        this.setPrimaryColor('--table-border-color', result.commonThemeSettings.tableBorderColor)
                        this.setPrimaryColor('--table-highlight-color', result.commonThemeSettings.tableHighlightColor)
                        //input
                        this.setPrimaryColor('--input-label-color', result.commonThemeSettings.inputLabelColor)
                        this.setPrimaryColor('--input-border-color', result.commonThemeSettings.inputBorderColor)
                        this.setPrimaryColor('--input-text-color', result.commonThemeSettings.inputTextColor)
                        this.setPrimaryColor('--input-icon-color', result.commonThemeSettings.inputIconColor)
                        this.setPrimaryColor('--input-bg-color', result.commonThemeSettings.inputBgcolor)
                        //toolbar
                        this.setPrimaryColor('--btn-toolbar-bg-color', result.commonThemeSettings.btnToolbarBgColor)
                        this.setPrimaryColor('--btn-toolbar-text-color', result.commonThemeSettings.btnToolbarTextColor)
                        this.setPrimaryColor('--btn-toolbar-border-color', result.commonThemeSettings.btnToolbarBorderColor)
                    }

                    // #region end

                    // setting theo design 7/24/2024

                }
            );
        }
    }

    static hexToRgba(hex: string, alpha: number = 1): string {
        hex = hex.replace(/^#/, '');
        let r = 0, g = 0, b = 0;

        if (hex.length === 6) {
            r = parseInt(hex.substring(0, 2), 16);
            g = parseInt(hex.substring(2, 4), 16);
            b = parseInt(hex.substring(4, 6), 16);
        }

        return `rgba(${r}, ${g}, ${b}, ${alpha / 100})`;
    }

    static rgbaToHex(rgba: string) {
        if (rgba) {
            const result = rgba.match(/rgba?\((\d+),\s*(\d+),\s*(\d+),\s*(\d*\.?\d+)\)/);
            if (result) {
                const r = parseInt(result[1], 10);
                const g = parseInt(result[2], 10);
                const b = parseInt(result[3], 10);
                const a = Math.round(parseFloat(result[4]) * 255);
                const toHex = (n: number): string => n.toString(16).padStart(2, '0');

               return this.hexToRgba(`#${toHex(r)}${toHex(g)}${toHex(b)}`, 10)
            }
            else {
                return rgba
            }
        }
    }

    static setPrimaryColor(variableName, newValue) {
        document.documentElement.style.setProperty(variableName, newValue);
    }

    private static configureLuxon() {
        let luxonLocale = new LocaleMappingService().map('luxon', abp.localization.currentLanguage.name);

        DateTime.local().setLocale(luxonLocale);
        DateTime.utc().setLocale(luxonLocale);
        Settings.defaultLocale = luxonLocale;

        if (abp.clock.provider.supportsMultipleTimezone) {
            Settings.defaultZone = abp.timing.timeZoneInfo.iana.timeZoneId;
        }

        DateTime.prototype.toJSON = function () {
            if (!abp.clock.provider.supportsMultipleTimezone) {
                let localDate = this.setLocale('en');
                return localDate.toString();
            }

            let date = this.setLocale('en').setZone(abp.timing.timeZoneInfo.iana.timeZoneId) as DateTime;
            return date.toISO();
        };
    }

    private static setEncryptedTokenCookie(encryptedToken: string, callback: () => void) {
        new LocalStorageService().setItem(
            AppConsts.authorization.encrptedAuthTokenName,
            {
                token: encryptedToken,
                expireDate: new Date(new Date().getTime() + 365 * 86400000), //1 year
            },
            callback
        );
    }

    private static loadAssetsForInstallPage(callback) {
        abp.setting.values['App.UiManagement.Theme'] = 'default';
        abp.setting.values['default.App.UiManagement.ThemeColor'] = 'default';

        DynamicResourcesHelper.loadResources(callback);
    }


}
