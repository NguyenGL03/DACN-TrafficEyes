import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { RecordStatusConsts } from "@app/shared/core/utils/consts/RecordStatusConsts";
import { CM_SUPPLIER_ENTITY, CM_SUPPLIER_TYPE_ENTITY, SupplierServiceProxy, SupplierTypeServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";

@Component({
    selector: "supplier-modal",
    templateUrl: "./supplier-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class SupplierModalComponent extends PopupBaseComponent<CM_SUPPLIER_ENTITY> implements OnInit {

    filterInput: CM_SUPPLIER_ENTITY;
    options: PrimeTableOption<CM_SUPPLIER_ENTITY>;
    supplierTypes: CM_SUPPLIER_TYPE_ENTITY[] = [];
    constructor(
        injector: Injector,
        private supplierService: SupplierServiceProxy,
        private supplierTypeSerive: SupplierTypeServiceProxy
    ) {
        super(injector);
        
        this.title = this.l("SupplierModalTitle");
        this.filterInput = new CM_SUPPLIER_ENTITY();
        this.filterInput.recorD_STATUS = RecordStatusConsts.Active;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'SupplierCode', name: 'suP_CODE', sortField: 'suP_CODE', width: 'auto' },
                { title: 'SupplierName', name: 'suP_NAME', sortField: 'suP_NAME', width: 'auto' },
                { title: 'Address', name: 'addr', sortField: 'addr', width: 'auto' },
                { title: 'Tel', name: 'tel', sortField: 'tel', width: 'auto' },
                { title: 'Email', name: 'email', sortField: 'email', width: 'auto' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }

    initCombobox() {
        this.supplierTypeSerive.cM_SUPPLIERTYPE_Search(this.getFillterForCombobox())
            .subscribe(response => {
                this.supplierTypes = response.items;
            });
    }


    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInputSearch);

        if (checkAll) this.filterInputSearch.maxResultCount = -1;

        const result = await this.supplierService.cM_SUPPLIER_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideLoading())).toPromise();

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.dataTable.setRecords(result.items, result.totalCount);
        }

        return result;
    }
}