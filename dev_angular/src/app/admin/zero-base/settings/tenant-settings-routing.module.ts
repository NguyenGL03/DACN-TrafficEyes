import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantSettingsComponent } from '@app/admin/zero-base/settings/tenant-settings.component';

const routes: Routes = [
    {
        path: '',
        component: TenantSettingsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TenantSettingsRoutingModule {}
