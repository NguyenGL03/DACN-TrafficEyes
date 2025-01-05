import { HttpClient } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DynamicEntityPropertyManagerComponent } from '@app/shared/common/dynamic-entity-property-manager/dynamic-entity-property-manager.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ReportTypeConsts } from '@app/shared/core/utils/consts/ReportTypeConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    AsposeServiceProxy,
    EntityDtoOfInt64,
    GetUsersInput,
    ReportInfo,
    UserListDto,
    UserServiceProxy
} from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import { LazyLoadEvent } from 'primeng/api';
import { FileUpload } from 'primeng/fileupload';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';
import { PermissionTreeModalComponent } from '../../shared/permission-tree-modal.component';
import { CreateOrEditUserModalComponent } from './create-or-edit-user-modal.component';
import { ImpersonationService } from './impersonation.service';

@Component({
    templateUrl: './users.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class UsersComponent extends DefaultComponentBase implements AfterViewInit {
    @ViewChild('createOrEditUserModal', { static: true }) createOrEditUserModal: CreateOrEditUserModalComponent;
    // @ViewChild('editUserPermissionsModal', { static: true }) editUserPermissionsModal: EditUserPermissionsModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('permissionFilterTreeModal', { static: true }) permissionFilterTreeModal: PermissionTreeModalComponent;
    @ViewChild('dynamicEntityPropertyManager', { static: true })
    dynamicEntityPropertyManager: DynamicEntityPropertyManagerComponent;

    uploadUrl: string;

    //Filters
    advancedFiltersAreShown = false;
    filterText = '';
    role = '';
    onlyLockedUsers = false;
    constructor(
        injector: Injector,
        private cdf: ChangeDetectorRef,
        public _impersonationService: ImpersonationService,
        private _userServiceProxy: UserServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _httpClient: HttpClient,
        private _localStorageService: LocalStorageService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Users/ImportFromExcel';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
        this.cdf.detectChanges();
    }

    getUsers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._userServiceProxy
            .getUsers(
                new GetUsersInput({
                    filter: this.filterText,
                    // permissions: this.permissionFilterTreeModal.getSelectedPermissions(),
                    permissions: [],
                    role: this.role !== '' ? parseInt(this.role) : undefined,
                    onlyLockedUsers: this.onlyLockedUsers,
                    sorting: this.primengTableHelper.getSorting(this.dataTable),
                    maxResultCount: this.primengTableHelper.getMaxResultCount(this.paginator, event),
                    skipCount: this.primengTableHelper.getSkipCount(this.paginator, event),
                    top: undefined,
                    autH_STATUS: undefined,
                    deP_ID: undefined,
                    independentUnit: false,
                    permission: undefined,
                    userName: undefined,
                    subbrId: undefined
                })
            )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.setUsersProfilePictureUrl(this.primengTableHelper.records);
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    unlockUser(record): void {
        this._userServiceProxy.unlockUser(new EntityDtoOfInt64({ id: record.id })).subscribe(() => {
            this.notify.success(this.l('UnlockedTheUser', record.userName));
            this.reloadPage();
        });
    }

    getRolesAsString(roles): string {
        let roleNames = '';

        for (let j = 0; j < roles.length; j++) {
            if (roleNames.length) {
                roleNames = roleNames + ', ';
            }

            roleNames = roleNames + roles[j].roleName;
        }

        return roleNames;
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    exportToExcel($event): void {
        let reportInfo = new ReportInfo();
        reportInfo.typeExport = ReportTypeConsts.Excel;
        var filterInput = {};
        filterInput['LEVEL'] = 'ALL'; //search null

        let filterReport = { ...filterInput, ...this.getFillterForCombobox() };
        filterReport.totalCount = this.isNull(filterReport.totalCount) ? 0 : filterReport.totalCount;
        reportInfo.parameters = this.GetParamsFromFilter(filterReport);

        reportInfo.pathName = '/COMMON/BC_USER.xlsx';
        reportInfo.storeName = 'rpt_TL_USER';
        reportInfo.values = this.GetParamsFromFilter({
            A1: this.l('CompanyReportHeader')
        });
        this.asposeService.getReport(reportInfo).subscribe((res) => {
            this._fileDownloadService.downloadTempFile(res);
        });
    }


    createUser(): void {
        this.createOrEditUserModal.show();
    }

    uploadExcel(data: { files: File }): void {
        const formData: FormData = new FormData();
        const file = data.files[0];
        formData.append('file', file, file.name);

        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => this.excelFileUpload.clear()))
            .subscribe((response) => {
                if (response.success) {
                    this.notify.success(this.l('ImportUsersProcessStart'));
                } else if (response.error != null) {
                    this.notify.error(this.l('ImportUsersUploadFailed'));
                }
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l('ImportUsersUploadFailed'));
    }

    deleteUser(user: UserListDto): void {
        if (user.userName === AppConsts.userManagement.defaultAdminUserName) {
            this.message.warn(this.l('{0}UserCannotBeDeleted', AppConsts.userManagement.defaultAdminUserName));
            return;
        }

        this.message.confirm(this.l('UserDeleteWarningMessage', user.userName), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._userServiceProxy.deleteUser(user.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    showDynamicProperties(user: UserListDto): void {
        this.dynamicEntityPropertyManager
            .getModal()
            .show('gAMSPro.Authorization.Users.User', user.id.toString());
    }

    setUsersProfilePictureUrl(users: UserListDto[]): void {
        for (let i = 0; i < users.length; i++) {
            let user = users[i];
            this._localStorageService.getItem(AppConsts.authorization.encrptedAuthTokenName, function (err, value) {
                let profilePictureUrl =
                    AppConsts.remoteServiceBaseUrl +
                    '/Profile/GetProfilePictureByUser?userId=' +
                    user.id +
                    '&' +
                    AppConsts.authorization.encrptedAuthTokenName +
                    '=' +
                    encodeURIComponent(value.token);
                (user as any).profilePictureUrl = profilePictureUrl;
            });
        }
    }

    isUserLocked(user: UserListDto): boolean {
        // if (!user.lockoutEndDateUtc) {
        //     return false;
        // }

        // let lockoutEndDateUtc = this._dateTimeService.changeDateTimeZone(user.lockoutEndDateUtc, 'UTC');
        // return lockoutEndDateUtc > this._dateTimeService.getUTCDate();
        return false
    }
}
