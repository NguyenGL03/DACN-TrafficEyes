import { Component, Injector, Input, ViewEncapsulation } from "@angular/core";
import {
    // CM_DEVICE_ENTITY, DeviceServiceProxy
 } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";
import { PopupBaseComponent } from "../popup-base/popup-base.component-old";

@Component({
    selector: "accessory-modal",
    templateUrl: "./accessory-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class AccessoryModalComponent extends PopupBaseComponent<any> {
    constructor(injector: Injector,
        // private deviceService: DeviceServiceProxy,
    ) {
        super(injector);
        this.filterInput.top = 300;
        this.filterInput.autH_STATUS = 'A';

        this.keyMember = 'devicE_ID';

    }

    initComboFromApi(){

        // this.deviceService.cM_DEVICE_Search(this.getFillterForCombobox()).subscribe(result => {
        //     this.lstDevice = result.items;
        //     this.updateView();
        // });
    }


    @Input() showColPotential: boolean = true
    @Input() showColAuthStatus: boolean = true


    // lstDevice: CM_DEVICE_ENTITY[];
    // lstDeviceChoose: CM_DEVICE_ENTITY[];

    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInputSearch);

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        // var result = await this.deviceService.cM_DEVICE_Search(this.filterInputSearch)
        //     .pipe(finalize(() => this.hideTableLoading())).toPromise();

        // if (checkAll) {
        //     var item = "";
        //     this.selectedItems = result.items;
        // }
        // else {
        //     this.dataTable.records = result.items;
        //     this.dataTable.totalRecordsCount = result.totalCount;
        //     this.filterInputSearch.totalCount = result.totalCount;
        // }

        // return result;
    }

}
