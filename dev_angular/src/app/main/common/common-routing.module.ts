import { NgModule } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { BranchEditComponent } from './branchs/branch-edit.component';
import { BranchListComponent } from './branchs/branch-list.component';
import { EmployeeEditComponent } from './employees/employee-edit.component';
import { EmployeeListComponent } from './employees/employee-list.component';
import { RegionEditComponent } from './regions/region-edit.component';
import { RegionListComponent } from './regions/region-list.component';
import { SupplierEditComponent } from './supplier/supplier-edit.component';
import { SupplierListComponent } from './supplier/supplier-list.component';
import { SupplierTypeListComponent } from './supplier-type/supplier-type-list.component';
import { SupplierTypeEditComponent } from './supplier-type/supplier-type-edit.component';
import { UnitEditComponent } from './unit/unit-edit.component';
import { UnitListComponent } from './unit/unit-list.component';
import { HangHoaListComponent } from './hanghoa/hanghoa-list.component';
import { HangHoaEditComponent } from './hanghoa/hanghoa-edit.component';
import { HangHoaGroupEditComponent } from './hanghoa-group/hanghoa-group-edit.component';
import { HangHoaGroupListComponent } from './hanghoa-group/hanghoa-group-list.component';
import { HangHoaTypeEditComponent } from './hanghoa-type/hanghoa-type-edit.component';
import { HangHoaTypeListComponent } from './hanghoa-type/hanghoa-type-list.component';
import { DynamicTableListComponent } from './dynamic-table/dynamic-table.component';
import { DynamicPrimeTableListComponent } from './dynamic-prime-table/dynamic-prime-table-list.component';
import { DynamicPrimeTableEditComponent } from './dynamic-prime-table/dynamic-prime-table-edit.component';
import { DynamicPageListComponent } from './dynamic-page/dynamic-page-list.component';
import { DynamicPageEditComponent } from './dynamic-page/dynamic-page-edit.component';
@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    // Vùng miền
                    { path: 'region', component: RegionListComponent, data: { permission: 'Pages.Main.Region' } },
                    { path: 'region-add', component: RegionEditComponent, data: { permission: 'Pages.Main.Region.Create', editPageState: EditPageState.add } },
                    { path: 'region-edit', component: RegionEditComponent, data: { permission: 'Pages.Main.Region.Edit', editPageState: EditPageState.edit } },
                    { path: 'region-view', component: RegionEditComponent, data: { permission: 'Pages.Main.Region.View', editPageState: EditPageState.viewDetail } },
                    // Đơn vị
                    { path: 'branch', component: BranchListComponent, data: { permission: 'Pages.Main.Branch' } },
                    { path: 'branch-add', component: BranchEditComponent, data: { permission: 'Pages.Main.Branch.Create', editPageState: EditPageState.add } },
                    { path: 'branch-edit', component: BranchEditComponent, data: { permission: 'Pages.Main.Branch.Edit', editPageState: EditPageState.edit } },
                    { path: 'branch-view', component: BranchEditComponent, data: { permission: 'Pages.Main.Branch.View', editPageState: EditPageState.viewDetail } },
                    // Danh mục nhân viên
                    { path: 'employee', component: EmployeeListComponent, data: { permission: 'Pages.Main.Employee' } },
                    { path: 'employee-add', component: EmployeeEditComponent, data: { permission: 'Pages.Main.Employee.Create', editPageState: EditPageState.add } },
                    { path: 'employee-edit', component: EmployeeEditComponent, data: { permission: 'Pages.Main.Employee.Edit', editPageState: EditPageState.edit } },
                    { path: 'employee-view', component: EmployeeEditComponent, data: { permission: 'Pages.Main.Employee.View', editPageState: EditPageState.viewDetail } },
                    // Danh mục nhà cung cấp
                    { path: 'supplier', component: SupplierListComponent, data: { permission: 'Pages.Main.Supplier' } },
                    { path: 'supplier-add', component: SupplierEditComponent, data: { permission: 'Pages.Main.Supplier.Create', editPageState: EditPageState.add } },
                    { path: 'supplier-edit', component: SupplierEditComponent, data: { permission: 'Pages.Main.Supplier.Edit', editPageState: EditPageState.edit } },
                    { path: 'supplier-view', component: SupplierEditComponent, data: { permission: 'Pages.Main.Supplier.View', editPageState: EditPageState.viewDetail } },
                    //Danh mục loại nhà cung cấp
                    { path: 'supplier-type', component: SupplierTypeListComponent, data: { permission: 'Pages.Main.SupplierType' } },
                    { path: 'supplier-type-add', component: SupplierTypeEditComponent, data: { permission: 'Pages.Main.SupplierType.Create', editPageState: EditPageState.add } },
                    { path: 'supplier-type-edit', component: SupplierTypeEditComponent, data: { permission: 'Pages.Main.SupplierType.Edit', editPageState: EditPageState.edit } },
                    { path: 'supplier-type-view', component: SupplierTypeEditComponent, data: { permission: 'Pages.Main.SupplierType.View', editPageState: EditPageState.viewDetail } },
                    // Danh mục đơn vị tính
                    { path: 'unit', component: UnitListComponent, data: { permission: 'Pages.Main.Unit' } },
                    { path: 'unit-add', component: UnitEditComponent, data: { permission: 'Pages.Main.Unit.Create', editPageState: EditPageState.add } },
                    { path: 'unit-edit', component: UnitEditComponent, data: { permission: 'Pages.Main.Unit.Edit', editPageState: EditPageState.edit } },
                    { path: 'unit-view', component: UnitEditComponent, data: { permission: 'Pages.Main.Unit.View', editPageState: EditPageState.viewDetail } },
                    // Danh mục hàng hóa
                    { path: 'hanghoa', component: HangHoaListComponent, data: { permission: 'Pages.Main.HangHoa' } },
                    { path: 'hanghoa-add', component: HangHoaEditComponent, data: { permission: 'Pages.Main.HangHoa.Create', editPageState: EditPageState.add } },
                    { path: 'hanghoa-edit', component: HangHoaEditComponent, data: { permission: 'Pages.Main.HangHoa.Edit', editPageState: EditPageState.edit } },
                    { path: 'hanghoa-view', component: HangHoaEditComponent, data: { permission: 'Pages.Main.HangHoa.View', editPageState: EditPageState.viewDetail } },
                    // Danh mục loại hàng hóa
                    { path: 'hanghoatype', component: HangHoaTypeListComponent, data: { permission: 'Pages.Main.HangHoaType' } },
                    { path: 'hanghoatype-add', component: HangHoaTypeEditComponent, data: { permission: 'Pages.Main.HangHoaType.Create', editPageState: EditPageState.add } },
                    { path: 'hanghoatype-edit', component: HangHoaTypeEditComponent, data: { permission: 'Pages.Main.HangHoaType.Edit', editPageState: EditPageState.edit } },
                    { path: 'hanghoatype-view', component: HangHoaTypeEditComponent, data: { permission: 'Pages.Main.HangHoaType.View', editPageState: EditPageState.viewDetail } },
                    // Danh mục nhóm hàng hóa
                    { path: 'hanghoagroup', component: HangHoaGroupListComponent, data: { permission: 'Pages.Main.HangHoaGroup' } },
                    { path: 'hanghoagroup-add', component: HangHoaGroupEditComponent, data: { permission: 'Pages.Main.HangHoaGroup.Create', editPageState: EditPageState.add } },
                    { path: 'hanghoagroup-edit', component: HangHoaGroupEditComponent, data: { permission: 'Pages.Main.HangHoaGroup.Edit', editPageState: EditPageState.edit } },
                    { path: 'hanghoagroup-view', component: HangHoaGroupEditComponent, data: { permission: 'Pages.Main.HangHoaGroup.View', editPageState: EditPageState.viewDetail } },

                    // Bảng dữ liệu động
                    { path: 'dynamic-table', component: DynamicTableListComponent, data: { permission: 'Pages.Main.DynamicTable' } },

                    // Dynamic Prime Table
                    { path: 'dynamic-prime-table', component: DynamicPrimeTableListComponent, data: { permission: 'Pages.Main.DynamicPrimeTable' } },
                    { path: 'dynamic-prime-table-add', component: DynamicPrimeTableEditComponent, data: { permission: 'Pages.Main.DynamicPrimeTable.Create', editPageState: EditPageState.add } },
                    { path: 'dynamic-prime-table-edit', component: DynamicPrimeTableEditComponent, data: { permission: 'Pages.Main.DynamicPrimeTable.Edit', editPageState: EditPageState.edit } },
                    { path: 'dynamic-prime-table-view', component: DynamicPrimeTableEditComponent, data: { permission: 'Pages.Main.DynamicPrimeTable.View', editPageState: EditPageState.viewDetail } },

                    // Dynamic Page - Cấu hình trang
                    { path: 'dynamic-page', component: DynamicPageListComponent, data: { permission: 'Pages.Main.DynamicPage' } },
                    { path: 'dynamic-page-add', component: DynamicPageEditComponent, data: { permission: 'Pages.Main.DynamicPage.Create', editPageState: EditPageState.add } },
                    { path: 'dynamic-page-edit', component: DynamicPageEditComponent, data: { permission: 'Pages.Main.DynamicPage.Edit', editPageState: EditPageState.edit } },
                    { path: 'dynamic-page-view', component: DynamicPageEditComponent, data: { permission: 'Pages.Main.DynamicPage.View', editPageState: EditPageState.viewDetail } },

                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class CommonRoutingModule {

    constructor(private router: Router) {
        router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                window.scroll(0, 0);
            }
        });
    }
}
