import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, RoleServiceProxy, TL_USER_ENTITY, TlUserServiceProxy } from "@shared/service-proxies/service-proxies";
import { lastValueFrom } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
    selector: "tl-user-represent-modal",
    templateUrl: "./tl-user-represent-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class TlUserRepresentModalComponent extends PopupBaseComponent<TL_USER_ENTITY> implements OnInit {
    @Input() branchTitle: string = '';
    @Input() showColPotential: boolean = true
    @Input() showColAuthStatus: boolean = true

    departments: CM_DEPARTMENT_ENTITY[];
    branchs: CM_BRANCH_ENTITY[];
    roles: any[];

    constructor(
        injector: Injector,
        private tlUserService: TlUserServiceProxy,
        private roleService: RoleServiceProxy,
    ) {
        super(injector);
        this.filterInput = new TL_USER_ENTITY();
        this.filterInput.top = 500;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
        this.filterInput.level = "UNIT";
        this.keyMember = 'tlnanme';
        this.filterInput.roleName = 'MAKER_'

        this.initSelect();

        this.title = this.l('UsersHeaderInfo');
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'EmpCode', name: 'emP_CODE', sortField: 'emP_CODE', width: '250px' },
                { title: 'UserName', name: 'tlnanme', sortField: 'tlnanme', width: '250px' },
                { title: 'FullName', name: 'tlFullName', sortField: 'tlFullName', width: '250px' },
                { title: 'RoleName', name: 'roleName', sortField: 'roleName', width: '250px' },
                { title: 'EmpPosName', name: 'poS_NAME', sortField: 'poS_NAME', width: '250px' },
                { title: 'Branch', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '250px' },
                { title: 'Department', name: 'deP_NAME', sortField: 'deP_NAME', width: '250px' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple
            }
        }
    }

    getRolesAsString(roles: any[]): string {

        let roleNames = '';

        for (let j = 0; j < roles.length; j++) {
            if (roleNames.length) {
                roleNames = roleNames + ', ';
            }
            roleNames = roleNames + roles[j].roleName;
        }

        return roleNames;
    }

    initSelect(): void {
        // this.roleService.getRoles(undefined).subscribe(res => {
        //     this.roles = res.items.map(function (item) {
        //         if (item.displayName == 'MAKER_QLTS' || item.displayName == 'MAKER_DVSD')
        //             return { displayName: item.displayName, value: item.displayName };
        //     });
        // })
    }

    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInput);

        if (checkAll) {
            this.filterInput.maxResultCount = -1;
        }
        this.filterInput.roleName = 'MAKER_'
        var result = await lastValueFrom(this.tlUserService.tL_USER_Search(this.filterInput)
            .pipe(finalize(() => this.hideLoading())));

        if (checkAll) { 
            this.selectedItems = result.items;
        }
        else {
            this.setRecords(result.items, result.totalCount);
            this.filterInput.totalCount = result.totalCount;
        }

        return result;
    }
}
