import { NgModule } from '@angular/core';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';
import { AdminSharedModule } from '../shared/admin-shared.module';
import { AllCodeEditComponent } from './all-codes/all-code-edit.component';
import { AllCodeListComponent } from './all-codes/all-code-list.component';
import { AppMenuEditComponent } from './app-menu/app-menu-edit.component';
import { AppMenuListComponent } from './app-menu/app-menu-list.component';
import { CommonRoutingModule } from './common-routing.module';
import { CommonServiceProxyModule } from './common-service-proxy.module';
import { SysParameterListComponent } from './sys-parameters/sys-parameter-list.component';
import { SysParameterEditComponent } from './sys-parameters/sys-parameter-edit.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CmProcessConcenterEditComponent } from './cm-process-concenter/cm-process-concenter-edit.component';
import { CmProcessConcenterListComponent } from './cm-process-concenter/cm-process-concenter-list.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SequentialWorkflowDesignerModule } from 'sequential-workflow-designer-angular';


@NgModule({
    imports: [
        commonDeclarationImports,
        AdminSharedModule,
        CommonServiceProxyModule,
        CommonRoutingModule,
        RouterModule,
        FormsModule,
        DragDropModule,
        SequentialWorkflowDesignerModule
    ],
    declarations: [
        AppMenuListComponent,
        AppMenuEditComponent,
        AllCodeListComponent,
        AllCodeEditComponent,
        SysParameterListComponent,
        SysParameterEditComponent,
        CmProcessConcenterListComponent,
        CmProcessConcenterEditComponent
    ],
    exports: [],
    providers: [],
})
export class CommonModule { }
