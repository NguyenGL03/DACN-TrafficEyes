import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { CustomizableDashboardModule } from '@app/shared/common/customizable-dashboard/customizable-dashboard.module';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';

@NgModule({
    declarations: [DashboardComponent],
    imports: [AppSharedModule, AdminSharedModule, DashboardRoutingModule, CustomizableDashboardModule, commonDeclarationImports],
})
export class DashboardModule { }
