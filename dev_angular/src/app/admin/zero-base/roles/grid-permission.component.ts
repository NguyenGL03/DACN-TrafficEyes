import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Injector, Input, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

import { GetRoleForEditOutput } from '@shared/service-proxies/service-proxies';
import { AppRoleItem } from './app-role';
import { AppRoleService } from './app-role.service';

@Component({
    templateUrl: './grid-permission.component.html',
    selector: 'grid-permission',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridPermissionComponent extends AppComponentBase {

    @ViewChild('menuAside') menuListing: any;
    @Input() treeModel: AppRoleItem[];
    roleForEdit: GetRoleForEditOutput;
    filter = '';
    treeView: AppRoleItem;

    constructor(
        injector: Injector,
        private _appRoleService: AppRoleService,
        private cdr: ChangeDetectorRef,
    ) {
        super(injector);
    }

    initPermission(result: GetRoleForEditOutput) {
        this.roleForEdit = result;
        this.treeModel = this._appRoleService.initChildsMenu(result);
        this.treeView = new AppRoleItem(result.role.displayName, result.role.displayName, this.treeModel.filter(x => x.parentId == null));
        this.cdr.detectChanges();
    }

    initRolePermission(grantedPermission: string[]) {
        this.treeModel.forEach(x => {
            x.checked = !!grantedPermission.firstOrDefault(y => y == x.name);
        })
        this.cdr.detectChanges();
    }

    getGrantedPermissionNames(): string[] {
        return this.treeModel.filter(x => x.checked).map(function (v) {
            return v.name;
        });
    }

    onCheckedParent(item: AppRoleItem) {
        this._appRoleService.checkByParent(item);
        if (item.isRootAction) {
            this._appRoleService.setOnCheckParentToAction(item, this.roleForEdit);
        }
        while (item.parent) {
            item = item.parent;
            item.checked = this.treeModel.filter(x => x.parentId == item.name && x.checked == true).length > 0;
        }
        this.cdr.detectChanges();
    }

    onCheckedParentActions(event: any, parentNode: AppRoleItem, action: AppRoleItem) {
        parentNode.checked = true;

        this._appRoleService.checkByParentAction(parentNode, action, event.target.checked);

        while (parentNode.parent) {
            parentNode = parentNode.parent;
            if (this.treeModel.filter(x => x.parentId == parentNode.name && x.checked == true).length > 0) {
                parentNode.checked = true;
            } else {
                parentNode.checked = false;
            }
        }
        this.cdr.detectChanges();
    }


    filterPermissions(event): void {
        if (this.filter) {
            this.treeView.items = this.treeModel.filter(x => x.displayName.toLowerCase().indexOf(this.filter.toLowerCase()) >= 0);
        } else {
            this.treeView.items = this.treeModel.filter(x => x.parentId == null);

        }
    }

    toggleChild(event: any): void {
        if (!event?.target) return;
        const element = event.target.closest('.m-menu__item');
        if (!element) return;
        element.classList.toggle('hidden-subnav')
    }

}
