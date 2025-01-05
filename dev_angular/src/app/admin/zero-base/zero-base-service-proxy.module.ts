
import { NgModule } from '@angular/core';
import { BranchServiceProxy, DashboardServiceProxy, DepartmentServiceProxy, EmployeeServiceProxy, RegionServiceProxy, UltilityServiceProxy } from '@shared/service-proxies/service-proxies';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';

@NgModule({
    providers: [
        ServiceProxyModule,
        EmployeeServiceProxy,
        BranchServiceProxy,
        RegionServiceProxy,
        UltilityServiceProxy,
        DepartmentServiceProxy,
        DashboardServiceProxy
    ]
})
export class ZeroBaseServiceProxyModule { }
