import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LanguagesComponent } from '@app/admin/zero-base/languages/languages.component';

const routes: Routes = [
    {
        path: '',
        component: LanguagesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LanguagesRoutingModules {}
