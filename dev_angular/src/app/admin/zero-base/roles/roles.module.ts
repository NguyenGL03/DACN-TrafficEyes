import { NgModule } from '@angular/core';
import { RolesRoutingModule } from './roles-routing.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { RolesComponent } from './roles.component';
import { CreateOrEditRoleModalComponent } from './create-or-edit-role-modal.component';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';
import { GridPermissionComponent } from './grid-permission.component';
import { AppRoleService } from './app-role.service';
import { DialogModule } from 'primeng/dialog';

@NgModule({
    declarations: [RolesComponent, CreateOrEditRoleModalComponent, GridPermissionComponent,],
    providers: [AppRoleService],
    imports: [
        commonDeclarationImports,
        AppSharedModule,
        AdminSharedModule,
        RolesRoutingModule,
        DialogModule
    ],
})
export class RolesModule { }
