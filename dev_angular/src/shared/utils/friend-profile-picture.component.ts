import { AfterViewInit, Component, Input } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from './local-storage.service';

@Component({
    selector: 'friend-profile-picture',
    template: `
        <img [src]="profilePicture" alt="..." />
    `,
})
export class FriendProfilePictureComponent implements AfterViewInit {
    @Input() userId: number;
    @Input() tenantId: number;

    profilePicture = AppConsts.appBaseUrl + '/assets/icons/theme1/avatar-male.svg';

    constructor(
        private _profileService: ProfileServiceProxy,
        private localStorageService: LocalStorageService,
    ) {
        this.localStorageService.getItem('theme', (err, value) => {
            if(value) this.profilePicture = AppConsts.appBaseUrl +`/assets/icons/${value}/avatar-male.svg`
        })
     }

    ngAfterViewInit(): void {
        this.setProfileImage();
    }

    private setProfileImage(): void {
        if (!this.tenantId) {
            this.tenantId = undefined;
        }

        this._profileService.getFriendProfilePicture(this.userId, this.tenantId).subscribe((result) => {
            if (result && result.profilePicture) {
                this.profilePicture = 'data:image/jpeg;base64,' + result.profilePicture;
            }
        });
    }
}
