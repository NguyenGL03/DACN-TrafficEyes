
/**
 * @interface PrimeTableOption 
 * @description Interface khởi tạo PrimeTable
 * @template T loại Object (Entity Core) được render trong PrimeTable
 */
export interface PrimeTableOption<T> {
    columns: ColumnPrimeTableOption<T>[],
    config: ConfigPrimeTableOption<T>,
}

/**
 * @interface ColumnPrimeTableOption
 * @description Interface khởi tạo column PrimeTable
 * @template T loại Object (Entity Core) được render trong PrimeTable 
 */
export interface ColumnPrimeTableOption<T> {
    /**
     * @description Field unique sử dụng cho việc filter cột
     * @type {String}
     */
    columnId?: string,
    /**
     * @description Title của cột trong PrimeTable
     * @type {string}
     */
    title: string,
    /**
     * @description Name của cột trong PrimeTable
     * @type {string}
     */
    name: string,
    /**
     * @description Liên kết các cột trong PrimeTable thành 1 group
     * @type {string}
     */
    group?: string,
    /**
     * @description ColumnType sử dụng để render dữ liệu theo type
     * @type {ColumnType}
     */
    type?: ColumnType,
    /**
     * @description Khai báo field được sử dụng để sort theo Framework Core
     * @type {string}
     */
    sortField?: string,
    /**
     * @description 
     * @type {string}
     */
    enableSort?: boolean,
    /**
     * @description 
     * @type {boolean}
     */
    required?: boolean,
    /**
     * @description 
     * @type {boolean}
     */
    editable?: boolean,
    /**
     * @description 
     * @type {string}
     */
    class?: string,
    /**
     * @description 
     * @type {string}
     */
    headerClass?: string,
    /**
     * @description 
     * @type {string}
     */
    bodyClass?: string,
    /**
     * @description Độ dài cột
     * @type {string}
     */
    width?: string,
    /**
     * @description Căn lề cho nội dung cột
     * @type {string}
     */
    align?: TextAlign,
    /**
     * @description 
     * @type {string}
     */
    template?: string,
    /**
     * @description Field chứa danh sách cho InputType.Dropdown
     * @type {Array}
     */
    items?: any[],
    /**
     * @description Field hiển thị trong DropDown
     * @type {InputType}
     */
    valueMember?: string,
    /**
     * @description Field giá trị khi chọn item trong DropDown
     * @type {InputType}
     */
    displayMember?: string,
    /**
     * @description Sử dụng để cột trong dòng dữ liệu render dưới dạng các input
     * @type {InputType}
     */
    inputType?: InputType,
    /**
     * @description Sử dụng để link tới modal cho InputType.Modal
     * @type {String}
     */
    selectorModal?: string,
    /**
     * @description 
     * @type {InputType}
     */
    defaultValue?: string | number | Date,
    /**
     * @description 
     * @type {InputType}
     */
    disabled?: boolean,
    /**
     * @description 
     * @type {InputType}
     */
    enableEdit?: boolean,
    /**
     * @description 
     * @type {InputType}
     */
    emptyText?: string,
    getDisabled?: (record?: T) => boolean,
    onChange?: (record?: T) => void,
    onFocusOut?: (record?: T) => void,
    onOpenModal?: (record?: T) => void,
    onDeleteModel?: (record?: T) => void,
    onSelect?: (data?: DataSelection<T>, index?:number) => void,
}

export interface DataSelection<T> {
    record?: T,
    selected?: any
}

export interface ConfigPrimeTableOption<T> {
    indexing?: boolean,
    checkbox?: boolean,
    isShowError?: boolean,
    isShowButtonAdd?: boolean,
    isShowButtonDelete?: boolean,
    isShowButtonExport?:boolean,
    isShowButtonImport?:boolean
}
export interface ListPrimeTable {
    name: string,
    items: any[]
}

export enum ColumnType {
    Date = 'date',
    DateTime = 'datetime',
    DateTime24h = 'datetime24h',
    Curency = 'curency',
    Percent = 'percent',
    Decimal = 'decimal'
}

export enum InputType {
    Text = 'text',
    Number = 'number',
    Currency = 'currency',
    Date = 'date',
    Datetime = 'datetime',
    Decimal = 'decimal',
    TextArea = 'textarea',
    Dropdown = 'dropdown',
    LazyDropdown = 'lazyDropdown',
    MultiSelect = 'multiSelect',
    Modal = 'Modal',
    Checkbox = 'checkbox',
    FilePicker = 'filePicker'
}

export enum TextAlign {
    Center = 'text-center',
    Left = 'text-left',
    Right = 'text-right'
}

