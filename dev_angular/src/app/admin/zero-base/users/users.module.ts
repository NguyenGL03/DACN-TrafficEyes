import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ImpersonationService } from '@app/admin/zero-base/users/impersonation.service';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DynamicEntityPropertyManagerModule } from '@app/shared/common/dynamic-entity-property-manager/dynamic-entity-property-manager.module';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';
import { ChangeProfilePictureModalModule } from '@app/shared/layout/profile/change-profile-picture-modal.module';
import { CreateOrEditUserModalComponent } from './create-or-edit-user-modal.component';
import { EditUserPermissionsModalComponent } from './edit-user-permissions-modal.component';
import { ExportExcelUserModalComponent } from './export-excel-user-modal.component';
import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './users.component';

@NgModule({
    declarations: [UsersComponent, EditUserPermissionsModalComponent, CreateOrEditUserModalComponent, ExportExcelUserModalComponent],
    imports: [commonDeclarationImports, AppSharedModule, AdminSharedModule, UsersRoutingModule, DynamicEntityPropertyManagerModule, ChangeProfilePictureModalModule],
    providers: [ImpersonationService],
})
export class UsersModule { }
