// #region IMPORT COMMON
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from '@angular/router';
import { AppSharedModule } from "@app/shared/app-shared.module";
import { AppCommonModule } from "@app/shared/common/app-common.module";
import { ApproveAuthorityModalComponent } from "@app/shared/common/approve-group/approve-authority-modal/approve-authority-modal.component";
import { ApproveCustomGroupViewComponent } from "@app/shared/common/approve-group/approve-custom-group-view/approve-custom-group-view.component";
import { ApproveCustomGroupComponent } from "@app/shared/common/approve-group/approve-custom-group/approve-custom-group.component";
import { ApproveRejectModalComponent } from "@app/shared/common/approve-group/approve-reject-modal/approve-reject-modal.component";
import { FileUploaderTableModalComponent } from "@app/shared/common/file-uploader-table/file-uploader-table-modal.component";
import { FileUploaderTableComponent } from "@app/shared/common/file-uploader-table/file-uploader-table.component";
import { FileUploaderMultiModalComponent } from "@app/shared/common/file-uploader/file-uploader-multi-modal.component";
import { FileUploaderComponent } from "@app/shared/common/file-uploader/file-uploader.component";
import { ApproveModalComponent } from "@app/shared/common/modals/approve-modal/approve-modal.component";
import { BranchModalComponent } from "@app/shared/common/modals/branch-modal/branch-modal.component";
import { DepartmentModalComponent } from "@app/shared/common/modals/dep-modal/department-modal.component";
import { DivisionModalComponent } from "@app/shared/common/modals/division-modal/division-modal.component";
import { DVDMModalComponent as DVDMModalComponent2 } from "@app/shared/common/modals/dvdm-modal/dvdm-modal.component";
import { EmployeeExistInTluserModalComponent } from "@app/shared/common/modals/employee-exist-in-tluser-modal/employee-exist-in-tluser-modal.component";
import { EmployeeMappingModalComponent } from "@app/shared/common/modals/employee-mapping-modal/employee-mapping-modal.component";
import { EmployeeModalComponent } from "@app/shared/common/modals/employee-modal/employee-modal.component";
import { PopupFrameComponent } from "@app/shared/common/modals/popup-frames/popup-frame.component";
import { RejectModalComponent } from "@app/shared/common/modals/reject-modal/reject-modal.component";
import { SupplierModalComponent } from "@app/shared/common/modals/supplier-modal/supplier-modal.component";
import { SupplierTypeModalComponent } from "@app/shared/common/modals/supplier-type-modal/supplier-type-modal.component";
import { TlUserModalComponent } from "@app/shared/common/modals/tl-user-modal/tl-user-modal.component";
import { TlUserRepresentModalComponent } from "@app/shared/common/modals/tl-user-represent-modal/tl-user-represent-modal.component";
import { UserModalComponent } from "@app/shared/common/modals/users-modal/user-modal.component";
import { ProcessHistoryViewComponent } from "@app/shared/common/process-history-view/process-history-view.component";
import { UtilsModule } from "@shared/utils/utils.module";
import { NgApexchartsModule } from "ng-apexcharts";
import { FileUploadModule } from "ng2-file-upload";
import { ModalModule } from "ngx-bootstrap/modal";
import { TabsModule } from "ngx-bootstrap/tabs";
import { NgxCleaveDirectiveModule } from "ngx-cleave-directive";
import { AccordionModule } from 'primeng/accordion';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ChartModule } from 'primeng/chart';
import { DialogModule } from 'primeng/dialog';
import { FieldsetModule } from "primeng/fieldset";
import { InputTextModule } from "primeng/inputtext";
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MultiSelectModule } from 'primeng/multiselect';
import { PaginatorModule } from "primeng/paginator";
import { SplitButtonModule } from 'primeng/splitbutton';
import { StepsModule } from 'primeng/steps';
import { TableModule } from 'primeng/table';
import { TreeModule } from "primeng/tree";
import { FileMultiComponent } from "../../common/file-modal/file-multi.component";
import { FilePickerComponent } from "../../common/file-modal/file-picker.component";
import { AccessoryModalComponent } from "../controls/accessory-modal/accessory-modal.component";
import { AllCodeSelectComponent } from "../controls/allCodes/all-code-select.component";
import { AuthStatusInputPageInventoryComponent } from "../controls/auth-status-input-page-inventory/auth-status-input-page-inventory.component";
import { AuthStatusInputPageComponent } from "../controls/auth-status-input-page/auth-status-input-page.component";
import { AutoCompleteCustomClickComponent } from "../controls/auto-complete-custom-click/auto-complete-custom-click.component";
import { AutoCompleteComponent } from "../controls/auto-complete/auto-complete.component";
import { BranchDepModalComponent } from "../controls/branch-dep-modal/branch-dep-modal.component";
import { BreadcrumbsComponent } from "../controls/breadcrumb/breadcrumbs.component";
import { ChartCoreComponent } from "../controls/chart-core/chart-core.component";
import { CheckboxControlComponent } from "../controls/checkbox-control/checkbox-control.component";
import { CkeditorControlComponent } from "../controls/ckeditor-control/ckeditor-control.component";
import { CoreTableComponent } from "../controls/core-table/core-table.component";
import { CurrentProcessViewComponent } from "../controls/current-process-view/current-process-view.component";
import { CurrentProcessWorkflowComponent } from "../controls/current-process-workflow/current-process-workflow.component";
import { CustomFlatpickrTimeComponent } from "../controls/custom-flatpickr-time/custom-flatpickr-time.component";
import { CustomFlatpickrComponent } from "../controls/custom-flatpickr/custom-flatpickr.component";
import { Select2CustomComponent } from "../controls/custom-select2/select2-custom.component";
import { DepartmentCostCenterModalComponent } from "../controls/department-modal/department-costcenter-modal.component";
import { DisabledInputComponent } from "../controls/disabledInput/disabled-input.component";
import { DisplayComponent } from "../controls/display/display.component";
import { DropdownControlComponent } from "../controls/dropdown-control/dropdown-control.component";
import { DropdownLazyControlComponent } from "../controls/dropdown-lazy-control/dropdown-lazy-control.component";
import { DvcmModalComponent } from "../controls/dvcm-modal/dvcm-modal.component";
import { DVDMModalComponent } from "../controls/dvdm-modal/dvdm-modal.component";
import { EditableTableComponent } from "../controls/editable-table/editable-table.component";
import { FileUploaderMultiModalEditableComponent } from "../controls/file-uploader-editable/file-multi-modal-editable.component";
import { FileUploaderEditableComponent } from "../controls/file-uploader-editable/file-uploader-editable.component";
import { FileUploaderMultiModalComponentOld } from "../controls/file-uploader/file-uploader-multi-modal.component";
import { FileUploaderOldComponent } from "../controls/file-uploader/file-uploader.component";
import { HeaderComponent } from "../controls/header/header.component";
import { ImageCarouselUploadModalComponent } from "../controls/image-carousel/image-carousel-upload-modal.component";
import { ImageCarouselComponent } from "../controls/image-carousel/image-carousel.component";
import { ImportButtonComponent } from "../controls/import-button/import-button.component";
import { ImportExcelComponent } from "../controls/import-excel/import-excel.component";
import { PrimeTableToolbarComponent } from "../controls/import-export-button/prime-table-toolbar.component";
import { InputControlComponent } from "../controls/input-control/input-control.component";
import { InputDateControlComponent } from "../controls/input-date-control/input-date-control.component";
import { InputModalControlComponent } from "../controls/input-modal-control/input-modal-control.component";
import { InputMoneyControlComponent } from "../controls/input-money-control/input-money-control.component";
import { InputSelectComponent } from "../controls/input-select-control/input-select-control.component";
import { InputSelectLinkComponent } from "../controls/input-select-link-control/input-select-link-control.component";
import { BranchInventoryModalComponent } from "../controls/inventory-branch-modal/inventory-branch-modal.component";
import { LocationControlComponent } from "../controls/localtion-control/location-control.component";
import { MoneyInputComponent } from "../controls/money-input/money-input.component";
import { Paginator2Component } from "../controls/p-paginator2/p-paginator2.component";
import { PLanBranchModalComponent } from "../controls/pl-branch-modal/pl-branch-modal.component";
import { PopupBaseDefaultComponent } from "../controls/popup-base/popup-base-default.component";
import { PopupBaseComponent } from "../controls/popup-base/popup-base.component";
import { PopupBaseComponent as PopupBaseComponentOld } from "../controls/popup-base/popup-base.component-old";
import { PopupFrameComponent as PopupFrameComponentOld } from "../controls/popup-frames/popup-frame.component-old";
import { PrimeTableInputComponent } from "../controls/primeng-table/prime-input/prime-table-input.component";
import { PrimeTableComponent } from "../controls/primeng-table/prime-table/prime-table.component";
import { RadioControlComponent } from "../controls/radio-control/radio-control.component";
import { RejectModalComponentWf } from "../controls/reject-modal-wf/reject-modal-wf.component";
import { RejectNotesComponent } from "../controls/reject-notes/reject-notes.component";
import { ReportNoteModalComponent } from "../controls/report-note-modal/report-note-modal.component";
import { ReportTemplateModalComponent } from "../controls/report-template-modal/report-template-modal.component";
import { SelectMultiComponent } from "../controls/select-multi/select-multi.component";
import { TextAreaControlComponent } from "../controls/textarea-control/textarea-control.component";
import { TlUserReceiveModalComponent } from "../controls/tl-user-receive-modal/tl-user-receive-modal.component";
import { ToolbarRejectExtComponent } from "../controls/toolbar-reject-ext/toolbar-reject-ext.component";
import { ToolbarWorkFlowComponent } from "../controls/toolbar-workflow/toolbar-workflow.component";
import { ToolbarComponent } from "../controls/toolbar/toolbar.component";
import { DateFormatPipe } from "../pipes/date-format.pipe";
import { DateTimeFormatPipe } from "../pipes/date-time-format.pipe";
import { DateTime24hFormatPipe } from "../pipes/date-time-format.pipe-24h";
import { MoneyFormatPipe } from "../pipes/money-format.pipe";
import { nl2brPipe } from "../pipes/nl2br.pipe";
import { PrimeFormatPipe } from "../pipes/prime-column-format.pipe";
import { ApproveGroupEditComponent } from "@app/shared/common/approve-group/approve-group-edit-staging/approve-group-edit.component";
import { ApproveGroupViewComponent } from "@app/shared/common/approve-group/approve-group-view-staging/approve-group-view.component";
import { ApproveGroupComponent } from "@app/shared/common/approve-group/approve-group/approve-group.component";
import { DropdownStatusWorkflowComponent } from "../controls/dropdown-status-workflow/dropdown-status-workflow.component";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
// #endregion IMPORT COMMON
export const commonDeclarationDeclarations = [
    // #region COMMON
    PrimeTableComponent,
    PrimeTableInputComponent,
    PrimeTableToolbarComponent,
    InputControlComponent,
    InputDateControlComponent,
    InputModalControlComponent,
    CheckboxControlComponent,
    DropdownControlComponent,
    DropdownLazyControlComponent,
    DropdownStatusWorkflowComponent,
    TextAreaControlComponent,
    AllCodeSelectComponent,
    InputMoneyControlComponent,
    ChartCoreComponent,
    ToolbarComponent,
    HeaderComponent,
    BreadcrumbsComponent,
    ApproveGroupComponent,
    ApproveGroupViewComponent,
    ApproveGroupEditComponent,
    ApproveCustomGroupComponent,
    ApproveCustomGroupViewComponent,
    ApproveRejectModalComponent,
    ApproveAuthorityModalComponent,
    ProcessHistoryViewComponent,
    FileUploaderComponent,
    FileUploaderMultiModalComponent,
    FileUploaderTableComponent,
    FileUploaderTableModalComponent,
    FilePickerComponent,
    FileMultiComponent,
    ImportExcelComponent,
    ImportButtonComponent, 
    PopupBaseDefaultComponent,
    PopupBaseComponent,
    PopupFrameComponent,
    ApproveModalComponent,
    RejectModalComponent,
    SupplierModalComponent,
    SupplierTypeModalComponent,
    BranchModalComponent,
    DepartmentModalComponent,
    DVDMModalComponent,
    EmployeeMappingModalComponent,
    EmployeeModalComponent,
    UserModalComponent,
    TlUserModalComponent,
    TlUserRepresentModalComponent,
    DivisionModalComponent,
    EmployeeExistInTluserModalComponent,
    AuthStatusInputPageInventoryComponent,
    AuthStatusInputPageComponent,
    CurrentProcessViewComponent,
    BranchInventoryModalComponent,
    ImageCarouselComponent,
    ImageCarouselUploadModalComponent,
    EditableTableComponent,
    CoreTableComponent,
    Select2CustomComponent,
    Paginator2Component,
    CustomFlatpickrComponent,
    ReportTemplateModalComponent,
    ReportNoteModalComponent,
    CkeditorControlComponent,
    MoneyInputComponent,
    ToolbarRejectExtComponent,
    PopupBaseComponentOld,
    PopupFrameComponentOld,
    DisabledInputComponent,
    SelectMultiComponent, FileUploaderEditableComponent, FileUploaderMultiModalEditableComponent,
    TlUserReceiveModalComponent,
    InputSelectComponent, FileUploaderMultiModalComponentOld,
    FileUploaderOldComponent,
    AccessoryModalComponent,
    CustomFlatpickrTimeComponent,
    AutoCompleteComponent,
    DepartmentCostCenterModalComponent,
    PLanBranchModalComponent,
    BranchDepModalComponent,
    RadioControlComponent,
    DisplayComponent,
    RejectNotesComponent, 
    RejectModalComponentWf,
    ToolbarWorkFlowComponent, CurrentProcessWorkflowComponent, AutoCompleteCustomClickComponent,
    InputSelectLinkComponent,
    DvcmModalComponent,
    LocationControlComponent,
    RadioControlComponent,
    DVDMModalComponent2,
    // #endregion COMMON

    // #region PIPE
    PrimeFormatPipe,
    MoneyFormatPipe,
    DateFormatPipe,
    DateTimeFormatPipe,
    DateTime24hFormatPipe,
    nl2brPipe,
    // #endregion PIPE

];

@NgModule({
    imports: [
        ReactiveFormsModule,
        FormsModule,
        CommonModule,
        ModalModule.forRoot(),
        TabsModule.forRoot(),
        TreeModule,
        UtilsModule,
        PaginatorModule,
        TableModule,
        RouterModule,
        ButtonModule,
        CalendarModule,
        FileUploadModule,
        InputTextareaModule,
        DialogModule,
        AccordionModule,
        MultiSelectModule,
        NgxCleaveDirectiveModule,
        StepsModule,
        FieldsetModule,
        SplitButtonModule,
        InputTextModule,
        NgApexchartsModule,
        ChartModule,
        BsDropdownModule,
    ],
    declarations: [commonDeclarationDeclarations],
    exports: [commonDeclarationDeclarations],
    providers: [],
})
export class CommonDeclarationDeclarationModule { }


export const commonDeclarationImports = [
    AppSharedModule,
    AppCommonModule,
    NgxCleaveDirectiveModule,
    CommonDeclarationDeclarationModule,
];
