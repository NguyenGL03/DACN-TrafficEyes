import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from '@app/shared/core/utils/consts/RecordStatusConsts';
import { ReportTypeConsts } from "@app/shared/core/utils/consts/ReportTypeConsts";
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { AllCodes } from "@app/utilities/enum/all-codes";
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AsposeServiceProxy, CM_UNIT_ENTITY, ReportInfo, UltilityServiceProxy, UnitServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from "@shared/utils/file-download.service";
import { finalize } from 'rxjs';

@Component({
    templateUrl: './unit-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class UnitEditComponent extends DefaultComponentBase implements OnInit, AfterViewInit, IUiAction<CM_UNIT_ENTITY> {
   
    @ViewChild('editForm') editForm: ElementRef;

    EditPageState = EditPageState;
    AllCodes = AllCodes;
    editPageState: EditPageState;

    inputModel: CM_UNIT_ENTITY = new CM_UNIT_ENTITY();
    filterInput: CM_UNIT_ENTITY;
    isApproveFunct: boolean;
    isShowError = false;

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    constructor(
        injector: Injector,
        private fileDownloadService: FileDownloadService,
        private asposeService : AsposeServiceProxy,
        private ultilityService: UltilityServiceProxy,
        private unitService: UnitServiceProxy
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState'); 
        this.inputModel.uniT_ID = this.getRouteParam('id');
        this.initFilter();
        this.initIsApproveFunct();
    }

    ngOnInit(): void {
        this.inputModel.makeR_ID = this.appSession.user.userName; 
    }
 
    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        switch (this.editPageState) {
            case EditPageState.add:
                this.inputModel.recorD_STATUS = RecordStatusConsts.Active;
                this.appToolbar.setRole('Unit', true, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('Unit', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getUnit();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('Unit', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getUnit();
                break;
        }
        this.appToolbar.setUiAction(this);
        this.cdr.detectChanges();
    }

    initIsApproveFunct() {
        this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe(isApproveFunct => {
            this.isApproveFunct = isApproveFunct;
        })
    }

    getUnit() {
        this.unitService.cM_UNIT_ById(this.inputModel.uniT_ID).subscribe(response => {
            this.inputModel = response;
            if (this.inputModel.autH_STATUS == AuthStatusConsts.Approve) {
                this.appToolbar.setButtonApproveEnable(false);
            } 
        });
    }
    saveInput(): void {
        if (this.isApproveFunct == undefined) {
            this.showErrorMessage(this.l('PageLoadUndone'));
            return;
        }

        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.showErrorMessage(this.l('FormInvalid')); 
            return;
        }

        if (this.editPageState != EditPageState.viewDetail) {
            this.showLoading();
            this.inputModel.makeR_ID = this.appSession.user.userName;
            if (!this.inputModel.uniT_ID) {
                this.unitService.cM_UNIT_Ins(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.addNewSuccess();
                            if (!this.isApproveFunct) {
                                this.unitService.cM_UNIT_App(response.id, this.appSession.user.userName)
                                    .pipe(finalize(() => { this.hideLoading(); }))
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
                this.unitService.cM_UNIT_Upd(this.inputModel).pipe(finalize(() => { this.hideLoading(); }))
                    .subscribe((response) => {
                        if (response.result != '0') {
                            this.showErrorMessage(response.errorDesc);
                        }
                        else {
                            this.updateSuccess();
                            if (!this.isApproveFunct) {
                                this.unitService.cM_UNIT_App(this.inputModel.uniT_ID, this.appSession.user.userName)
                                    .pipe(finalize(() => { this.hideLoading(); }))
                                    .subscribe((response) => {
                                        if (response.result != '0') {
                                            this.showErrorMessage(response.errorDesc);
                                        }
                                        else {
                                            this.inputModel.autH_STATUS = AuthStatusConsts.Approve;
                                            this.appToolbar.setButtonApproveEnable(false); 
                                        }
                                    });
                            }
                            else {
                                this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove; 
                            }
                        }
                    });
            }
        }

    } 

    onAdd(): void {
        throw new Error('Method not implemented.');
    }

    onUpdate(item: CM_UNIT_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onDelete(item: CM_UNIT_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onApprove(item: CM_UNIT_ENTITY, btnElmt?: any): void {
        throw new Error('Method not implemented.');
    }

    onViewDetail(item: CM_UNIT_ENTITY): void {
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

    onReject(item: CM_UNIT_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    
    onSendApp(item: CM_UNIT_ENTITY): void {
        throw new Error("Method not implemented.");
    } 

    exportToExcel() {
        let reportInfo = new ReportInfo();
        reportInfo.typeExport = ReportTypeConsts.Excel;

        let reportFilter = { ...this.filterInput };

        reportFilter.maxResultCount = -1;

        reportInfo.parameters = this.GetParamsFromFilter(reportFilter)

        reportInfo.values = this.GetParamsFromFilter({
            A1 : this.l('CompanyReportHeader')
        });

        reportInfo.pathName = "/COMMON/BC_DONVITINH.xlsx";
        reportInfo.storeName = "CM_UNIT_Search";

        this.asposeService.getReport(reportInfo).subscribe(x => {
            this.fileDownloadService.downloadTempFile(x);
        });
    }
}
