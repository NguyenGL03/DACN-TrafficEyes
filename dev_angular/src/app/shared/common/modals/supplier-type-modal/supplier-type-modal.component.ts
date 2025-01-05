import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from "@app/shared/core/utils/consts/RecordStatusConsts";
import { CM_SUPPLIER_TYPE_ENTITY, SupplierTypeServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";

@Component({
    selector: "supplier-type-modal",
    templateUrl: "./supplier-type-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class SupplierTypeModalComponent extends PopupBaseComponent<CM_SUPPLIER_TYPE_ENTITY> implements OnInit {


    filterInput: CM_SUPPLIER_TYPE_ENTITY;
    options: PrimeTableOption<CM_SUPPLIER_TYPE_ENTITY>;

    constructor(
        injector: Injector,
        private supplierTypeServive: SupplierTypeServiceProxy
    ) {
        super(injector);
        this.title = this.l("SupplierModalTitle");
        this.filterInput = new CM_SUPPLIER_TYPE_ENTITY();
        this.filterInput.recorD_STATUS = RecordStatusConsts.Active;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;

    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'SupTypeCode', name: 'menU_ID', sortField: 'menU_ID', width: 'auto' },
                { title: 'SupTypeName', name: 'menU_NAME', sortField: 'menU_NAME', width: 'auto' },
                { title: 'Address', name: 'menU_LINK', sortField: 'menU_LINK', width: 'auto' },
                { title: 'Phone', name: 'menU_NAME_EL', sortField: 'menU_NAME_EL', width: 'auto' },
                { title: 'Email', name: 'menU_PERMISSION', sortField: 'menU_PERMISSION', width: 'auto' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }


    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInputSearch);

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        const result = await this.supplierTypeServive.cM_SUPPLIERTYPE_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading())).toPromise();
        console.log(result)
        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.dataTable.setRecords(result.items, result.totalCount);
        }

        return result;
    }
}