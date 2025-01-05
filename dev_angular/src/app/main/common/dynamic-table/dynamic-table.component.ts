
import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { EditPageState } from "@app/utilities/enum/edit-page-state";
import { ListComponentBase } from "@app/utilities/list-component-base";
import { IUiAction } from "@app/utilities/ui-action";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { DynamicTableServiceProxy, DYNAMIC_TABLE_MAP, DYNAMIC_PROC_ENTITY, DYNAMIC_TABLE_NEW_ENTITY, DYNAMIC_PROC_MAP, DYNAMIC_TABLE_UPDATE_INPUT, DYNAMIC_TABLE_UPDATE_ENTITY, DYNAMIC_TABLE_INPUT, DYNAMIC_TRIGGER_ENTITY } from '@shared/service-proxies/service-proxies';
import { plainToClass } from "class-transformer";
import { DataType } from "@app/utilities/enum/data-type";
import { NzConfigService } from 'ng-zorro-antd/core/config';

@Component({
    templateUrl: './dynamic-table.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styleUrl: './dynamicStyle.css'
})


export class DynamicTableListComponent extends ListComponentBase<DYNAMIC_TABLE_MAP> implements IUiAction<DYNAMIC_TABLE_MAP>, OnInit, AfterViewInit {
    inputModel: DYNAMIC_TABLE_MAP = new DYNAMIC_TABLE_MAP();
    inputModelProc: DYNAMIC_PROC_ENTITY = new DYNAMIC_PROC_ENTITY();
    inputModelTrigger: DYNAMIC_TRIGGER_ENTITY = new DYNAMIC_TRIGGER_ENTITY();
    editPageState: EditPageState;
    options: PrimeTableOption<DYNAMIC_TABLE_MAP>;
    disableInput: boolean = false;
    typeData: any[] = [];       // store data type of table
    selectedTab: string = 'DatabaseStructure';      // Current tab
    selectedCol: string = '';       // Current column
    selectedRowIndex: number | null = null;     // Index of the current selected row
    dynamicTableName: DYNAMIC_TABLE_MAP[];      // Store list of table name
    dynamicProcName: DYNAMIC_PROC_ENTITY[];       // Store list of store produce name
    dynamicTriggerName: DYNAMIC_TRIGGER_ENTITY[];       // Store list of trigger name
    getTableName: string;       // Save table name of selected table
    getProcName: string;       // Save store prodedure's name selected 
    getTriggerName: string;     // Save triiger's name selected
    selectedTableIndex: number | null = null;       // Index of the current selected table
    tableData = [       // Default table field
        {
            tableName: '',
            columns: [
                {
                    ColName: '',
                    type: '',
                    isNotNull: false,
                    isPrimaryKey: false,
                    autoIncrease: false,
                    defaultCt: '',
                    value: ''
                }
            ]
        }
    ];

    dataUpdate: any[] = [];     // Store value of column being updated
    selectedRowDBIndex: number | null = null;   // Index of the current row need update
    inputX: DYNAMIC_TABLE_INPUT = new DYNAMIC_TABLE_INPUT()     // input for insert table
    inputY: DYNAMIC_TABLE_UPDATE_INPUT = new DYNAMIC_TABLE_UPDATE_INPUT();      // input for update table
    inputZ: DYNAMIC_PROC_MAP = new DYNAMIC_PROC_MAP();
    insertedData: any[] = [];       // Store data for new table being created
    updatedData: any[] = [];        // Store the data for table being updated
    tempDBUpdate: any[] = [];       // Store the row is being deleted
    procCode: string;       // variable for store procedure's code
    originalCode: string;       // variable for old store procedure's code
    triggerCode: string;        // variable for trigger's code
    originalTriggerCode: string;        // variable for old trigger's code
    code = `-------------------- You code here --------------------`;

    ngOnInit(): void {
    }
    // make sure to destory the editor
    ngOnDestroy(): void {
    }
    constructor(
        injector: Injector,
        private changeDetector: ChangeDetectorRef,
        private _dynamicTable: DynamicTableServiceProxy,
        private nzConfigService: NzConfigService

    ) {
        super(injector);
        this.initFilter();
        this.initCombobox();
    }
    initCombobox() {
        this.typeData = Object.values(DataType).map(type => ({
            Type: type, NameType: type
        }));
        this.loadData();
        this.loadProcName();
        this.loadTriggerName();
    }
    // Load all store procedure name
    loadTriggerName() {
        this._dynamicTable.dYNAMIC_TRIGGER_GetName().subscribe((response) => {
            this.dynamicTriggerName = response;
        })
    }
    // loadtrigger's code for a given trigger's name
    loadTriggerCode(triggername: string) {
        this.changeDetector.detectChanges();
        this.getTriggerName = triggername;
        if (triggername !== null) {
            this._dynamicTable.dYNAMIC_PROC_GetProcCode(triggername).subscribe((response) => {
                var newResult = response.result.replace("CREATE TRIGGER", "ALTER TRIGGER");
                this.triggerCode = newResult;
                this.originalTriggerCode = newResult;
            })
        }
    }
    // Load all store procedure name
    loadProcName() {
        this._dynamicTable.dYNAMIC_PROC_GetName().subscribe((response) => {
            this.dynamicProcName = response;
        })
    }
    // load store procedure's code for a given store procedure's name
    loadProcCode(procname: string) {
        this.changeDetector.detectChanges();
        this.getProcName = procname;
        if (procname !== null) {
            this._dynamicTable.dYNAMIC_PROC_GetProcCode(procname).subscribe((response) => {
                var newResult = response.result.replace("CREATE PROC", "ALTER PROC");
                this.procCode = newResult
                this.originalCode = newResult;
            })
        }
    }
    // Load all table name 
    loadData() {
        this._dynamicTable.dYNAMIC_TABLE_GetTableName().subscribe((response) => {
            this.dynamicTableName = response;
        })
    }
    // Load all column name for a given table name
    loadColName(tablename: string) {
        this.changeDetector.detectChanges();
        this.getTableName = tablename;
        if (tablename !== null) {
            this._dynamicTable.dYNAMIC_TABLE_GetColName(tablename).subscribe((response) => {
                this.dataUpdate = response.map(item => ({
                    action: item.action,
                    valueType: (item.characteR_MAXIMUM_LENGTH !== null ? item.characteR_MAXIMUM_LENGTH : ''),
                    columnName: item.columN_NAME,
                    type: item.datA_TYPE,
                    tableName: item.tablE_NAME,
                    originName: item.columN_NAME
                }))
            })
        }
    }

    onCheckboxChange(index: number) {
        this.changeDetector.detectChanges();
    }

    ngAfterViewInit(): void {
        this.initDefault();
        this.appToolbar.setPrefix('Pages.Main');
        this.appToolbar.setRole('AssLiqRequest', true, true, false, true, true, true, false, true);
        this.appToolbar.setEnableForListPage();
        this.appToolbar.setUiAction(this);
        this.changeDetector.detectChanges();
    }
    search(): void {
        this.showTableLoading();
    }

    /**
     *Browse data
     */

    // Select the row for update
    selectRowUpdate(rowIndex: number) {
        this.selectedRowDBIndex = rowIndex;
    }
    // Delete a column from the update list
    onDeleteColUpdate(rowIndex: number) {
        if (this.dataUpdate[rowIndex].action === 'none') {
            this.tempDBUpdate.push({
                tableName: this.dataUpdate[rowIndex].tableName,
                columnName: this.dataUpdate[rowIndex].columnName,
                type: this.dataUpdate[rowIndex].type,
                valueType: this.dataUpdate[rowIndex].valueType,
                action: 'delete',
                originName: this.dataUpdate[rowIndex].originName
            })
        }
        this.dataUpdate.splice(rowIndex, 1);
    }
    // Handle change for the value type of the column
    onChangeValueTypeUpdate(index: number, newValue: string) {
        const column = this.dataUpdate[index];
        if (!column.originValue) {
            column.originValue = column.valueType;
        }
        if (newValue !== column.originValue) {
            column.valueType = newValue
        }
    }
    // Handle change for the value of the column
    onChangeTypeUpdate(index: number, newType: string) {
        const column = this.dataUpdate[index];
        column.type = newType;
        if (column.action !== 'rename') {
            column.originType = column.type;
            if (newType !== column.originType) {
                column.action = 'retype';
                column.type = newType;
            }
            else {
                column.type = newType;
            }
        }
    }
    //  Handle change for column name 
    onChangeNameUpdate(index: number, newName: string) {
        this.changeDetector.detectChanges();
        const column = this.dataUpdate[index];
        if (column.action !== 'add') {
            if (column.originalName === '') {
                // Store the original name if it hasn't been stored already
                column.originalName = column.columnName;
            }
            if (newName !== column.originalName) {
                // If the name has changed from the original name, set action to 'rename'
                column.action = 'rename';
                column.newName = newName;
            } else {
                // If the name is changed back to the original name, revert action to 'none'
                column.action = 'none';
                column.newName = '';
            }
        }
    }
    // Add new column to update list
    onAddNewCol(tableName: string) {
        this.dataUpdate.push(
            {
                action: 'add',
                valueType: '',
                columnName: '',
                type: '',
                tableName: tableName,
                originName: ''
            }
        )
    }
    // Update the data update list with temporary data
    onDataUpdate() {
        this.updatedData = [...this.dataUpdate];
        const tempUpdates = [...this.tempDBUpdate];
        var len = tempUpdates.length;
        for (var i = 0; i < len; i++) {
            this.updatedData.push(
                {
                    tableName: tempUpdates[i].tableName,
                    columnName: tempUpdates[i].columnName,
                    type: tempUpdates[i].type,
                    valueType: tempUpdates[i].valueType,
                    action: tempUpdates[i].action,
                    originName: tempUpdates[i].originName
                }
            )
        }
        this.tempDBUpdate = [];
    }
    // Method implement update list
    onUpdate() {
        this.onDataUpdate();
        let newTodo = plainToClass(DYNAMIC_TABLE_UPDATE_ENTITY, this.updatedData);
        this.inputY.entities = newTodo;
        this._dynamicTable.dYNAMIC_TABLE_NEW_UPDATE(this.inputY)
            .subscribe(
                (response: DYNAMIC_TABLE_UPDATE_ENTITY[]) => {
                    this.updateSuccess();
                    this.loadColName(this.getTableName);
                    console.log('Tables created successfully:', response);
                },
                (error) => {
                    console.error('Error creating tables:', error);
                }
            );
    }
    /**
     * Database Structure 
     */
    // Convert structure of list
    onData() {
        var len = this.tableData.length;
        for (var i = 0; i < len; i++) {
            this.insertedData.push(
                {
                    tableName: this.tableData[i].tableName,
                    columns: this.getRowEachTable(i)
                }
            )
        }
    }
    // Method implement insert table to database
    onAdd(): void {
        this.onData();
        let newTodo = plainToClass(DYNAMIC_TABLE_NEW_ENTITY, this.insertedData);
        this.inputX.entities = newTodo;
        this._dynamicTable.dYNAMIC_TABLE_NEW_CREATE(this.inputX)
            .subscribe(
                (response: DYNAMIC_TABLE_NEW_ENTITY[]) => {
                    this.notify.info(this.l('InsertSuccessfully'));
                    //console.log('Tables created successfully:', response);
                },
                (error) => {
                    console.error('Error creating tables:', error);
                }
            );
    }
    // Convert object each row to a string 
    getRowEachTable(indexTable: number) {
        return this.tableData[indexTable].columns.map(col => {
            return `[${col.ColName}] ${col.type}${col.type === 'varchar' || col.type === 'nvarchar' ? ` (${col.value})` : ''} ${col.type == 'BIT' ? `CHECK (${col.ColName} = 0 OR ${col.ColName}  = 1)` : ''}  ${col.isNotNull ? 'NOT NULL' : ''} ${col.isPrimaryKey ? 'PRIMARY KEY' : ''} ${col.autoIncrease ? ' IDENTITY(1,1) ' : ''} `
        }).join(', ');
    }
    // Add a new row to the table
    addRow(index: number) {
        this.tableData[index].columns.push({
            ColName: '',
            type: '',
            isNotNull: false,
            isPrimaryKey: false,
            autoIncrease: false,
            defaultCt: '',
            value: ''
        });
        this.initCombobox();
        this.selectedRowIndex = null;

    }
    // Remove a row from table
    removeRow(tableIndex: number, rowIndex: number) {
        if (rowIndex !== null) {
            this.tableData[tableIndex].columns.splice(rowIndex, 1);
            this.selectedRowIndex = null;
        }
    }
    // Select a row in the table
    selectRow(tableIndex: number, rowIndex: number) {
        this.selectedTableIndex = tableIndex;
        this.selectedRowIndex = rowIndex;
    }
    // Create new table 
    addNewTable() {
        this.tableData.push(
            {
                tableName: '',
                columns: [{
                    ColName: '',
                    type: '',
                    isNotNull: false,
                    isPrimaryKey: false,
                    autoIncrease: false,
                    defaultCt: '',
                    value: ''
                }]
            }
        );
        this.selectedTableIndex = null;
    }
    // Remove table
    removeNewTable(index: number) {
        this.tableData.splice(index, 1);
    }

    /** 
     * Procedure data
    */
   
    // Update sql query from user
    executeCode() {
        this.inputZ.queryData = this.code;
        this._dynamicTable.dYNAMIC_EXECUTE_PROC(this.inputZ)
            .subscribe(
                (response: DYNAMIC_PROC_MAP[]) => {
                    this.notify.info('Execute code successfully');
                    //console.log('Execute code successdully:', response);
                },
                (error) => {
                    console.error('Error executing code', error);
                }
            );
    }
    updateCode() {
        //console.log(this.procCode);
        this.inputZ.queryData = this.procCode;
        this._dynamicTable.dYNAMIC_EXECUTE_PROC(this.inputZ)
            .subscribe(
                (response: DYNAMIC_PROC_MAP[]) => {
                    this.notify.info('Update code successfully');
                    // console.log('Update code successdully:', response);
                },
                (error) => {
                    console.error('Error Updating code', error);
                }
            );
    }
    updateTriggerCode() {
        this.inputZ.queryData = this.triggerCode;
        this._dynamicTable.dYNAMIC_EXECUTE_PROC(this.inputZ)
            .subscribe(
                (response: DYNAMIC_PROC_MAP[]) => {
                    this.notify.info('Update trigger code');
                },
                (error) => {
                    console.error('Error executing code', error);
                }
            )
    }

    // default method
    onDelete(item: DYNAMIC_TABLE_MAP): void {
    }
    onApprove(item: DYNAMIC_TABLE_MAP): void {
    }
    onViewDetail(item: DYNAMIC_TABLE_MAP): void {
    }
    onSave(): void {
    }
    onResetSearch(): void {
    }
    onReject(item: DYNAMIC_TABLE_MAP): void {
        throw new Error("Method not implemented.");
    }
    onSendApp(item: DYNAMIC_TABLE_MAP): void {
        throw new Error("Method not implemented.");
    }

}
