import { Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { LazyDropdownResponse } from "@app/shared/core/controls/dropdown-lazy-control/dropdown-lazy-control.component";
import { PopupBaseComponent } from "@app/shared/core/controls/popup-base/popup-base.component";
import { PrimeTableOption } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { AuthStatusConsts } from "@app/shared/core/utils/consts/AuthStatusConsts";
import { BranchServiceProxy, CM_BRANCH_ENTITY, CM_DEPARTMENT_ENTITY, DepartmentServiceProxy, RoleServiceProxy, SysParametersServiceProxy, TL_USER_ENTITY, TlUserServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";

@Component({
    selector: "tl-user-modal",
    templateUrl: "./tl-user-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class TlUserModalComponent extends PopupBaseComponent<TL_USER_ENTITY> implements OnInit {

    @Input() branchTitle: string = '';
    @Input() showColPotential: boolean = true
    @Input() showColAuthStatus: boolean = true
    @Input() lstTlUserInput: TL_USER_ENTITY[];
    @Input() lstRoleInput: any[];

    departments: CM_DEPARTMENT_ENTITY[];
    allDepartments: CM_DEPARTMENT_ENTITY[];
    branchs: CM_BRANCH_ENTITY[];
    roles: any[];
    depts: CM_DEPARTMENT_ENTITY[] = [];
    lstTlUser: TL_USER_ENTITY[];
    isAbleToLoadAllUsers: boolean = true;
    depQLTS: string[];
    emptyBranchText: string;
    emptyDepText: string;
    filterCombobox: any = this.getFillterForCombobox();
    options: PrimeTableOption<TL_USER_ENTITY>;

    constructor(injector: Injector,
        private tlUserService: TlUserServiceProxy,
        private roleService: RoleServiceProxy,
        private branchService: BranchServiceProxy,
        private _departmentService: DepartmentServiceProxy,
        private sysParametersService: SysParametersServiceProxy,
        private deptService: DepartmentServiceProxy

    ) {
        super(injector);
        this.filterInput = new TL_USER_ENTITY();
        this.filterInput.top = 300;
        this.filterInput.autH_STATUS = AuthStatusConsts.Approve;
        this.filterInput.level = "ALL";
        this.keyMember = 'tlnanme';
        this.title = this.l('UsersHeaderInfo');
        this.onLoadUsersInfoScope();
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

    initComboFromApi() {
        if (this.lstTlUserInput != null) {
            this.lstTlUser = this.lstTlUserInput;
        }
        else {
            this.tlUserService.tL_USER_Search(this.getFillterForCombobox()).subscribe(result => {
                this.lstTlUser = result.items;
            });
        }

        this.filterCombobox.brancH_ID = null;
        this._departmentService.cM_DEPARTMENT_Search(this.filterCombobox).subscribe(response => {
            this.allDepartments = response.items;
        })

        if (this.lstRoleInput != null) {
            this.roles = this.lstRoleInput.map(function (item) {
                return { displayName: item.displayName, value: item.displayName };
            });
        } else {
            this.roleService.getRoles(undefined, undefined).subscribe(res => {
                this.roles = res.items.map(function (item) {
                    return { displayName: item.displayName, value: item.displayName };
                });
            })
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

    getBranches(branchId: string) {
        this.filterCombobox.brancH_ID = branchId;
        this.branchService.cM_BRANCH_Search(this.filterCombobox).subscribe(result => {
            this.branchs = result.items;
        });
    }
    getDepts(branchId: string) {
        this.filterCombobox.brancH_ID = branchId;
        this._departmentService.cM_DEPARTMENT_Search(this.filterCombobox).subscribe(response => {
            this.departments = response.items;
        })
    }

    initSelect(): void {

    }
    onLoadUsersInfoScope() {
        //phạm vi truy cập thông tin users : chỉ bộ phận qlts (phòng HCQT) + phòng QLCT&NS mới dc xem toàn bộ, còn lại thì đơn vị phòng ban nào thì chỉ xem dc của đơn vị phòng ban đó
        this.sysParametersService.sYS_PARAMETERS_ByParaKey('DEP_CODE_QLTS_HS').subscribe(response => {
            this.depQLTS = response.paraValue.split('|'); //get dep_code bộ phận qlts + phòng QLCT&NS
            this.isAbleToLoadAllUsers = !!this.depQLTS.find(dep => dep == this.appSession.user.deP_CODE && this.appSession.user.branch.brancH_TYPE == 'HS');
            this.initDefaultBranchDepTxt();
        });
    }

    initDefaultBranchDepTxt() {//disable và gán text mặc định cho đơn vị - phòng ban của user đvkd và user không thuộc bộ phận qlts hoặc phòng QLCT&NS
        if (this.appSession.user.branch.brancH_TYPE == 'HS' && !this.isAbleToLoadAllUsers || this.appSession.user.branch.brancH_TYPE != 'HS') { //user HS không thuộc BP.QLTS hoặc user ở chi nhánh / PGD
            this.getBranches(this.appSession.user.subbrId); //đơn vị: HS || chi nhánh / PGD
            this.getDepts(this.appSession.user.subbrId); //list phòng ban thuộc HS hoặc chi nhánh / PGD
            this.emptyBranchText = this.appSession.user.branchCode + " - " + this.appSession.user.branchName
            if (this.appSession.user.branch.brancH_TYPE != 'HS') {
                this.emptyDepText = this.l('SelectAll');
            } else {
                this.emptyDepText = this.appSession.user.deP_CODE + " - " + this.appSession.user.deP_NAME
                this.filterInput.deP_ID = this.appSession.user.deP_ID;
            }
            this.filterInput.tlsubbrid = this.appSession.user.subbrId;
        } else { //users thuộc BP.QLTS ở HS
            this.getBranches(null); // tất cả đơn vị trong hệ thống
            this.getDepts(null); // tất cả phòng ban trong hệ thống
            this.emptyBranchText = this.l('SelectAll');
            this.emptyDepText = this.l('SelectAll');
        }
        console.log("emptyBranchText ", this.emptyBranchText);
        console.log("emptyDepText ", this.emptyDepText);
        this.initSelect();
    }

    async getResult(checkAll: boolean = false): Promise<any> {
        this.setSortingForFilterModel(this.filterInput);

        if (checkAll) {
            this.filterInput.maxResultCount = -1;
        }
        const result = await this.tlUserService.tL_USER_Search(this.filterInput)
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
        else if (branch.brancH_ID == 'DV0001') {
            this.filterInput.deP_ID = this.appSession.user.deP_ID;
        }
        else {
            this.filterInput.deP_ID = undefined;
            this.filterInput.deP_NAME = undefined;
        }

        this.filterCombobox.brancH_ID = branch.brancH_ID;
        this.departments = this.allDepartments.filter(x => x.brancH_ID == branch.brancH_ID)
    }

    getDep(data?: LazyDropdownResponse): void {
        let filterInput = this.getFillterForCombobox();
        filterInput.isLoadHsAll = true;
        filterInput.skipCount = data.state?.skipCount;
        filterInput.totalCount = data.state?.totalCount;
        filterInput.maxResultCount = data.state?.maxResultCount;
        this.deptService.cM_DEPARTMENT_Search(filterInput)
            .subscribe(result => {
                this.depts = result.items;
                data.callback(result);
            })
    }
}
