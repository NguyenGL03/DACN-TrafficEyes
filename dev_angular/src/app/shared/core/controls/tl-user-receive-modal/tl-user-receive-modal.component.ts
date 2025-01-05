import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, DepartmentServiceProxy, RoleServiceProxy, TL_USER_ENTITY, TlUserServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";
import { PopupBaseComponent } from "../../../core/controls/popup-base/popup-base.component";
import { AuthStatusConsts } from "../../../core/utils/consts/AuthStatusConsts";
import { PrimeTableOption } from "../primeng-table/prime-table/primte-table.interface";

@Component({
    selector: "tl-user-receive-modal",
    templateUrl: "./tl-user-receive-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class TlUserReceiveModalComponent extends PopupBaseComponent<TL_USER_ENTITY> implements OnInit {
    departments: CM_DEPARTMENT_ENTITY[];
    options: PrimeTableOption<TL_USER_ENTITY>;

    constructor(
        injector: Injector,
        private tlUserService: TlUserServiceProxy,
        private roleService: RoleServiceProxy,
        private branchService: BranchServiceProxy,
        private departmentService: DepartmentServiceProxy,

    ) {
        super(injector);
        this.filterInput = new TL_USER_ENTITY();
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
        this.filterInput.level = "ALL";
        this.filterInput.autH_STATUS = 'A';
        this.filterInput.tlsubbrid = this.appSession.user.subbrId;
        this.filterInput.deP_ID = this.appSession.user.deP_ID;
        this.keyMember = 'tlnanme';
        this.title = this.l('Tìm kiếm thông tin người dùng');
    }

    ngOnInit(): void {
        this.options = {
            columns: [
                { title: 'EmployeeCode', name: 'emP_CODE', sortField: 'emP_CODE', width: '250px' },
                { title: 'UserName', name: 'tlnanme', sortField: 'tlnanme', width: '250px' },
                { title: 'FullName', name: 'tlFullName', sortField: 'tlFullName', width: '250px' },
                { title: 'Permissions', name: 'roleName', sortField: 'roleName', width: '250px' },
                { title: 'JobPosition', name: 'poS_NAME', sortField: 'poS_NAME', width: '250px' },
                { title: 'Email', name: 'emailAddress', sortField: 'emailAddress', width: '250px' },
                { title: 'Branch', name: 'brancH_NAME', sortField: 'brancH_NAME', width: '250px' },
                { title: 'Department', name: 'deP_NAME', sortField: 'deP_NAME', width: '250px' },
                { title: 'BranchCode', name: 'brancH_CODE', sortField: 'menU_ID', width: 'auto' },
                { title: 'BranchName', name: 'brancH_NAME', sortField: 'menU_NAME', width: 'auto' },
            ],
            config: {
                indexing: true,
                checkbox: this.multiple,
            }
        }
    }

    runAfterViewed() {
        if (this.lstTlUserInput != null) {
            this.lstTlUser = this.lstTlUserInput;
        } else {
            this.tlUserService.tL_USER_Search(this.getFillterForCombobox()).subscribe(result => {
                this.lstTlUser = result.items;
                this.updateView();
            });
        }
        if (this.lstBranchInput != null) {
            this.branchs = this.lstBranchInput;
        } else {
            this.branchService.cM_BRANCH_Search(this.getFillterForCombobox()).subscribe(result => {
                this.branchs = result.items;
                super.updateView();
            });
        }

        if (this.lstRoleInput != null) {
            this.roles = this.lstRoleInput;
        } else {
            this.roleService.getRoles(undefined, undefined).subscribe(res => {
                this.roles = res.items.map(function (item) {
                    return { displayName: item.displayName, value: item.displayName };
                });
                this.updateView();
            })
        }

        // this.tlUserService.tL_USER_Search(this.getFillterForCombobox()).subscribe(result => {
        //     this.lstTlUser = result.items;
        //     this.updateView();
        // });

        // GiaNT
        // this.roleService.getRoles(undefined, undefined).subscribe(res => {
        //     this.roles = res.items.map(function (item) {
        //         return { displayName: item.displayName, value: item.displayName };
        //     });
        //     this.updateView();
        // })

    }


    @Input() branchTitle: string = ''; //= this.l('UsersHeaderInfo'); // branch use title: this.l('Search') + ' ' + this.l('BranchNameUse').toLowerCase()
    @Input() showColPotential: boolean = true
    @Input() showColAuthStatus: boolean = true

    @Input() lstBranchInput: CM_BRANCH_ENTITY[] = null;
    @Input() lstTlUserInput: TL_USER_ENTITY[];
    @Input() lstRoleInput: any[];
    branchs: CM_BRANCH_ENTITY[];
    // roles: RoleListDto[] = [];
    roles: any[];
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
    initSelect(): void {



        //sonnq
        // this.roleService.getRoles(undefined, undefined).subscribe(response => {
        //     this.roles = response.items;
        //     this.updateView();
        // })


    }


    lstTlUser: TL_USER_ENTITY[];
    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInput);

        if (checkAll) {
            this.filterInput.maxResultCount = -1;
        }

        var result = await this.tlUserService.tL_USER_Search(this.filterInput)
            .pipe(finalize(() => this.hideTableLoading())).toPromise();

        if (checkAll) {
            this.selectedItems = result.items;
        } else {
            this.setRecords(result.items, result.totalCount);
            this.filterInput.totalCount = result.totalCount;
        }

        return result;
    }
    onChangeBranch(branch: CM_BRANCH_ENTITY) {
        if (!branch) {
            branch = { brancH_ID: this.appSession.user.subbrId } as any
        }
        this.filterInput.deP_ID = undefined;
        this.filterInput.deP_NAME = undefined;
        let filterCombobox = this.getFillterForCombobox();
        filterCombobox.brancH_ID = branch.brancH_ID;
        this.departmentService.cM_DEPARTMENT_Search(filterCombobox).subscribe(response => {
            this.departments = response.items;
            this.updateView();
        })
    }
}
