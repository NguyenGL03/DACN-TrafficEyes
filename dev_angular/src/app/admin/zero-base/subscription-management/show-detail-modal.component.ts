import { Component, Injector, ViewChild } from '@angular/core';
import { PermissionTreeComponent } from '@app/admin/shared/permission-tree.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PaymentServiceProxy, SubscriptionPaymentProductDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal'; 

@Component({
    selector: 'showDetailModal',
    templateUrl: './show-detail-modal.component.html',
})
export class ShowDetailModalComponent extends AppComponentBase {
    @ViewChild('showDetailModal', { static: true }) modal: ModalDirective;
    @ViewChild('permissionTree') permissionTree: PermissionTreeComponent;

    products: SubscriptionPaymentProductDto[];
    extraProperties: string;

    resettingPermissions = false;

    constructor(injector: Injector, private _paymentService: PaymentServiceProxy) {
        super(injector);
    }

    show(paymentId: number): void {
        this._paymentService.getPayment(paymentId)
        .subscribe((result) => {
            this.products = result.subscriptionPaymentProducts;
            this.modal.show();
        });
    }

    close(): void {
        this.modal.hide();
    }
}
