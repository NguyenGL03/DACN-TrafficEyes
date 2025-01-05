import { AfterViewInit, OnInit, Component, ViewEncapsulation, Injector, ChangeDetectorRef, ViewChild } from "@angular/core";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { DynamicPrimeTableServiceProxy, DYNAMIC_PRIME_TABLE_ENTITY, DYNAMIC_COLUMN_PRIME_TABLE, DYNAMIC_CONFIG_PRIME_TABLE, DYNAMIC_SCREEN_PRIME_TABLE } from '@shared/service-proxies/service-proxies';
import { InputType, PrimeTableOption, TextAlign } from '@app/shared/core/controls/primeng-table/prime-table/primte-table.interface';
import { PrimeTableComponent } from "@app/shared/core/controls/primeng-table/prime-table/prime-table.component";
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { NgForm } from "@angular/forms";
import { AuthStatusConsts } from '@app/shared/core/utils/consts/AuthStatusConsts';

@Component({
    templateUrl: './dynamic-prime-table-edit.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})

export class DynamicPrimeTableEditComponent extends DefaultComponentBase implements IUiAction<DYNAMIC_PRIME_TABLE_ENTITY>, OnInit, AfterViewInit {
    options: PrimeTableOption<DYNAMIC_COLUMN_PRIME_TABLE>;
    configsValue: any[] = [];
    inputModel: DYNAMIC_PRIME_TABLE_ENTITY = new DYNAMIC_PRIME_TABLE_ENTITY();
    inputModelConfig: DYNAMIC_CONFIG_PRIME_TABLE = new DYNAMIC_CONFIG_PRIME_TABLE();
    editPageState: EditPageState;
    dynamicAllData: DYNAMIC_PRIME_TABLE_ENTITY[];
    inputTableName: DYNAMIC_SCREEN_PRIME_TABLE = new DYNAMIC_SCREEN_PRIME_TABLE();

    defaultTable: DYNAMIC_PRIME_TABLE_ENTITY[];
    isShowError = false;
    @ViewChild('editTableView') editTableView: PrimeTableComponent<DYNAMIC_COLUMN_PRIME_TABLE>;
    @ViewChild('editForm') editForm: NgForm;

    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private _dynamicPrimeTable: DynamicPrimeTableServiceProxy,
    ) {
        super(injector);
        this.editPageState = this.getRouteData('editPageState');
        this.inputTableName.tableName = this.getRouteParam('id');
        this.initFilter();
        this.initCombobox();
        this.initValue();
    }
    onSearch(): void {
        throw new Error("Method not implemented.");
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


    loadAllData() {
        this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_GetAllData(this.inputTableName.tableName).subscribe((response) => {
            this.dynamicAllData = response;
            this.editTableView.setRecords(response[0].columns);
            this.inputModel.screenName = response[0].screenName;
            this.inputModel.tableName = response[0].tableName;
            this.inputModel.screenId = response[0].screenId;
            this.inputModel.autH_STATUS = response[0].autH_STATUS;
            this.inputModelConfig.indexing = response[0].config.indexing;
            this.inputModelConfig.checkbox = response[0].config.checkbox;
            this.inputModelConfig.isShowButtonAdd = response[0].config.isShowButtonAdd;
            this.inputModelConfig.isShowButtonDelete = response[0].config.isShowButtonDelete;
            this.inputModelConfig.isShowError = response[0].config.isShowError;
        })
    }
    onUpdate(): void {

    }
    initValue() {
        this.inputModelConfig.isShowButtonAdd = true;
        this.inputModelConfig.isShowButtonDelete = true;
        this.inputModelConfig.isShowError = true;
    }
    onViewDetail(item: DYNAMIC_PRIME_TABLE_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    async loadDefaultTable() {
        try {
            const response = await this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_GetAllData('primeTable').toPromise();
            //console.log('Default Table Response:', response);
            this.defaultTable = response;
        } catch (error) {
            //console.error('Error loading default table:', error);
        }
    }
    // ngOnInit(): void {
    //     this.options = {
    //         columns: [
    //             { title: 'title', name: 'title', sortField: 'title', width: '150px', inputType: InputType.Text },
    //             { title: 'name', name: 'name', sortField: 'name', width: '150px', inputType: InputType.Text },
    //             { title: 'sortField', name: 'sortField', sortField: 'sortField', width: '150px', inputType: InputType.Text },
    //             { title: 'width', name: 'width', sortField: 'width', width: '100px', inputType: InputType.Text },
    //             { title: 'align', name: 'align', sortField: 'align', width: '100px', inputType: InputType.Text },
    //             // { title: 'disabled', name: 'disabled', sortField: 'disabled', width: '100px', inputType: InputType.Text },
    //             { title: 'inputType', name: 'inputType', sortField: 'inputType', width: '100px', inputType: InputType.Text },
    //             { title: 'selectorModal', name: 'selectorModal', sortField: 'selectorModal', width: '100px', inputType: InputType.Text },
    //             { title: 'columnId', name: 'columnId', sortField: 'columnId', width: '100px', inputType: InputType.Text },
    //             // { title: 'methodInput', name: 'methodInput', sortField: 'methodInput', width: '250px', inputType: InputType.Text },
    //         ],
    //         config: {
    //             indexing: true,
    //             checkbox: true
    //         }
    //     }
    // }
    async ngOnInit(): Promise<void> {
        await this.loadDefaultTable();
        if (this.defaultTable && this.defaultTable.length > 0) {
            this.options = {
                columns: this.defaultTable[0].columns.map((column: any) => ({
                    ...column,
                    align: this.convertAlign(column.align),
                    inputType: this.convertType(column.inputType)
                })),
                config: this.defaultTable[0].config
            };
        } else {
            //console.error('empty or undefined');
        }
    }
    showPopupUser(item: DYNAMIC_COLUMN_PRIME_TABLE) {
        console.log(1);
    }

    convertType(inputType: string): InputType {
        switch (inputType) {
            case 'InputType.Text':
                return InputType.Text;
            case 'InputType.Modal':
                return InputType.Modal;
        }
    }
    // Use signature index
    convertAlign(textAlign: string): TextAlign {
        const textType: { [key: string]: TextAlign } = {
            'TextAlign.Center': TextAlign.Center,
            'TextAlign.Left': TextAlign.Left,
            'TextAlign.Right': TextAlign.Right
        }
        return textType[textAlign] || TextAlign.Center
    }
    get editTable(): boolean {
        return this.editPageState != EditPageState.viewDetail && this.editPageState != EditPageState.edit;
    }
    ngAfterViewInit(): void {
        this.appToolbar.setPrefix('Pages.Main');
        switch (this.editPageState) {
            case EditPageState.add:
                this.appToolbar.setRole('DynamicPrimeTable', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();

                break;
            case EditPageState.edit:
                this.appToolbar.setRole('DynamicPrimeTable', false, false, true, false, false, false, false, false);
                this.appToolbar.setEnableForEditPage();
                setTimeout(() => { this.loadAllData(); }, 100)
                break;
            case EditPageState.viewDetail:
                this.appToolbar.setRole('DynamicPrimeTable', false, false, false, false, false, false, true, false);
                this.appToolbar.setEnableForViewDetailPage();
                setTimeout(() => { this.loadAllData(); }, 100)
                break;
        }
        this.appToolbar.setUiAction(this);
        //this.hideLoading();
        this.changeDetector.detectChanges();
    }
    handleOnAdd(): DYNAMIC_COLUMN_PRIME_TABLE {
        const item = this.editTableView.addRecord();
        return item;
    }

    handleOnDelete(): void {
        this.editTableView.removeAllCheckedRecord();
    }
    onAdd(): void {
    }
    onDelete(item: DYNAMIC_PRIME_TABLE_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onApprove(item: DYNAMIC_PRIME_TABLE_ENTITY, btnElmt?: any): void {
        if (!this.inputModel.tableName)
            return;
        var currentUserName = this.appSession.user.userName;
        this.message.confirm(
            this.l('ApproveWarningMessage', this.l(this.inputModel.tableName)),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed)
                    this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_SendAppr(this.inputModel.tableName)
                        .subscribe((response) => {
                            if (response.result != '0') {
                                this.showErrorMessage(response.errorDesc);
                            }
                            else {
                                this.approveSuccess();
                            }
                        })
            }
        )
    }
    saveInput() {
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            this.showErrorMessage(this.l('FormInvalid'));
            return;
        }
        this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove;
        console.log(this.inputModel.autH_STATUS);
        this.inputModel.columns = this.editTableView.allData;
        this.inputModel.columns.forEach((column: any) => {
            column.tableName = this.inputModel.tableName;
        });
        this.inputModel.config = this.inputModelConfig;
        if (this.editPageState === EditPageState.add) {
            this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_CREATE(this.inputModel)
                .subscribe(
                    (response: DYNAMIC_PRIME_TABLE_ENTITY[]) => {
                        this.addNewSuccess();
                        console.log('Add Successful', response);
                    },
                    (error) => {
                        console.log('Error creating tables', error);
                    }
                )
        }
        else {
            this.inputModel.autH_STATUS = AuthStatusConsts.NotApprove;
            this._dynamicPrimeTable.dYNAMIC_PRIME_TABLE_UPDATE(this.inputModel)
                .subscribe(
                    (response: DYNAMIC_PRIME_TABLE_ENTITY[]) => {
                        this.updateSuccess();
                        console.log('Update Successful', response);
                    },
                    (error) => {
                        console.log('Error updating tables', error);
                    }
                )
        }

    }
    onSave(): void {
        this.saveInput();
    }
    onResetSearch(): void {
        throw new Error("Method not implemented.");
    }
    onReject(item: DYNAMIC_PRIME_TABLE_ENTITY): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: DYNAMIC_PRIME_TABLE_ENTITY): void {
        throw new Error("Method not implemented.");
    }

}
