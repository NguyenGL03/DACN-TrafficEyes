
import { NgModule } from '@angular/core';
import { AllCodeServiceProxy, ImageServiceProxy, AsposeServiceProxy, BranchServiceProxy, CommonProcedureServiceProxy, DVDMServiceProxy, DepartmentServiceProxy, DeptGroupServiceProxy, DivisionServiceProxy, DynamicPageServiceProxy, EmployeeServiceProxy, HangHoaGroupServiceProxy, HangHoaServiceProxy, HangHoaTypeServiceProxy, RegionServiceProxy, RejectServiceProxy, RequestProcessServiceProxy, SupplierServiceProxy, SupplierTypeServiceProxy, SysGroupLimitServiceProxy, SysParametersServiceProxy, TermServiceProxy, TitleServiceProxy, TlUserServiceProxy, UltilityServiceProxy, UnitServiceProxy } from '@shared/service-proxies/service-proxies';

@NgModule({
    providers: [
        BranchServiceProxy,
        RegionServiceProxy,
        SupplierServiceProxy,
        SupplierTypeServiceProxy,
        RegionServiceProxy,
        BranchServiceProxy,
        DepartmentServiceProxy,
        EmployeeServiceProxy,
        DeptGroupServiceProxy,
        UnitServiceProxy,
        UltilityServiceProxy,
        HangHoaServiceProxy,
        HangHoaTypeServiceProxy,
        HangHoaGroupServiceProxy,
        SysGroupLimitServiceProxy,
        DVDMServiceProxy,
        AsposeServiceProxy,
        AllCodeServiceProxy,
        DynamicPageServiceProxy,
        DivisionServiceProxy,
        RejectServiceProxy,
        RequestProcessServiceProxy,
        TlUserServiceProxy,
        SysParametersServiceProxy,
        CommonProcedureServiceProxy,
        TitleServiceProxy,
        TermServiceProxy,
        ImageServiceProxy
    ]
})
export class CommonServiceProxyModule { }
