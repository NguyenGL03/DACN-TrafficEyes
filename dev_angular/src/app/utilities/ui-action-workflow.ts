import { CM_PROCESS_ENTITY } from "@shared/service-proxies/service-proxies";
import { IUiAction } from "./ui-action";

export interface IUiActionWorkflow<TList> extends IUiAction<TList> {
    onReject(item: TList): void;
    onAccess(item: TList, process: CM_PROCESS_ENTITY): void;
}
