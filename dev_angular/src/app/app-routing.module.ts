import { NgModule } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationSkipped, NavigationStart, RouteConfigLoadStart, Router, RouterModule } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppComponent } from './app.component';
import { AppRouteGuard } from './shared/common/auth/auth-route-guard';
import { NotificationsComponent } from './shared/layout/notifications/notifications.component';
import { HangHoaEditComponent } from './main/common/hanghoa/hanghoa-edit.component';
import { UploadImagesComponent } from './admin/zero-base/uploadimages/uploadimages.component';
import { CommonModule } from '@angular/common';
@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild([
            {
                path: 'app',
                component: AppComponent,
                canActivate: [AppRouteGuard],
                canActivateChild: [AppRouteGuard],
                children: [
                    {
                        path: '',
                        children: [
                            { path: 'notifications', component: NotificationsComponent },
                            { path: '', redirectTo: '/app/main/dashboard', pathMatch: 'full' },
                        ],
                    },
                    {
                        path: 'admin/uploadimages',component:UploadImagesComponent,
                    },
                    {
                        path: 'main',
                        loadChildren: () => import('app/main/main.module').then((m) => m.MainModule), //Lazy load main module
                        data: { preload: true },
                        canLoad: [AppRouteGuard],
                    },
                    {
                        path: 'admin',
                        loadChildren: () => import('app/admin/admin.module').then((m) => m.AdminModule), //Lazy load admin module
                        data: { preload: true },
                        canLoad: [AppRouteGuard],
                    },
                    {
                        path: '**',
                        redirectTo: 'dashboard',
                    },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class AppRoutingModule {
    constructor(private router: Router, private spinnerService: NgxSpinnerService) {
        router.events.subscribe((event) => {  
            if (event instanceof RouteConfigLoadStart || event instanceof NavigationStart) {
                spinnerService.show();
            }

            if (event instanceof NavigationCancel || event instanceof NavigationSkipped || event instanceof NavigationError) {
                spinnerService.hide();
            }
            
            if (event instanceof NavigationEnd) {
                document.querySelector('meta[property=og\\:url').setAttribute('content', window.location.href);
                spinnerService.hide();
            }
        });
    }
}
