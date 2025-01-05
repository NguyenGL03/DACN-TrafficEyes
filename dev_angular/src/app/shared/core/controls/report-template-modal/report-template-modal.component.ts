import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { DefaultComponentBase } from '@app/utilities/default-component-base';

import { ReportInfo, ReportParameter, AsposeServiceProxy as ReportServiceProxy, ReportTable, RoleEditDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ReportNoteModalComponent } from '../report-note-modal/report-note-modal.component';
import { CkeditorControlComponent } from '../ckeditor-control/ckeditor-control.component';


@Component({
    selector: 'report-template',
    templateUrl: './report-template-modal.component.html'
})
export class ReportTemplateModalComponent extends DefaultComponentBase {

    templateContent: string;
    storeData: ReportTable[];

    @ViewChild('reportNoteModal') reportNoteModal: ReportNoteModalComponent;
    @ViewChild('ckeditor') ckeditor: CkeditorControlComponent;
    @ViewChild('previewModal') previewModal: ModalDirective;
    //@ViewChild('permissionTree') permissionTree: PermissionTreeComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    // saving = false;


    role: RoleEditDto = new RoleEditDto();
    constructor(
        injector: Injector,
        private _reportService: ReportServiceProxy,
        // private _templatetService: PreviewTemplateService,
        // private _reportTemplateService: ReportTemplateServiceProxy,


    ) {
        super(injector);
    }



    show(reportTemplateCode: string, params: ReportParameter[], values: ReportParameter[]): void {

        this.active = true;
        // abp.ui.setBusy(undefined, this.l('SavingWithThreeDot'));
        // this._reportTemplateService.cM_REPORT_TEMPLATE_ByCode(reportTemplateCode).subscribe(response => {
        //     var defaultTemplate = response.reporT_TEMPLATE_DETAILs.find(x => x.isDefault == true);
        //     var reportInfo = new ReportInfo();
        //     reportInfo.storeName = response.reporT_TEMPLATE_STORE;
        //     reportInfo.parameters = params;
        //     reportInfo.values = values;
        //     this._reportService.getDataFromStore(reportInfo).subscribe(storeData => {
        //         this.storeData = storeData;
        //         this.templateContent = defaultTemplate.reporT_TEMPLATE_DETAIL_CONTENT;
        //         this.previewModal.show();
        //         this.updateParentView();
        //         abp.ui.clearBusy();
        //     });
        // });

    }

    onShown(): void {
    }

    save(event): void {

        const self = this;
        // this.saving = true;
        this.modalSave.emit(this.templateContent);
        this.close();

    }

    close(): void {
        this.active = false;
        // this.saving = false;
        this.previewModal.hide();
    }
    showReportNoteModale() {
        // var storeInfo = new ReportInfo();
        // storeInfo.storeName = storeName;
        // storeInfo.parameters = null;
        this.reportNoteModal.show(this.storeData);

    }
    print() {
        // this._templatetService.print(this.ckeditor.reportContent);
    }

}
