import { Component, EventEmitter, Injector, Input, Output } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { AppNavigationService } from '@app/shared/layout/nav/app-navigation.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LocalStorageService } from '@shared/utils/local-storage.service';

export class BreadcrumbItem {
    text: string;
    routerLink?: string;
    navigationExtras?: NavigationExtras;

    constructor(text: string, routerLink?: string, navigationExtras?: NavigationExtras) {
        this.text = text;
        this.routerLink = routerLink;
        this.navigationExtras = navigationExtras;
    }
}

@Component({
    selector: 'breadcrumbs',
    templateUrl: './breadcrumbs.component.html'
})
export class BreadcrumbsComponent extends AppComponentBase {
    @Input() breadcrumbs: BreadcrumbItem[];
    @Input() showBtnExcel: boolean = false;
    @Input() showBtnSwitch: boolean = false;
    @Input() showBtnSetting: boolean = false;

    @Output() excel: any = new EventEmitter<any>();
    @Output() setting: any = new EventEmitter<any>();
    @Output() switch: any = new EventEmitter<any>();

    path: string = '../../../../../assets/icons/theme1/home.svg'

    constructor(private _router: Router, injector: Injector,
        private localStorageService: LocalStorageService,

    ) {
        super(injector);
        this.localStorageService.getItem('theme', (err, value) => {
            if (value) {
                this.path = `../../../../../assets/icons/${value}/home.svg`
            }
        })
    }

    goToBreadcrumb(breadcrumb: BreadcrumbItem): void {
        if (!breadcrumb.routerLink) {
            return;
        }

        if (breadcrumb.navigationExtras) {
            this._router.navigate([breadcrumb.routerLink], breadcrumb.navigationExtras);
        } else {
            this._router.navigate([breadcrumb.routerLink]);
        }
    }
    goToHome(): void {
        this._router.navigate(['app/admin/dashboard']);
    }

    exportExcel() {
        this.excel.emit();
    }

    breadcrumbSwitch() {
        this.switch.emit();
    }

    breadcrumbSetting() {
        this.setting.emit();
    }
}
