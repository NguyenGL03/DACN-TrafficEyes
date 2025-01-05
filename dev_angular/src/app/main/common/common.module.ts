import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { BranchEditComponent } from './branchs/branch-edit.component';
import { BranchListComponent } from './branchs/branch-list.component';
import { CommonRoutingModule } from './common-routing.module';
import { CommonServiceProxyModule } from './common-service-proxy.module';
import { EmployeeEditComponent } from './employees/employee-edit.component';
import { EmployeeListComponent } from './employees/employee-list.component';
import { RegionEditComponent } from './regions/region-edit.component';
import { RegionListComponent } from './regions/region-list.component';
import { SupplierEditComponent } from './supplier/supplier-edit.component';
import { SupplierListComponent } from './supplier/supplier-list.component';
import { SupplierTypeListComponent } from './supplier-type/supplier-type-list.component';
import { SupplierTypeEditComponent } from './supplier-type/supplier-type-edit.component';

import { UnitListComponent } from './unit/unit-list.component';
import { UnitEditComponent } from './unit/unit-edit.component';
import { HangHoaEditComponent } from './hanghoa/hanghoa-edit.component';
import { HangHoaListComponent } from './hanghoa/hanghoa-list.component';
import { HangHoaGroupListComponent } from './hanghoa-group/hanghoa-group-list.component';
import { HangHoaGroupEditComponent } from './hanghoa-group/hanghoa-group-edit.component';
import { HangHoaTypeListComponent } from './hanghoa-type/hanghoa-type-list.component';
import { HangHoaTypeEditComponent } from './hanghoa-type/hanghoa-type-edit.component';
import { SequentialWorkflowDesignerModule } from 'sequential-workflow-designer-angular';
import { DynamicTableListComponent } from './dynamic-table/dynamic-table.component';
import { DynamicPrimeTableListComponent } from './dynamic-prime-table/dynamic-prime-table-list.component';
import { DynamicPrimeTableEditComponent } from './dynamic-prime-table/dynamic-prime-table-edit.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { NgxEditorModule } from 'ngx-editor';
import { NzCodeEditorModule } from 'ng-zorro-antd/code-editor';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { FormsModule } from '@angular/forms';
import { DynamicPageListComponent } from './dynamic-page/dynamic-page-list.component';
import { DynamicPageEditComponent } from './dynamic-page/dynamic-page-edit.component';
import { DynamicPageGeneratorPreviewComponent } from './dynamic-page/dynamic-page-generator-preview.component';

@NgModule({
    imports: [
        commonDeclarationImports,
        ServiceProxyModule,
        CommonServiceProxyModule,
        CommonRoutingModule,
        SequentialWorkflowDesignerModule,
        CKEditorModule,
        NgxEditorModule,
        NzCodeEditorModule,
        NzSwitchModule,
        FormsModule

    ],
    declarations: [
        RegionListComponent,
        RegionEditComponent,
        BranchListComponent,
        BranchEditComponent,
        SupplierListComponent,
        SupplierEditComponent,
        EmployeeListComponent,
        EmployeeEditComponent,
        UnitListComponent,
        UnitEditComponent,
        HangHoaListComponent,
        HangHoaEditComponent,
        HangHoaGroupListComponent,
        HangHoaGroupEditComponent,
        HangHoaTypeListComponent,
        HangHoaTypeEditComponent,
        SupplierTypeListComponent,
        DynamicTableListComponent,
        SupplierTypeEditComponent,
        DynamicPrimeTableListComponent,
        DynamicPrimeTableEditComponent,
        DynamicPageListComponent,
        DynamicPageEditComponent,
        DynamicPageGeneratorPreviewComponent,
    ],
    exports: [
    ],
    providers: [

    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],

})
export class CommonModule { }
