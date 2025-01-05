
import { NgModule } from '@angular/core';
import { AllCodeServiceProxy, AppMenuServiceProxy, ProcessServiceProxy, ApproveGroupServiceProxy, AsposeServiceProxy, BranchServiceProxy, DepartmentServiceProxy, EmployeeServiceProxy, RegionServiceProxy, SupplierServiceProxy, SupplierTypeServiceProxy, SysParametersServiceProxy } from '@shared/service-proxies/service-proxies';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { FileDownloadService } from '@shared/utils/file-download.service';
@NgModule({
    providers: [
        ServiceProxyModule,
        AsposeServiceProxy,
        FileDownloadService,
        AppMenuServiceProxy,
        ApproveGroupServiceProxy,
        AllCodeServiceProxy,
        SysParametersServiceProxy,
        ProcessServiceProxy
    ]
})
export class CommonServiceProxyModule { }
