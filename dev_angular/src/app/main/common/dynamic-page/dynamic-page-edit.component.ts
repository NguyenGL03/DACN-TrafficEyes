import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { NgForm } from "@angular/forms";
import { PrimeTableComponent } from "@app/shared/core/controls/primeng-table/prime-table/prime-table.component";
import { InputType, PrimeTableOption, TextAlign } from '@app/shared/core/controls/primeng-table/prime-table/primte-table.interface';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { DYNAMIC_PAGE_ENTITY, DYNAMIC_PAGE_INPUT_ENTITY, DynamicPageServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from "rxjs";

@Component({
    templateUrl: './dynamic-page-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})

export class DynamicPageEditComponent extends DefaultComponentBase implements IUiAction<DYNAMIC_PAGE_ENTITY>, OnInit, AfterViewInit {

    options: PrimeTableOption<DYNAMIC_PAGE_INPUT_ENTITY>;
    configsValue: any[] = [];
    inputModel: DYNAMIC_PAGE_ENTITY = new DYNAMIC_PAGE_ENTITY();
    editPageState: EditPageState;
    dynamicAllData: DYNAMIC_PAGE_ENTITY[];
    isShowError = false;
    isPreview = false;

    @ViewChild('editTableView') editTableView: PrimeTableComponent<DYNAMIC_PAGE_INPUT_ENTITY>;
    @ViewChild('editForm') editForm: NgForm;

    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private dynamicPageService: DynamicPageServiceProxy,
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputModel.pagE_ID = this.getRouteParam('id');
        this.initFilter();
        this.initCombobox();
    }

    get disableInput(): boolean {
        return this.editPageState == EditPageState.viewDetail;
    }

    initCombobox() {
        this.configsValue = [
            { nameConfig: "Yes", values: true },
            { nameConfig: "No", values: false }
        ];
    }

    onViewDetail(item: DYNAMIC_PAGE_ENTITY): void {
        throw new Error("Method not implemented.");
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'Position', name: 'position', width: '100px', inputType: InputType.Text },
                { title: 'Label', name: 'label', width: '200px', inputType: InputType.Text },
                {title: 'FieldName', name: 'fielD_NAME', width: '200px', inputType: InputType.Text,},
                { title: 'InputName', name: 'inpuT_NAME', width: '200px', inputType: InputType.Text },
                { title: 'InputModel', name: 'inpuT_MODEL', width: '200px', inputType: InputType.Text },
                { title: 'InputType', name: 'inpuT_TYPE', width: '200px' },
                { title: 'DefaultValue', name: 'defaulT_VALUE', width: '200px', inputType: InputType.Text },
                { title: 'GridWidth', name: 'griD_WIDTH', width: '200px' },
                { title: 'IsRequired', name: 'iS_REQUIRED', width: '150px', align: TextAlign.Center },
                { title: 'IsDisabled', name: 'iS_DISABLED', width: '150px', align: TextAlign.Center },
                { title: 'IsEditable', name: 'iS_EDITABLE', width: '150px', align: TextAlign.Center },
                { title: 'CustomWidth', name: 'custoM_WIDTH', width: '200px', inputType: InputType.Text },
            ],
            config: {
                indexing: true,
                checkbox: true
            }
        }
    }

    get editTable(): boolean {
        return this.editPageState != EditPageState.viewDetail && this.editPageState != EditPageState.edit;
    }

    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        switch (this.editPageState) {
            case EditPageState.add:
                this.appToolbar.setRole('DynamicPage', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.inputModel.recorD_STATUS = '1';
                break;
            case EditPageState.edit:
                this.appToolbar.setRole('DynamicPage', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                this.getDetail();
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('DynamicPage', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                this.getDetail();
                break;
        }
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }

    getDetail() {
        this.showLoading();
        this.dynamicPageService.dYNAMIC_PAGE_ById(this.inputModel.pagE_ID)
            .pipe(finalize(() => { this.hideLoading(); }))
            .subscribe(result => {
                this.inputModel = result;
                this.editTableView.setList(result.inputs);
            })
    }

    handleOnAdd(): DYNAMIC_PAGE_INPUT_ENTITY {
        const item = this.editTableView.addRecord();
        item.position = (this.editTableView.allData.length + '');
        item.inpuT_MODEL = 'InputModel';
        item.griD_WIDTH = 'col-3';
        item.inpuT_TYPE = 'text';
        return item;
    }

    handleOnDelete(): void {
        this.editTableView.removeAllCheckedRecord();
    }

    saveInput() {
        console.log(this.inputModel)
        console.log(this.editTableView.allData)
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.showErrorMessage(this.l('FormInvalid'));
            return;
        }

        this.hideLoading()
        this.inputModel.inputs = this.editTableView.allData;
        if (!this.inputModel.pagE_ID) {
            this.dynamicPageService.dYNAMIC_PAGE_Ins(this.inputModel)
                .pipe(finalize(() => { this.hideLoading() }))
                .subscribe((result) => {
                    if (result['Result'] != '0') {
                        this.showErrorMessage(result['ErrorDesc'])
                    } else {
                        this.inputModel.pagE_ID = result['PAGE_ID']
                        this.addNewSuccess();
                    }
                })
        } else {
            this.dynamicPageService.dYNAMIC_PAGE_Upd(this.inputModel)
                .pipe(finalize(() => { this.hideLoading() }))
                .subscribe((result) => {
                    if (result['Result'] != '0') {
                        this.showErrorMessage(result['ErrorDesc'])
                    } else {
                        this.inputModel.pagE_ID = result['PAGE_ID']
                        this.updateSuccess();
                    }

                })
        }

    }
    getPage(): DYNAMIC_PAGE_ENTITY {
        const _page = this.inputModel;
        _page.inputs = this.editTableView.allData;
        return _page;
    }

    onSave(): void {
        this.saveInput();
    }
    onSearch(): void {
        throw new Error("Method not implemented.");
    }
    onAdd(): void {
        throw new Error("Method not implemented.");
    }
    onUpdate(): void {
        throw new Error("Method not implemented.");
    }
    onDelete(item: DYNAMIC_PAGE_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onApprove(item: DYNAMIC_PAGE_ENTITY, btnElmt?: any): void {
        throw new Error("Method not implemented.");
    }
    onResetSearch(): void {
        throw new Error("Method not implemented.");
    }
    onReject(item: DYNAMIC_PAGE_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: DYNAMIC_PAGE_ENTITY): void {
        throw new Error("Method not implemented.");
    }

}
