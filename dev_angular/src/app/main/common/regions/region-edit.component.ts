import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { CM_REGION_ENTITY, RegionServiceProxy, UltilityServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';

@Component({
    templateUrl: './region-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RegionEditComponent extends DefaultComponentBase implements OnInit, AfterViewInit, IUiAction<CM_REGION_ENTITY> {


    @ViewChild('editForm') editForm: ElementRef;

    inputModel: CM_REGION_ENTITY = new CM_REGION_ENTITY();
    regionItems: CM_REGION_ENTITY[];
    editPageState: EditPageState;
    isShowError: boolean = false;
    filterInput: CM_REGION_ENTITY;
    isApproveFunct: boolean;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    get editTable(): boolean {
        return this.editPageState != EditPageState.viewDetail && this.editPageState != EditPageState.edit;
    }

    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private ultilityService: UltilityServiceProxy,
        private regionService: RegionServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.regioN_ID = this.getRouteParam('id');
        console.log('id: ' + this.inputModel.regioN_ID);
        this.initFilter();
        this.initIsApproveFunct();
        console.log('Filter:', this.getFillterForCombobox());
        this.regionService.cM_REGION_Search(this.getFillterForCombobox())
            .subscribe((response) => {
                console.log(response);
                this.regionItems = response.items;
            })
        
    }
    
    onReject(item: CM_REGION_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: CM_REGION_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    ngOnInit(): void {
        this.inputModel.makeR_ID = this.appSession.user.userName;
        this.initFilter();
    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        switch (this.editPageState) {
            case EditPageState.add:
                this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
                this.appToolbar.setRole('Region', true, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('Region', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getRegion();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('Region', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getRegion();
                break;
        }
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }


    initIsApproveFunct() {
        this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe(isApproveFunct => {
            this.isApproveFunct = isApproveFunct;
        })
    }

    getRegion() {
        this.regionService.cM_REGION_ById(this.inputModel.regioN_ID).subscribe(response => {
            this.inputModel = response;
            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) {
                this.appToolbar.setButtonApproveEnable(false);
            }
            this.updateView();
        });
    }

    saveInput() {

        if (this.isApproveFunct == undefined) {
            this.showErrorMessage(this.l('PageLoadUndone'));
            return;
        }

        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.showErrorMessage(this.l('FormInvalid'));
            this.updateView();
            return;
        }
        if (this.editPageState != EditPageState.viewDetail) {
            this.saving = true;
            this.inputModel.makeR_ID = this.appSession.user.userName;
            if (!this.inputModel.regioN_ID) {
                this.regionService.cM_REGION_Ins(this.inputModel).pipe(finalize(() => { this.saving = false; }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.addNewSuccess();
                            if (!this.isApproveFunct) {
                                this.regionService.cM_REGION_App(response.id, this.appSession.user.userName)
                                    .pipe(finalize(() => { this.saving = false; }))
                                    .subscribe((response) => {
                                        if (response.result != '0') {
                                            this.showErrorMessage(response.errorDesc);
                                        }
                                    });
                            }
                        }
                    });
            }
            else {
                this.regionService.cM_REGION_Upd(this.inputModel).pipe(finalize(() => { this.saving = false; }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.updateSuccess();
                            if (!this.isApproveFunct) {
                                this.regionService.cM_REGION_App(this.inputModel.regioN_ID, this.appSession.user.userName)
                                    .pipe(finalize(() => { this.saving = false; }))
                                    .subscribe((response) => {
                                        if (response.result != '0') {
                                            this.showErrorMessage(response.errorDesc);
                                        }
                                        else {
                                            this.inputModel.autH_STATUS = AuthStatusConsts.Approve;
                                            this.appToolbar.setButtonApproveEnable(false);
                                            this.updateView();
                                        }
                                    });
                            }
                            else {
                                this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove;
                                this.updateView();
                            }
                        }
                    });
            }
        }
    }

    goBack() {
        this.navigatePassParam('/app/main/region', null, undefined);
    }

    onAdd(): void {
    }

    onUpdate(item: CM_REGION_ENTITY): void {
    }

    onDelete(item: CM_REGION_ENTITY): void {
    }

    onApprove(item: CM_REGION_ENTITY): void {
        if (!this.inputModel.regioN_ID) {
            return;
        }
        var currentUserName = this.appSession.user.userName;
        if (currentUserName == this.inputModel.makeR_ID) {
            this.showErrorMessage(this.l('ApproveFailed'));
            return;
        }
        this.message.confirm(
            this.l('ApproveWarningMessage', this.l(this.inputModel.regioN_NAME)),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.saving = true;
                    this.regionService.cM_REGION_App(this.inputModel.regioN_ID, currentUserName)
                        .pipe(finalize(() => { this.saving = false; }))
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.approveSuccess();
                            }
                        });
                }
            }
        );
    }

    onViewDetail(item: CM_REGION_ENTITY): void {
    }

    onSave(): void {
        this.saveInput();
    }

    onSearch(): void {
    }

    onResetSearch(): void {
    }
}
