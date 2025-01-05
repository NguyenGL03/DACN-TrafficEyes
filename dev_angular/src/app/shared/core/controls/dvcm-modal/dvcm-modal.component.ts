import { Component, Injector, Input, ViewEncapsulation } from '@angular/core';
import { CM_DVDM_ENTITY, DVDMServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PopupBaseComponent } from '../popup-base/popup-base.component-old';

@Component({
  selector: 'dvcm-modal',
  templateUrl: './dvcm-modal.component.html',
  encapsulation: ViewEncapsulation.None
})
export class DvcmModalComponent extends PopupBaseComponent<CM_DVDM_ENTITY> {

    constructor(injector: Injector, private dvdmService: DVDMServiceProxy) {
        super(injector);
        this.filterInput = new CM_DVDM_ENTITY();
        this.keyMember = 'dvdM_ID';
        this.initCombobox();
        // this.filterInput.recorD_STATUS = RecordStatusConsts.Active;
        // this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
    }

    @Input() label: string = this.l('Tìm kiếm đơn vị chuyên môn');

    @Input() filter1: string = this.l('Mã đơn vị chuyên môn');
    @Input() filter2: string = this.l('Tên đơn vị chuyên môn');
    @Input() labelListName: string = this.l('Danh sách đơn vị chuyên môn');
    @Input() col1: string = this.l('Mã đơn vị chuyên môn');
    @Input() col2: string = this.l('Tên đơn vị chuyên môn');

    initCombobox() { }

    async getResult(checkAll: boolean = false): Promise<any> {
        this.filterInputSearch = this.filterInput;
        this.setSortingForFilterModel(this.filterInputSearch);

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        var result = await this.dvdmService
            .cM_DVCM_Search(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading()))
            .toPromise();

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.dataTable.records = result.items;
            this.dataTable.totalRecordsCount = result.totalCount;
            this.filterInputSearch.totalCount = result.totalCount;
        }

        return result;
    }

}
