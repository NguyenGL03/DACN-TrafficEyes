import {
    AfterViewInit,
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Injector,
    Input,
    OnInit,
    ViewEncapsulation
} from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { DrawerComponent, MenuComponent, ScrollComponent, ToggleComponent } from '@metronic/app/kt/components';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FormattedStringValueExtracter } from '@shared/helpers/FormattedStringValueExtracter';
import { PermissionCheckerService } from 'abp-ng2-module';
import * as objectPath from 'object-path';
import { filter } from 'rxjs/operators';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';
import { AppNavigationService } from './app-navigation.service';

@Component({
    templateUrl: './side-bar-menu.component.html',
    selector: 'side-bar-menu',
    encapsulation: ViewEncapsulation.None,
})
export class SideBarMenuComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @Input() iconMenu = false;
    @Input() menuClass = 'menu menu-column menu-rounded menu-sub-indention px-3';

    menu: AppMenu = null;
    allMenuItems: AppMenuItem[];
    currentRouteUrl = '';
    insideTm: any;
    outsideTm: any;

    theme: string
    imgPath = `../../../../assets/icons/${'theme2'}/`

    constructor(
        injector: Injector,
        private router: Router,
        private changeDetectorRef: ChangeDetectorRef,
        public permission: PermissionCheckerService,
        private appNavigationService: AppNavigationService,
    ) {
        super(injector);
    }
 
    ngOnInit() {
        this.appNavigationService.getMenuList().subscribe(appMenus => {
            this.allMenuItems = appMenus;
            this.appNavigationService.buildTreeMenu(this.allMenuItems);
            this.menu = new AppMenu('MainMenu', 'MainMenu', this.allMenuItems.filter(x => x.parent == null));
            this.changeDetectorRef.detectChanges();
        });

        this.currentRouteUrl = this.router.url.split(/[?#]/)[0];
        this.router.events
            .pipe(filter((event) => event instanceof NavigationEnd))
            .subscribe((event) => (this.currentRouteUrl = this.router.url.split(/[?#]/)[0]));
        // this.router.events
        //     .pipe(filter((event) => event instanceof NavigationEnd || event instanceof NavigationCancel))
        //     .subscribe((event) => {
        //         this.reinitializeMenu();
        //     });

        const darkMode = abp.setting.get(abp.setting.get('App.UiManagement.Theme') + '.App.UiManagement.DarkMode');
        const isDarkMode = JSON.parse(darkMode.toLowerCase());

        this.appNavigationService.getAllSetting().subscribe(res => {
            this.imgPath = `../../../../assets/icons/${res.commonThemeSettings.theme || 'theme1'}/${isDarkMode ? 'dark' : 'light'}/`
        })

    }

    ngAfterViewInit(): void {
        this.scrollToCurrentMenuElement();
    }

    reinitializeMenu(): void {
        this.appNavigationService.getMenu().subscribe(menu => {
            this.menu = menu
            this.currentRouteUrl = this.router.url.split(/[?#]/)[0];
            setTimeout(() => {
                MenuComponent.reinitialization();
                DrawerComponent.reinitialization();
                ToggleComponent.reinitialization();
                ScrollComponent.reinitialization();
            }, 50);
        });
    }

    showMenuItem(menuItem): boolean {
        // return this.appNavigationService.showMenuItem(menuItem);
        const isVisible = this.appNavigationService.showMenuItem(menuItem);
        if (
            menuItem.name === 'Quản trị hệ thống' 
            || menuItem.name === 'Quản lý danh mục'
            || menuItem.name === 'Quản lý TSCĐ/CCLĐ'
            // || menuItem.name === 'Quản lý hàng hóa'
            || menuItem.name === 'Quản lý nhà cung cấp'
            || menuItem.name === 'Quản lý kế hoạch mua sắm'
            || menuItem.name === 'Quản lý mua sắm'
            || menuItem.name === 'Quản lý bất động sản'
            || menuItem.name === 'Quản lý kho vật liệu'
            || menuItem.name === 'Quản lý KT-CT Tòa nhà'
            || menuItem.name === 'Quản lý phương tiện vận tải'
            || menuItem.name === 'Quản lý công trình xây dựng cơ bản'
            || menuItem.permissionName === 'Pages.Main.ManagePayment'
            || menuItem.name === 'Danh mục loại hàng hóa'
            || menuItem.name === 'Danh mục nhóm hàng hóa'
            || menuItem.name === 'Danh mục hàng hóa'
        ) {
            return false;
        }
    
        return isVisible;
    }

    getTriggerCssClass(item, parentItem): string {
        if (!item.items && item.items.length <= 0) {
            return 'click';
        }

        if (parentItem) return 'hover';

        return 'click'
    }

    isMenuItemIsActive(item: any): boolean {
        if (item.items.length) {
            return this.isMenuRootItemIsActive(item);
        }

        if (!item.route) {
            return false;
        }
        let urlTree = this.router.parseUrl(this.currentRouteUrl.replace(/\/$/, ''));
        let urlString = '/' + urlTree.root.children.primary.segments.map((segment) => segment.path).join('/');
        if (item.route !== '/' && (
            urlString == item.route
            || urlString == item.route + '-add'
            || urlString == item.route + '-edit'
            || urlString == item.route + '-view'
        )) {
            return true;
        }

        let exactMatch = urlString === item.route.replace(/\/$/, '');
        if (!exactMatch && item.routeTemplates) {
            for (let i = 0; i < item.routeTemplates.length; i++) {
                let result = new FormattedStringValueExtracter().Extract(urlString, item.routeTemplates[i]);
                if (result.IsMatch) {
                    return true;
                }
            }
        }
        return exactMatch;
    }

    isMenuRootItemIsActive(item): boolean {
        let result = false;

        for (const subItem of item.items) {
            result = this.isMenuItemIsActive(subItem);
            if (result) {
                return true;
            }
        }

        return false;
    }

    scrollToCurrentMenuElement(): void {
        const path = location.pathname;
        const menuItem = document.querySelector('a[href=\'' + path + '\']');
        if (menuItem) {
            menuItem.scrollIntoView({ block: 'center' });
        }
    }

    getItemCssClasses(item: AppMenuItem, parentItem: AppMenuItem) {
        let classes = 'menu-item';

        if (item.items.length) {
            if (!this.iconMenu) {
                classes += ' menu-accordion';
            } else {
                if (parentItem == null) {
                    classes += ' menu-dropdown';
                } else {
                    classes += ' menu-accordion';
                }
            }
        }

        // custom class for menu item
        const customClass = objectPath.get(item, 'custom-class');
        if (customClass) {
            classes += ' ' + customClass;
        }

        if (this.iconMenu && parentItem == null) {
            classes += ' pb-3';
        }

        if (!this.iconMenu && this.isMenuItemIsActive(item)) {
            classes += ' show';
        }

        return classes;
    }

    getSubMenuItemCssClass(item: AppMenuItem, parentItem: AppMenuItem): string {
        let classes = 'menu-sub';

        if (!this.iconMenu) {
            classes += ' menu-sub-accordion';
        } else {
            if (parentItem == null) {
                classes += ' menu-sub-dropdown px-1 py-4';
            } else {
                classes += ' menu-sub-accordion';
            }
        }

        return classes;
    }

    checkIsIconClass(value: string) {
        return value.includes('fa')
    }
}
