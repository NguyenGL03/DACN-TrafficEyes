import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CM_EMPLOYEE_ENTITY, CreateOrUpdateUserInput, OrganizationUnitDto, PasswordComplexitySetting, ProfileServiceProxy, UserEditDto, UserRoleDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { IOrganizationUnitsTreeComponentData, OrganizationUnitsTreeComponent } from '../../shared/organization-unit-tree.component';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';
import { PasswordMeterComponent } from '@metronic/app/kt/components';
import { LocalStorageService } from '@shared/utils/local-storage.service';

@Component({
    selector: 'createOrEditUserModal',
    templateUrl: './create-or-edit-user-modal.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class CreateOrEditUserModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('organizationUnitTree') organizationUnitTree: OrganizationUnitsTreeComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    canChangeUserName = true;
    canChangeProfilePicture = false;
    isTwoFactorEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled');
    isLockoutEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.UserLockOut.IsEnabled');
    passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();

    user: UserEditDto = new UserEditDto();
    roles: UserRoleDto[];
    sendActivationEmail = true;
    setRandomPassword = true;
    passwordComplexityInfo = '';
    profilePicture: string;
    allowedUserNameCharacters = '';
    isSMTPSettingsProvided = false;
    passwordMeterInitialized = false;

    allOrganizationUnits: OrganizationUnitDto[];
    memberedOrganizationUnits: string[];
    userPasswordRepeat = '';

    constructor(
        injector: Injector,
        private _userService: UserServiceProxy,
        private _profileService: ProfileServiceProxy,
        private localStorageService: LocalStorageService,

    ) {
        super(injector);
    }

    show(userId?: number): void {
        if (!userId) {
            this.active = true;
            this.setRandomPassword = true;
            this.sendActivationEmail = true;
            this.canChangeProfilePicture = false;
        } else {
            this.canChangeProfilePicture = this.permission.isGranted('Pages.Administration.Users.ChangeProfilePicture');
        }

        this._userService.getUserForEdit(userId).subscribe((userResult) => {
            this.user = userResult.user;
            this.roles = userResult.roles;
            this.canChangeUserName = this.user.userName !== AppConsts.userManagement.defaultAdminUserName;
            this.allowedUserNameCharacters = userResult.allowedUserNameCharacters;
            this.isSMTPSettingsProvided = userResult.isSMTPSettingsProvided;
            this.sendActivationEmail = userResult.isSMTPSettingsProvided;

            this.allOrganizationUnits = userResult.allOrganizationUnits;
            this.memberedOrganizationUnits = userResult.memberedOrganizationUnits;

            this.getProfilePicture(userId);

            if (userId) {
                this.active = true;

                setTimeout(() => {
                    this.setRandomPassword = false;
                }, 0);

                this.sendActivationEmail = false;
            }

            this._profileService.getPasswordComplexitySetting().subscribe((passwordComplexityResult) => {
                this.passwordComplexitySetting = passwordComplexityResult.setting;
                this.setPasswordComplexityInfo();
                this.modal.show();
            });
        });
    }

    setPasswordComplexityInfo(): void {
        this.passwordComplexityInfo = '<ul>';

        if (this.passwordComplexitySetting.requireDigit) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireDigit_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requireLowercase) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireLowercase_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requireUppercase) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireUppercase_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requireNonAlphanumeric) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireNonAlphanumeric_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requiredLength) {
            this.passwordComplexityInfo +=
                '<li>' +
                this.l('PasswordComplexity_RequiredLength_Hint', this.passwordComplexitySetting.requiredLength) +
                '</li>';
        }

        this.passwordComplexityInfo += '</ul>';
    }

    getProfilePicture(userId: number): void {
        let path = this.localStorageService.getItem('theme', (err, value) => value)

        if (!userId) {
            this.profilePicture = this.appRootUrl() + `assets/icons/${path}/avatar-male.svg`;
            return;
        }

        this._profileService.getProfilePictureByUser(userId).subscribe((result) => {
            if (result && result.profilePicture) {
                this.profilePicture = 'data:image/jpeg;base64,' + result.profilePicture;
            } else {
                this.profilePicture = this.appRootUrl() + `assets/icons/${path}/avatar-male.svg`;
            }
        });
    }

    onShown(): void {
        this.organizationUnitTree.data = <IOrganizationUnitsTreeComponentData>{
            allOrganizationUnits: this.allOrganizationUnits,
            selectedOrganizationUnits: this.memberedOrganizationUnits,
        };

        document.getElementById('Name')?.focus();
    }
    @ViewChild('userForm') editForm: ElementRef;

    save(): void {
        let input = new CreateOrUpdateUserInput();

        input.user = this.user;
        input.setRandomPassword = this.setRandomPassword;
        input.sendActivationEmail = this.sendActivationEmail;
        input.assignedRoleNames = _map(
            _filter(this.roles, { isAssigned: true, inheritedFromOrganizationUnit: false }),
            (role) => role.roleName,
        );

        input.organizationUnits = this.organizationUnitTree.getSelectedOrganizationIds();

        this.showLoading();
        this._userService
            .createOrUpdateUser(input)
            .pipe(
                finalize(() => {
                    this.hideLoading();
                }),
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.userPasswordRepeat = '';
        this.modal.hide();
    }

    getAssignedRoleCount(): number {
        return _filter(this.roles, { isAssigned: true }).length;
    }

    getAssignedMemberedOrganizationUnitCount(): number {
        return this.memberedOrganizationUnits?.length;
    }

    onOrganizationUnitTreeSelectionChanged(): void {
        let organizationUnits = this.organizationUnitTree.getSelectedOrganizations();
        this.memberedOrganizationUnits = _map(organizationUnits, (ou) => ou.code);
    }

    setRandomPasswordChange(event): void {
        if (this.passwordMeterInitialized) {
            return;
        }

        if (!this.setRandomPassword) {
            return;
        }

        setTimeout(() => {
            PasswordMeterComponent.bootstrap();
            this.passwordMeterInitialized = true;
        }, 0);
    }

    getSingleEmployee(input: CM_EMPLOYEE_ENTITY) {
        if (!input || !input.emP_ID)
            return;

        this.user.emP_ID = input.emP_ID;
        this.user.emP_NAME = input.emP_NAME;
        this.user.poS_CODE = input.poS_CODE;
        this.user.poS_NAME = input.poS_NAME;
        this.user.subbrId = input.brancH_ID;
        this.user.deP_ID = input.deP_ID;
        this.user.name = input.emP_NAME
        this.user.emailAddress = input.email;
        this.user.phoneNumber = input.phone;
    }
}
