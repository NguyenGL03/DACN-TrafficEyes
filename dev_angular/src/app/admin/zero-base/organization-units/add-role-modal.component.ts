import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    FindOrganizationUnitRolesInput,
    NameValueDto,
    OrganizationUnitServiceProxy,
    RolesToOrganizationUnitInput,
} from '@shared/service-proxies/service-proxies';
import { map as _map } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { IRolesWithOrganizationUnit } from './roles-with-organization-unit';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'addRoleModal',
    templateUrl: './add-role-modal.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddRoleModalComponent extends AppComponentBase {
    @Output() rolesAdded: EventEmitter<IRolesWithOrganizationUnit> = new EventEmitter<IRolesWithOrganizationUnit>();
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    organizationUnitId: number;

    isShown = false;
    filterText = '';
    tenantId?: number;
    saving = false;
    selectedRoles: NameValueDto[];

    constructor(injector: Injector, private _organizationUnitService: OrganizationUnitServiceProxy) {
        super(injector);
    }

    show(): void {
        this.modal.show();
    }

    refreshTable(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    close(): void {
        this.modal.hide();
    }

    shown(): void {
        this.isShown = true;
        this.getRecordsIfNeeds(null);
    }

    getRecordsIfNeeds(event: LazyLoadEvent): void {
        if (!this.isShown) {
            return;
        }

        this.getRecords(event);
    }

    getRecords(event?: LazyLoadEvent): void {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        const input = new FindOrganizationUnitRolesInput();
        input.organizationUnitId = this.organizationUnitId;
        input.filter = this.filterText;
        input.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
        input.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

        this._organizationUnitService
            .findRoles(input)
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    addRolesToOrganizationUnit(): void {
        const input = new RolesToOrganizationUnitInput();
        input.organizationUnitId = this.organizationUnitId;
        input.roleIds = _map(this.selectedRoles, (selectedRole) => Number(selectedRole.value));
        this.showLoading();
        this._organizationUnitService.addRolesToOrganizationUnit(input).subscribe(() => {
            this.notify.success(this.l('SuccessfullyAdded'));
            this.rolesAdded.emit({
                roleIds: input.roleIds,
                ouId: input.organizationUnitId,
            });
            this.hideLoading();
            this.close();
            this.selectedRoles = [];
        });
    }
}
