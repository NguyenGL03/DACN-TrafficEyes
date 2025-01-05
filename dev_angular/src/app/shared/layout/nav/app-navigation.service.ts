import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';
import { map, Observable, Subject, } from 'rxjs';
import { AppMenuServiceProxy, HostSettingsEditDto, HostSettingsServiceProxy } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/utils/local-storage.service';

@Injectable()
export class AppNavigationService {
    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService,
        private _appMenuService: AppMenuServiceProxy,
        private _hostSettingsService: HostSettingsServiceProxy,
        private localStorageService: LocalStorageService
    ) { }
    static appMenus: AppMenuItem[];

    getMenuList(): Observable<AppMenuItem[]> {
        let subject = new Subject<AppMenuItem[]>();
        this._appMenuService.getAllMenus().subscribe(response => {
            let appMenus: AppMenuItem[] = [];
            response = response.filter(x => this._permissionCheckerService.isGranted(x.permissionName))
            response.forEach(x => {
                let appMenu: AppMenuItem = new AppMenuItem(x.name, x.permissionName, x.icon, x.route);
                appMenu.id = x.menuId;
                appMenu.parentId = x.parentId;
                appMenu.items = [];
                appMenus.push(appMenu);
            });

            AppNavigationService.appMenus = appMenus;

            subject.next(appMenus);
        });
        return subject.asObservable();
    }

    buildTreeMenu(appMenus: AppMenuItem[]) {
        let dist = {};

        appMenus.forEach((x) => { dist[x.id] = x; });

        appMenus.forEach((x) => {
            let parent = dist[x.parentId];
            if (parent) {
                parent.items.push(x);
                x.parent = parent;
            }
        })
    }

    getMenu(): Observable<AppMenu> {
        let menu: AppMenu;
        const subject = new Subject<AppMenu>();

        this.getMenuList().subscribe(appMenus => {
            this.buildTreeMenu(appMenus);
            menu = new AppMenu('MainMenu', 'MainMenu', appMenus.filter(x => x.parent == null));
            subject.next(menu);
        });

        return subject.asObservable();
    }

    checkChildMenuItemPermission(menuItem): boolean {
        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                if (!subMenuItem.hasFeatureDependency()) {
                    return true;
                }

                if (subMenuItem.featureDependencySatisfied()) {
                    return true;
                }
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }

        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (
            menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' &&
            this._appSessionService.tenant &&
            !this._appSessionService.tenant.edition
        ) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): Observable<AppMenuItem[]> {
        var subject = new Subject<AppMenuItem[]>();
        this.getMenu().subscribe(menu => {
            let allMenuItems: AppMenuItem[] = [];
            menu.items.forEach((menuItem) => {
                allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
            });
            subject.next(allMenuItems);
        });
        return subject.asObservable();
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach((subMenu) => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }

    getAllSetting() {
        return this._hostSettingsService.getAllSettings().pipe(
            map((res: HostSettingsEditDto) => {
                this.localStorageService.setItem('theme', res.commonThemeSettings.theme)
                return res
            })
        )
    }
}
