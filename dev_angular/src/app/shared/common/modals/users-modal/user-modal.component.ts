import { ViewEncapsulation, Injector, Component, Input } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { TLUSER_GETBY_BRANCHID_ENTITY, CmUserServiceProxy, RoleListDto, RoleServiceProxy, CM_BRANCH_ENTITY, BranchServiceProxy } from "@shared/service-proxies/service-proxies";
import { lastValueFrom } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    selector: "user-modal",
    templateUrl: "./user-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class UserModalComponent extends PopupBaseComponent<TLUSER_GETBY_BRANCHID_ENTITY> {

    @Input() tlname: string;

    isFilterInfo = false;
    role_ID: number;
    roles: RoleListDto[] = [];
    branchs: CM_BRANCH_ENTITY[];

    constructor(injector: Injector,
        private roleService: RoleServiceProxy,
        private branchService: BranchServiceProxy,
        private userService: CmUserServiceProxy) {
        super(injector);
        this.filterInput = new TLUSER_GETBY_BRANCHID_ENTITY();
        this.keyMember = 'tlnanme';
        this.initCombobox();
        this.initFilterInput();
    }


    initCombobox() {
        this.roleService.getRoles(undefined, undefined).subscribe(result => {
            this.roles = result.items;
        });

        var filterCombobox = this.getFillterForCombobox();
        this.branchService.cM_BRANCH_Search(filterCombobox).subscribe(response => {
            this.branchs = response.items;
        });
    }

    onGetAssGroups(entity: RoleListDto) {
        this.filterInput.rolE_ID = entity.id;
    }

    initComboFromApi() {
        this.onGetAssGroups({ id: this.filterInput.rolE_ID } as RoleListDto)
    }

    initFilterInput() {
        this.filterInput.tlsubbrid = this.appSession.user.subbrId;
    }

    async getResult(checkAll: boolean = false): Promise<any> {
        this.filterInputSearch = this.filterInput;
        this.setSortingForFilterModel(this.filterInputSearch);

        if (checkAll) {
            this.filterInputSearch.maxResultCount = -1;
        }

        const result = await lastValueFrom(this.userService.tLUSER_GETBY_BRANCHID(this.filterInputSearch)
            .pipe(finalize(() => this.hideTableLoading())));

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.setRecords(result.items, result.totalCount);
            this.filterInputSearch.totalCount = result.totalCount;
        }

        return result;
    }

    getRolesAsString(roles): string {
        let roleNames = '';

        for (let j = 0; j < roles.length; j++) {
            if (roleNames.length) {
                roleNames = roleNames + ', ';
            }
            roleNames = roleNames + roles[j].roleName;
        }

        return roleNames;
    }

}
