import { CommonModule } from '@angular/common';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatSignalrService } from '@app/shared/layout/chat/chat-signalr.service';
import { CreateNewUserDelegationModalComponent } from '@app/shared/layout/create-new-user-delegation-modal.component';
import { LinkAccountModalComponent } from '@app/shared/layout/link-account-modal.component';
import { LinkedAccountsModalComponent } from '@app/shared/layout/linked-accounts-modal.component';
import { ChangePasswordModalComponent } from '@app/shared/layout/profile/change-password-modal.component';
import { MySettingsModalComponent } from '@app/shared/layout/profile/my-settings-modal.component';
import { SmsVerificationModalComponent } from '@app/shared/layout/profile/sms-verification-modal.component';
import { UserDelegationsModalComponent } from '@app/shared/layout/user-delegations-modal.component';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ImageCropperModule } from 'ngx-image-cropper';
import { FileUploadModule as PrimeNgFileUploadModule } from 'primeng/fileupload';
import { PaginatorModule } from 'primeng/paginator';
import { ProgressBarModule } from 'primeng/progressbar';
import { TableModule } from 'primeng/table';
import { ImpersonationService } from './admin/zero-base/users/impersonation.service';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppCommonModule } from './shared/common/app-common.module';
import { ChatBarComponent } from './shared/layout/chat/chat-bar.component';
import { ChatFriendListItemComponent } from './shared/layout/chat/chat-friend-list-item.component';
import { ChatMessageComponent } from './shared/layout/chat/chat-message.component';
import { FooterComponent } from './shared/layout/footer.component';
import { LinkedAccountService } from './shared/layout/linked-account.service';
import { SideBarMenuComponent } from './shared/layout/nav/side-bar-menu.component';
import { TopBarMenuComponent } from './shared/layout/nav/top-bar-menu.component';
import { UserNotificationHelper } from './shared/layout/notifications/UserNotificationHelper';
import { HeaderNotificationsComponent } from './shared/layout/notifications/header-notifications.component';
import { NotificationSettingsModalComponent } from './shared/layout/notifications/notification-settings-modal.component';
import { NotificationsComponent } from './shared/layout/notifications/notifications.component';
import { ThemeSelectionPanelComponent } from './shared/layout/theme-selection/theme-selection-panel.component';
import { DefaultBrandComponent } from './shared/layout/themes/default/default-brand.component';
import { DefaultLayoutComponent } from './shared/layout/themes/default/default-layout.component';
import { Theme10BrandComponent } from './shared/layout/themes/theme10/theme10-brand.component';
import { Theme10LayoutComponent } from './shared/layout/themes/theme10/theme10-layout.component';
import { Theme11BrandComponent } from './shared/layout/themes/theme11/theme11-brand.component';
import { Theme11LayoutComponent } from './shared/layout/themes/theme11/theme11-layout.component';
import { Theme12BrandComponent } from './shared/layout/themes/theme12/theme12-brand.component';
import { Theme12LayoutComponent } from './shared/layout/themes/theme12/theme12-layout.component';
import { Theme13BrandComponent } from './shared/layout/themes/theme13/theme13-brand.component';
import { Theme13LayoutComponent } from './shared/layout/themes/theme13/theme13-layout.component';
import { Theme2BrandComponent } from './shared/layout/themes/theme2/theme2-brand.component';
import { Theme2LayoutComponent } from './shared/layout/themes/theme2/theme2-layout.component';
import { Theme3BrandComponent } from './shared/layout/themes/theme3/theme3-brand.component';
import { Theme3LayoutComponent } from './shared/layout/themes/theme3/theme3-layout.component';
import { Theme4BrandComponent } from './shared/layout/themes/theme4/theme4-brand.component';
import { Theme4LayoutComponent } from './shared/layout/themes/theme4/theme4-layout.component';
import { Theme5BrandComponent } from './shared/layout/themes/theme5/theme5-brand.component';
import { Theme5LayoutComponent } from './shared/layout/themes/theme5/theme5-layout.component';
import { Theme6BrandComponent } from './shared/layout/themes/theme6/theme6-brand.component';
import { Theme6LayoutComponent } from './shared/layout/themes/theme6/theme6-layout.component';
import { Theme7BrandComponent } from './shared/layout/themes/theme7/theme7-brand.component';
import { Theme7LayoutComponent } from './shared/layout/themes/theme7/theme7-layout.component';
import { Theme8BrandComponent } from './shared/layout/themes/theme8/theme8-brand.component';
import { Theme8LayoutComponent } from './shared/layout/themes/theme8/theme8-layout.component';
import { Theme9BrandComponent } from './shared/layout/themes/theme9/theme9-brand.component';
import { Theme9LayoutComponent } from './shared/layout/themes/theme9/theme9-layout.component';
import { ActiveDelegatedUsersComboComponent } from './shared/layout/topbar/active-delegated-users-combo.component';
import { ChatToggleButtonComponent } from './shared/layout/topbar/chat-toggle-button.component';
import { LanguageSwitchDropdownComponent } from './shared/layout/topbar/language-switch-dropdown.component';
import { QuickThemeSelectionComponent } from './shared/layout/topbar/quick-theme-selection.component';
import { SubscriptionNotificationBarComponent } from './shared/layout/topbar/subscription-notification-bar.component';
import { UserMenuComponent } from './shared/layout/topbar/user-menu.component';

import { IMaskModule } from 'angular-imask';
import { DefaultLogoComponent } from './shared/layout/themes/default/default-logo.component';
// Metronic
import { PerfectScrollbarModule } from '@craftsjs/perfect-scrollbar';
import { AppBsModalModule } from '@shared/common/appBsModal/app-bs-modal.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { SessionTimeoutModalComponent } from './shared/common/session-timeout/session-timeout-modal-component';
import { SessionTimeoutComponent } from './shared/common/session-timeout/session-timeout.component';
import { AddFriendModalComponent } from './shared/layout/chat/add-friend-modal.component';
import { AddFromDifferentTenantModalComponent } from './shared/layout/chat/add-from-different-tenant-modal.component';
import { FriendsLookupTableComponent } from './shared/layout/chat/friends-lookup-table.component';
import { MenuSearchBarComponent } from './shared/layout/nav/menu-search-bar/menu-search-bar.component';
import { ChangeProfilePictureModalModule } from './shared/layout/profile/change-profile-picture-modal.module';
import { EnableTwoFactorAuthenticationModalComponent } from './shared/layout/profile/enable-two-factor-authentication-modal.component';
import { RecoveryCodesComponent } from './shared/layout/profile/recovery-codes.component';
import { VerifyCodeModalComponent } from './shared/layout/profile/verify-code-modal.component';
import { ViewRecoveryCodesModalComponent } from './shared/layout/profile/view-recovery-codes-modal.component';
import { ScrollTopComponent } from './shared/layout/scroll-top.component';
import { ToggleDarkModeComponent } from './shared/layout/toggle-dark-mode/toggle-dark-mode.component';
import { SubheaderModule } from "./shared/common/sub-header/subheader.module";
import { ApproveGroupServiceProxy, AttachFileServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { CustomNotifyService } from '@shared/common/ui/custom-notify.service';
import { AppPreBootstrap } from 'AppPreBootstrap';
import { AccordionModule } from 'primeng/accordion';

@NgModule({
    declarations: [
        AppComponent,
        DefaultLayoutComponent,
        Theme2LayoutComponent,
        Theme3LayoutComponent,
        Theme4LayoutComponent,
        Theme5LayoutComponent,
        Theme6LayoutComponent,
        Theme7LayoutComponent,
        Theme8LayoutComponent,
        Theme9LayoutComponent,
        Theme10LayoutComponent,
        Theme11LayoutComponent,
        Theme12LayoutComponent,
        Theme13LayoutComponent,
        HeaderNotificationsComponent,
        SideBarMenuComponent,
        TopBarMenuComponent,
        FooterComponent,
        ScrollTopComponent,
        LinkedAccountsModalComponent,
        UserDelegationsModalComponent,
        CreateNewUserDelegationModalComponent,
        LinkAccountModalComponent,
        ChangePasswordModalComponent,
        MySettingsModalComponent,
        SmsVerificationModalComponent,
        EnableTwoFactorAuthenticationModalComponent,
        RecoveryCodesComponent,
        VerifyCodeModalComponent,
        ViewRecoveryCodesModalComponent,
        NotificationsComponent,
        ChatBarComponent,
        AddFriendModalComponent,
        AddFromDifferentTenantModalComponent,
        FriendsLookupTableComponent,
        ThemeSelectionPanelComponent,
        ChatFriendListItemComponent,
        NotificationSettingsModalComponent,
        ChatMessageComponent,
        QuickThemeSelectionComponent,
        LanguageSwitchDropdownComponent,
        ChatToggleButtonComponent,
        SubscriptionNotificationBarComponent,
        UserMenuComponent,
        DefaultBrandComponent,
        Theme2BrandComponent,
        Theme3BrandComponent,
        Theme4BrandComponent,
        Theme5BrandComponent,
        Theme6BrandComponent,
        Theme7BrandComponent,
        Theme8BrandComponent,
        Theme9BrandComponent,
        Theme10BrandComponent,
        Theme11BrandComponent,
        Theme12BrandComponent,
        Theme13BrandComponent,
        SessionTimeoutModalComponent,
        SessionTimeoutComponent,
        MenuSearchBarComponent,
        ActiveDelegatedUsersComboComponent,
        DefaultLogoComponent,
        ToggleDarkModeComponent,
    ],
    providers: [
        ImpersonationService,
        LinkedAccountService,
        UserNotificationHelper,
        ChatSignalrService,
        AttachFileServiceProxy,
        ApproveGroupServiceProxy,
        {
            provide: APP_INITIALIZER,
            useFactory: appInitializerFactory,
            multi: true,
        },
        { provide: NotifyService, useClass: CustomNotifyService }
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        ModalModule.forRoot(),
        TooltipModule.forRoot(),
        TabsModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot(),
        BsDatepickerModule.forRoot(),
        FileUploadModule,
        AppRoutingModule,
        UtilsModule,
        AppCommonModule.forRoot(),
        ServiceProxyModule,
        TableModule,
        PaginatorModule,
        PrimeNgFileUploadModule,
        ProgressBarModule,
        PerfectScrollbarModule,
        IMaskModule,
        ImageCropperModule,
        AutoCompleteModule,
        NgxSpinnerModule,
        AppBsModalModule,
        ChangeProfilePictureModalModule,
        SubheaderModule,
        AccordionModule,
    ]
})
export class AppModule {
    constructor(private injector: Injector) {
        AppPreBootstrap.initializeServices(this.injector);
    }

    ngDoBootstrap() {
        AppPreBootstrap.setTheme();
    }
}

function appInitializerFactory() {
    return () => {

        const url = new URL(location.href);
        const params = url.searchParams;

        if (params.has('t')) {
            params.delete('t');
            window.location.href = url.toString();
        }
    };
}
