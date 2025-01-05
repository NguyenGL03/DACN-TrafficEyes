import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { LinkedAccountsModalComponent } from '@app/shared/layout/linked-accounts-modal.component';
import { UserNotificationHelper } from '@app/shared/layout/notifications/UserNotificationHelper';
import { NotificationSettingsModalComponent } from '@app/shared/layout/notifications/notification-settings-modal.component';
import { ChangePasswordModalComponent } from '@app/shared/layout/profile/change-password-modal.component';
import { ChangeProfilePictureModalComponent } from '@app/shared/layout/profile/change-profile-picture-modal.component';
import { MySettingsModalComponent } from '@app/shared/layout/profile/my-settings-modal.component';
import { UserDelegationsModalComponent } from '@app/shared/layout/user-delegations-modal.component';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { SubscriptionStartType } from '@shared/service-proxies/service-proxies';
import { ChatSignalrService } from 'app/shared/layout/chat/chat-signalr.service';
import { AppComponentBase } from 'shared/common/app-component-base';
import { SignalRHelper } from 'shared/helpers/SignalRHelper';
import { DateTimeService } from './shared/common/timing/date-time.service';

import {
    DrawerComponent,
    MenuComponent,
    ScrollComponent,
    ScrollTopComponent,
    StickyComponent,
    ToggleComponent,
} from '@metronic/app/kt/components';
import moment from 'moment';

@Component({
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.less'],
})
export class AppComponent extends AppComponentBase implements OnInit {
    @ViewChild('linkedAccountsModal') linkedAccountsModal: LinkedAccountsModalComponent;
    @ViewChild('userDelegationsModal', { static: true }) userDelegationsModal: UserDelegationsModalComponent;
    @ViewChild('changePasswordModal', { static: true }) changePasswordModal: ChangePasswordModalComponent;
    @ViewChild('changeProfilePictureModal', { static: true }) changeProfilePictureModal: ChangeProfilePictureModalComponent;
    @ViewChild('mySettingsModal', { static: true }) mySettingsModal: MySettingsModalComponent;
    @ViewChild('notificationSettingsModal', { static: true }) notificationSettingsModal: NotificationSettingsModalComponent;
    @ViewChild('chatBarComponent') chatBarComponent;

    subscriptionStartType = SubscriptionStartType;
    theme: string;
    installationMode = true;
    isQuickThemeSelectEnabled: boolean = this.setting.getBoolean('App.UserManagement.IsQuickThemeSelectEnabled');
    IsSessionTimeOutEnabled: boolean =
        this.setting.getBoolean('App.UserManagement.SessionTimeOut.IsEnabled') && this.appSession.userId != null;

    public constructor(
        injector: Injector,
        private _chatSignalrService: ChatSignalrService,
        private _userNotificationHelper: UserNotificationHelper,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._userNotificationHelper.settingsModal = this.notificationSettingsModal;
        this.theme = abp.setting.get('App.UiManagement.Theme').toLocaleLowerCase();
        this.installationMode = UrlHelper.isInstallUrl(location.href);

        this.registerModalOpenEvents();

        const scope = this;

        Date.prototype['toISOString_old'] = Date.prototype.toISOString;

        Date.prototype.toISOString = function () {
            return moment(this).format(scope.s('gAMSProCore.DateTimeFormatClient'));
        }

        String.prototype['toMoment'] = function () {
            return scope.parseStringToMoment(this);
        }

        String.prototype['clone'] = function () {
            return scope.parseStringToMoment(this).clone();
        }

        String.prototype['format'] = function (opt) {
            return scope.parseStringToMoment(this).format(opt);
        }

        moment.prototype['toISOString_old'] = moment.prototype.toISOString;

        moment.prototype.toISOString = function () {
            return moment(this).format(scope.s('gAMSProCore.DateTimeFormatClient'));
        }

        Array.prototype.firstOrDefault = function (callbackfn: (value: any, index: number, array: any[]) => boolean, option1?: any) {
            let result = undefined;
            if (!callbackfn) {
                result = this;
            }
            else {
                result = this.filter(callbackfn);
            }
            if (result.length == 0) {
                return option1;
            }
            return result[0];
        };

        Array.prototype.sum = function (callbackfn?: (value: any, index: number, array: any[]) => number) {
            let result = undefined;
            let sum = 0;
            if (!callbackfn) {
                this.forEach(item => {
                    sum += item;
                });
            }
            else {
                let index = 0;
                this.forEach(item => {
                    let value = callbackfn(item, index, this);
                    sum += value || 0;
                    index++;
                });
            }

            return sum;
        };

        
		Array.prototype.sumWDefault = function (callbackfn?: (value: any, index: number, array: any[]) => number, valDefault?: any) {
			if (!this) {
				return undefined;
			}
			let result = undefined;
			let sum = 0;
			if (!callbackfn) {
				this.forEach(item => {
					sum += item;
				});
			}
			else {
				let index = 0;
				this.forEach(item => {
					let value = callbackfn(item, index, this);
					if (value == null || value == undefined) {
						value = valDefault;
					}
					sum += value || 0;
					index++;
				});
			}

			return sum;
		};

		const unique = (value, index, self) => {
			return self.indexOf(value) === index
		}

		Array.prototype.distinct = function () {
			return this.filter(unique);
		}
        
        if (this.appSession.application) {
            SignalRHelper.initSignalR(() => {
                this._chatSignalrService.init();
            });
        }

        this.pluginsInitialization();
    }

    pluginsInitialized(): boolean {
        let menuItems = document.querySelectorAll('[data-kt-menu="true"]');
        for (let i = 0; i < menuItems.length; i++) {
            let el = menuItems[i];
            const menuItem = el as HTMLElement;
            let menuInstance = MenuComponent.getInstance(menuItem);
            if (menuInstance) {
                return true;
            }
        }

        return false;
    }

    pluginsInitialization() {

        abp.event.on('app.dynamic-styles-loaded', function () {
            KTUtil.resize();
        });

        setTimeout(() => {
            if (this.pluginsInitialized()) {
                return;
            }

            ToggleComponent.bootstrap();
            ScrollTopComponent.bootstrap();
            DrawerComponent.bootstrap();
            StickyComponent.bootstrap();
            MenuComponent.bootstrap();
            ScrollComponent.bootstrap();
        }, 200);
    }

    parseStringToMoment(str: string) {
        return moment(str);
    }

    subscriptionStatusBarVisible(): boolean {
        return (
            this.appSession.tenantId > 0 &&
            (this.appSession.tenant.isInTrialPeriod || this.subscriptionIsExpiringSoon())
        );
    }

    subscriptionIsExpiringSoon(): boolean {
        // if (this.appSession.tenant?.subscriptionEndDateUtc) {
        //     let today = this._dateTimeService.getUTCDate();
        //     let daysFromNow = this._dateTimeService.plusDays(today, AppConsts.subscriptionExpireNootifyDayCount);
        //     return daysFromNow >= this.appSession.tenant.subscriptionEndDateUtc;
        // }

        return false;
    }

    registerModalOpenEvents(): void {
        this.subscribeToEvent('app.show.linkedAccountsModal', () => {
            this.linkedAccountsModal.show();
        });

        this.subscribeToEvent('app.show.userDelegationsModal', () => {
            this.userDelegationsModal.show();
        });

        this.subscribeToEvent('app.show.changePasswordModal', () => {
            this.changePasswordModal.show();
        });

        this.subscribeToEvent('app.show.changeProfilePictureModal', () => {
            this.changeProfilePictureModal.show();
        });

        this.subscribeToEvent('app.show.mySettingsModal', () => {
            this.mySettingsModal.show();
        });
    }

    getRecentlyLinkedUsers(): void {
        abp.event.trigger('app.getRecentlyLinkedUsers');
    }

    onMySettingsModalSaved(): void {
        abp.event.trigger('app.onMySettingsModalSaved');
    }
}
