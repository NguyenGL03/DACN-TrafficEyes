import { Injectable, Injector, ViewChild } from '@angular/core';
import { ApproveCustomGroupComponent } from '@app/shared/common/approve-group/approve-custom-group/approve-custom-group.component';
import { ApproveGroupServiceProxy } from '@shared/service-proxies/service-proxies';

@Injectable({ providedIn: 'root' })
export class ApproveGroupService {
    @ViewChild('approveCustomGroup') approveCustomGroup: ApproveCustomGroupComponent;

    private approveGroupServiceProxy: ApproveGroupServiceProxy;

    constructor(injector: Injector) {
        this.approveGroupServiceProxy = injector.get(ApproveGroupServiceProxy);

    }
}