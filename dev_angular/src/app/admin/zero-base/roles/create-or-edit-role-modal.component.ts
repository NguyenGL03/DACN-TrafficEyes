import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Injector, NgZone, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrUpdateRoleInput, GetRoleForEditOutput, RoleEditDto, RoleServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { GridPermissionComponent } from './grid-permission.component';

@Component({
    selector: 'createOrEditRoleModal',
    templateUrl: './create-or-edit-role-modal.component.html',
})
export class CreateOrEditRoleModalComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('gridPermission') gridPermission: GridPermissionComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    saving = false;
    visible = false;
    role: RoleEditDto = new RoleEditDto();
    result: GetRoleForEditOutput;

    get title(): string {
        return this.role.id ? this.l('EditRole') + ': ' + this.role.displayName : this.l('CreateNewRole');
    }

    constructor(
        injector: Injector,
        private _roleService: RoleServiceProxy,
        private zone: NgZone,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        this.zone.runOutsideAngular(() => {
            this._roleService.getAllRole().subscribe(result => {
                this.result = result;
                this.gridPermission.initPermission(result);
                this.cdr.detectChanges();
            });
            this.cdr.detectChanges();
        })
    }

    show(roleId?: number): void {
        this.zone.runOutsideAngular(() => {
            const self = this;
            self.showLoading();
            self._roleService.getRoleForEdit(roleId).subscribe((result) => {
                self.role = result.role;
                self.visible = true;
                self.gridPermission.initRolePermission(result.grantedPermissionNames);
                self.hideLoading();
            });
        })
    }

    save(): void {
        const input = new CreateOrUpdateRoleInput();
        input.role = this.role;
        input.grantedPermissionNames = this.gridPermission.getGrantedPermissionNames();
        this.showLoading();
        this._roleService.createOrUpdateRole(input)
            .pipe(finalize(() => (this.hideLoading())))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.visible = false;
    }
}
