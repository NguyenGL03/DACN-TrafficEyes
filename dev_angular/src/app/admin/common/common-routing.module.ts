import { NgModule } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { AllCodeEditComponent } from './all-codes/all-code-edit.component';
import { AllCodeListComponent } from './all-codes/all-code-list.component';
import { AppMenuEditComponent } from './app-menu/app-menu-edit.component';
import { AppMenuListComponent } from './app-menu/app-menu-list.component';
import { SysParameterListComponent } from './sys-parameters/sys-parameter-list.component';
import { SysParameterEditComponent } from './sys-parameters/sys-parameter-edit.component';
import { CmProcessConcenterListComponent } from './cm-process-concenter/cm-process-concenter-list.component';
import { CmProcessConcenterEditComponent } from './cm-process-concenter/cm-process-concenter-edit.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    // Thông tin trang
                    { path: 'app-menu', component: AppMenuListComponent, data: { permission: 'Pages.Administration.Menu' } },
                    { path: 'app-menu-add', component: AppMenuEditComponent, data: { permission: 'Pages.Administration.Menu.Create', editPageState: EditPageState.add } },
                    { path: 'app-menu-edit', component: AppMenuEditComponent, data: { permission: 'Pages.Administration.Menu.Edit', editPageState: EditPageState.edit } },
                    { path: 'app-menu-view', component: AppMenuEditComponent, data: { permission: 'Pages.Administration.Menu.View', editPageState: EditPageState.viewDetail } },
                    // Quy trình luồng duyệt
                    { path: 'cm-process-concenter', component: CmProcessConcenterListComponent, data: { permission: 'Pages.Administration.CmProcessConcenter' } },
                    { path: 'cm-process-concenter-add', component: CmProcessConcenterEditComponent, data: { permission: 'Pages.Administration.CmProcessConcenter.Create', editPageState: EditPageState.add } },
                    { path: 'cm-process-concenter-edit', component: CmProcessConcenterEditComponent, data: { permission: 'Pages.Administration.CmProcessConcenter.Edit', editPageState: EditPageState.edit } },
                    { path: 'cm-process-concenter-view', component: CmProcessConcenterEditComponent, data: { permission: 'Pages.Administration.CmProcessConcenter.View', editPageState: EditPageState.viewDetail } },
                    // Danh mục trường giao dịch
                    { path: 'all-code', component: AllCodeListComponent, data: { permission: 'Pages.Administration.AllCode' } },
                    { path: 'all-code-add', component: AllCodeEditComponent, data: { permission: 'Pages.Administration.AllCode.Create', editPageState: EditPageState.add } },
                    { path: 'all-code-edit', component: AllCodeEditComponent, data: { permission: 'Pages.Administration.AllCode.Edit', editPageState: EditPageState.edit } },
                    { path: 'all-code-view', component: AllCodeEditComponent, data: { permission: 'Pages.Administration.AllCode.View', editPageState: EditPageState.viewDetail } },
                    // Tham số hệ thống
                    { path: 'sys-parameter', component: SysParameterListComponent, data: { permission: 'Pages.Administration.SysParameter' } },
                    { path: 'sys-parameter-add', component: SysParameterEditComponent, data: { permission: 'Pages.Administration.SysParameter.Create', editPageState: EditPageState.add } },
                    { path: 'sys-parameter-edit', component: SysParameterEditComponent, data: { permission: 'Pages.Administration.SysParameter.Edit', editPageState: EditPageState.edit } },
                    { path: 'sys-parameter-view', component: SysParameterEditComponent, data: { permission: 'Pages.Administration.SysParameter.View', editPageState: EditPageState.viewDetail } },
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class CommonRoutingModule {
}
