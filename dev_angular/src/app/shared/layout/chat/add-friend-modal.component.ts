import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateFriendshipForCurrentTenantInput, CreateFriendshipRequestInput, FriendshipServiceProxy, NameValueDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs';
import { AddFromDifferentTenantModalComponent } from './add-from-different-tenant-modal.component';

@Component({
    selector: 'addFriendModal',
    templateUrl: './add-friend-modal.component.html',
})
export class AddFriendModalComponent extends AppComponentBase implements OnInit {
    @Output() itemSelected: EventEmitter<NameValueDto> = new EventEmitter<NameValueDto>();

    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @ViewChild('addFromDifferentTenantModal') verifyCodeModal: AddFromDifferentTenantModalComponent;

    public saving = false;

    tenantId?: number;
    interTenantChatAllowed = false;
    canListUsersInTenant = false;
    userName = '';

    constructor(injector: Injector, private _friendshipService: FriendshipServiceProxy) {
        super(injector);
    }
    ngOnInit(): void {
        this.interTenantChatAllowed =
            this.feature.isEnabled('App.ChatFeature.TenantToTenant') ||
            this.feature.isEnabled('App.ChatFeature.TenantToHost') ||
            !this.appSession.tenant;

        this.canListUsersInTenant = this.permission.isGranted('Pages.Administration.Users');
    }

    addFriendSelected(item: NameValueDto): void {
        const input = new CreateFriendshipRequestInput();
        input.userId = parseInt(item.value);
        input.tenantId = this.appSession.tenant ? this.appSession.tenant.id : null;

        this._friendshipService.createFriendshipRequest(input).subscribe(() => {
        });
    }

    save(): void {
        let input = new CreateFriendshipForCurrentTenantInput();
        input.userName = this.userName;

        this.showLoading();
        this._friendshipService
            .createFriendshipForCurrentTenant(input)
            .pipe(
                finalize(() => {
                    this.hideLoading();
                    this.userName = '';
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('FriendshipRequestAccepted'));
            });
    }

    show(): void {
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
    }

}
