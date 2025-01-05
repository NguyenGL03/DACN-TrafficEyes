export interface IListComponent<T> {
    changePage(currentPage: number): void;
    setRecords(records: T[], totalCount: number): void;
    onSetData(list : any);
}
