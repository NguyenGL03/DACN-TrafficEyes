import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    CreateOrganizationUnitInput,
    ORGANIZATION_UNIT_ENTITY,
    OrganizationUnitDto,
    OrganizationUnitServiceProxy,
    UpdateOrganizationUnitInput,
} from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'createOrEditOrganizationUnitModal',
    templateUrl: './create-or-edit-unit-modal.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class CreateOrEditUnitModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('organizationUnitDisplayName', { static: true }) organizationUnitDisplayNameInput: ElementRef;

    @Output() unitCreated: EventEmitter<ORGANIZATION_UNIT_ENTITY> = new EventEmitter<ORGANIZATION_UNIT_ENTITY>();
    @Output() unitUpdated: EventEmitter<ORGANIZATION_UNIT_ENTITY> = new EventEmitter<ORGANIZATION_UNIT_ENTITY>();

    active = false;
    saving = false;

    organizationUnit: ORGANIZATION_UNIT_ENTITY = new ORGANIZATION_UNIT_ENTITY();

    constructor(
        injector: Injector,
        private _organizationUnitService: OrganizationUnitServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    onShown(): void {
    }

    show(organizationUnit: ORGANIZATION_UNIT_ENTITY): void {
        this.organizationUnit = organizationUnit;
        this.active = true;
        this.modal.show();
        this._changeDetector.detectChanges();
    }

    save(): void {
        if (!this.organizationUnit.id) {
            this.createUnit();
        } else {
            this.updateUnit();
        }
    }

    createUnit() {
        const createInput = new ORGANIZATION_UNIT_ENTITY();
        createInput.parentId = this.organizationUnit.parentId;
        createInput.displayName = this.organizationUnit.displayName;
        createInput.creatorUserId = String(this.appSession.userId);
        createInput.organizationCode = this.organizationUnit.organizationCode;
        createInput.organizationType = this.organizationUnit.organizationType;

        this.showLoading();
        this._organizationUnitService
            .oRGANIZATION_UNIT_Ins(createInput)
            .pipe(finalize(() => (this.hideLoading())))
            .subscribe((result) => {
                if (result.result === '-1') {
                    this.notify.error(result.errorDesc || this.l('SavedFailed'));
                } else {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    createInput.id = result.attr1;
                    createInput.organizationTypeName = result.attr2;
                    this.unitCreated.emit(createInput);
                }
            });
    }

    updateUnit() {
        const updateInput = new ORGANIZATION_UNIT_ENTITY();
        updateInput.id = this.organizationUnit.id;
        updateInput.displayName = this.organizationUnit.displayName;
        updateInput.lastModifierUserId = String(this.appSession.userId);
        updateInput.organizationCode = this.organizationUnit.organizationCode;
        updateInput.organizationType = this.organizationUnit.organizationType;

        this.showLoading();
        this._organizationUnitService
            .oRGANIZATION_UNIT_Upd(updateInput)
            .pipe(finalize(() => (this.hideLoading())))
            .subscribe((result) => {
                if (result.result === '-1') {
                    this.notify.error(result.errorDesc || this.l('SavedFailed'));
                } else {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    updateInput.id = result.attr1;
                    updateInput.organizationTypeName = result.attr2;
                    this.unitUpdated.emit(updateInput);
                }
            });
    }

    close(): void {
        this.modal.hide();
        this.active = false;
    }
}
