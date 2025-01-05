import { NgModule } from '@angular/core';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { ZeroBaseRoutingModule } from './zero-base-routing.module';
import { ZeroBaseServiceProxyModule } from './zero-base-service-proxy.module';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';
import { AdminSharedModule } from '../shared/admin-shared.module';

@NgModule({
    imports: [
        ServiceProxyModule,
        ZeroBaseServiceProxyModule,
        ZeroBaseRoutingModule,
        commonDeclarationImports,
        AdminSharedModule
    ],
    declarations: [],
    exports: [],
    providers: []
})
export class ZeroBaseModule { }
