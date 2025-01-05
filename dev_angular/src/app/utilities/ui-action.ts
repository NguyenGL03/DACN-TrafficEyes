export interface IUiAction<TList> {
    onAdd(): void;
    onUpdate(item: TList): void;
    onDelete(item: TList): void;
    onApprove(item: TList, btnElmt?: any): void;
    onViewDetail(item: TList): void;
    onSave(): void;
    onSearch(): void;
    onResetSearch(): void;
    onReject(item: TList): void;
    onSendApp(item: TList): void;
}