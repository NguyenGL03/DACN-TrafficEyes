import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { CM_REGION_ENTITY, CM_SUPPLIER_ENTITY, CM_SUPPLIER_TYPE_ENTITY, RegionServiceProxy, SupplierServiceProxy, SupplierTypeServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';

@Component({
    templateUrl: './supplier-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SupplierEditComponent extends DefaultComponentBase implements OnInit, AfterViewInit, IUiAction<CM_SUPPLIER_ENTITY> {


    @ViewChild('editForm') editForm: ElementRef;


    inputModel: CM_SUPPLIER_ENTITY = new CM_SUPPLIER_ENTITY();
    editPageState: EditPageState;
    isShowError: boolean = false;
    supplierTypeList: CM_SUPPLIER_TYPE_ENTITY[] = [];
    regionList: CM_REGION_ENTITY[] = [];
    rExpSp: RegExp = /^\S+/;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    get editTable(): boolean {
        return this.editPageState != EditPageState.viewDetail && this.editPageState != EditPageState.edit;
    }

    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private supplierTypeService: SupplierTypeServiceProxy,
        private regionService: RegionServiceProxy,
        private supplierService: SupplierServiceProxy,
        // private ultilityService: UltilityServiceProxy,
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.suP_ID = this.getRouteParam('id');
    }

    ngOnInit(): void {
        this.inputModel.makeR_ID = this.appSession.user.userName;
        this.initCombobox();
        this.initFilter();
    }

    initCombobox() {
        this.supplierTypeService.cM_SUPPLIERTYPE_Search(this.getFillterForCombobox())
            .subscribe(result => {
                this.supplierTypeList = result.items;
            })
        this.regionService.cM_REGION_Search(this.getFillterForCombobox())
            .subscribe(result => {
                this.regionList = result.items;
            })
    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        switch (this.editPageState) {
            case EditPageState.add:
                this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
                this.appToolbar.setRole('Supplier', true, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('Supplier', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getSupplier();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('Supplier', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getSupplier();
                break;
        }
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }


    getSupplier() {
        this.supplierService.cM_SUPPLIER_ById(this.inputModel.suP_ID).subscribe(response => {
            this.inputModel = response;
            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) {
                this.appToolbar.setButtonApproveEnable(false);
            }
        });
    }

    saveInput(): void {
        console.log((this.editForm as any).form)
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.showErrorMessage(this.l('FormInvalid'));
            return;
        }

        if (!this.rExpSp.test(this.inputModel.suP_NAME)) {
            this.showErrorMessage(this.l('Tên nhà cung cấp có khoảng trắng đầu tiên,vui lòng nhập ký tự trước'));
            return;
        }

        if (this.editPageState == EditPageState.add) {
            this.addNewSupplier();
        } else {
            this.updateSupplier();
        }

    }


    addNewSupplier() {
        this.supplierService.cM_SUPPLIER_Ins(this.inputModel)
            .pipe(finalize(() => { this.hideLoading(); }))
            .subscribe(response => {
                if (response.result != '0') {
                    this.showErrorMessage(response.errorDesc)
                }
                else {
                    setTimeout(() => {
                        this.addNewSuccess();
                    }, 1500);
                    this.navigatePassParam('/app/main/supplier-edit',
                        { id: response['id'] },
                        { inputModel: JSON.stringify(this.inputModel) });
                }
            });
    }

    updateSupplier() {
        this.supplierService.cM_SUPPLIER_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
            .subscribe(response => {
                if (response.result != '0') {
                    this.showErrorMessage(response.errorDesc);
                } else {
                    this.updateSuccess();
                    this.inputModel.autH_STATUS = AuthStatusConsts.Draft;
                }
            });
    }

    onAdd(): void {
        throw new Error('Method not implemented.');
    }

    onUpdate(item: CM_SUPPLIER_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onDelete(item: CM_SUPPLIER_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onApprove(item: CM_SUPPLIER_ENTITY, btnElmt?: any): void {
        throw new Error('Method not implemented.');
    }

    onViewDetail(item: CM_SUPPLIER_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
        throw new Error('Method not implemented.');
    }

    onResetSearch(): void {
        throw new Error('Method not implemented.');
    }

    onReject(item: CM_SUPPLIER_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    onSendApp(item: CM_SUPPLIER_ENTITY): void {
        throw new Error("Method not implemented.");
    }
}
