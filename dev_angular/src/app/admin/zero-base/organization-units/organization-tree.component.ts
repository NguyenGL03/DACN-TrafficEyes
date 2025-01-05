import { Component, EventEmitter, Injector, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    ListResultDtoOfOrganizationUnitDto,
    MoveOrganizationUnitInput,
    ORGANIZATION_UNIT_ENTITY,
    OrganizationUnitDto,
    OrganizationUnitServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { filter as _filter, remove as _remove } from 'lodash-es';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IBasicOrganizationUnitInfo, OrganizationType } from './basic-organization-unit-info';
import { CreateOrEditUnitModalComponent } from './create-or-edit-unit-modal.component';
import { IRoleWithOrganizationUnit } from './role-with-organization-unit';
import { IRolesWithOrganizationUnit } from './roles-with-organization-unit';
import { IUserWithOrganizationUnit } from './user-with-organization-unit';
import { IUsersWithOrganizationUnit } from './users-with-organization-unit';

import { MenuItem, TreeNode } from 'primeng/api';

import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { ArrayToTreeConverterService } from '@shared/utils/array-to-tree-converter.service';
import { TreeDataHelperService } from '@shared/utils/tree-data-helper.service';

export interface IOrganizationUnitOnTree extends IBasicOrganizationUnitInfo {
    id: number;
    parent: string | number;
    code: string;
    displayName: string;
    organizationCode: string;
    organizationTypeName: string;
    memberCount: number;
    roleCount: number;
    text: string;
    state: any;
}

@Component({
    selector: 'organization-tree',
    templateUrl: './organization-tree.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class OrganizationTreeComponent extends AppComponentBase implements OnInit {
    @Output() ouSelected = new EventEmitter<IBasicOrganizationUnitInfo>();

    @ViewChild('createOrEditOrganizationUnitModal', { static: true })
    createOrEditOrganizationUnitModal: CreateOrEditUnitModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;

    treeData: any;
    selectedOu: TreeNode;
    ouContextMenuItems: MenuItem[];
    canManageOrganizationUnits = false;
    totalUnitCount = 0;

    _entityTypeFullName = 'Abp.Organizations.OrganizationUnit';

    constructor(
        injector: Injector,
        private _organizationUnitService: OrganizationUnitServiceProxy,
        private _arrayToTreeConverterService: ArrayToTreeConverterService,
        private _treeDataHelperService: TreeDataHelperService
    ) {
        super(injector);
    }


    ngOnInit(): void {
        this.canManageOrganizationUnits = this.isGranted('Pages.Administration.OrganizationUnits.Create');
        this.ouContextMenuItems = this.getContextMenuItems();
        this.getTreeDataFromServer();
    }

    nodeSelect(event: any) {
        this.ouSelected.emit(<IBasicOrganizationUnitInfo>{
            id: event.node.data.id,
            displayName: event.node.data.displayName,
            organizationType: event.node.data.organizationType,
            organizationTypeName: event.node.data.organizationTypeName,
            organizationCode: event.node.data.organizationCode,
        });
    }

    isDroppingBetweenTwoNodes(event: any): boolean {
        return event.originalEvent.target.nodeName === 'LI';
    }

    nodeDrop(event) {
        const input = new MoveOrganizationUnitInput();
        input.id = event.dragNode.data.id;
        let dropNodeDisplayName = '';

        if (this.isDroppingBetweenTwoNodes(event)) {
            //between two item
            input.newParentId = event.dropNode.parent ? event.dropNode.parent.data.id : null;
            dropNodeDisplayName = event.dropNode.parent ? event.dropNode.parent.data.displayName : this.l('Root');
        } else {
            input.newParentId = event.dropNode.data.id;
            dropNodeDisplayName = event.dropNode.data.displayName;
        }

        this.message.confirm(
            this.l('OrganizationUnitMoveConfirmMessage', event.dragNode.data.displayName, dropNodeDisplayName),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._organizationUnitService
                        .oRGANIZATION_UNIT_Move(String(input.id), String(input.newParentId))
                        .pipe(
                            catchError((error) => {
                                this.revertDragDrop();
                                return throwError(error);
                            })
                        )
                        .subscribe(() => {
                            this.notify.success(this.l('SuccessfullyMoved'));
                            this.reload();
                        });
                } else {
                    this.revertDragDrop();
                }
            }
        );
    }

    revertDragDrop() {
        this.reload();
    }

    reload(): void {
        this.getTreeDataFromServer();
    }

    addUnit(parentId?: number): void {
        const item = new ORGANIZATION_UNIT_ENTITY();
        item.parentId = parentId ? String(parentId) : null;
        item.organizationType = OrganizationType.Branch;
        this.createOrEditOrganizationUnitModal.show(item);
    }

    addDepartment(parentId?: number): void {
        const item = new ORGANIZATION_UNIT_ENTITY();
        item.parentId = String(parentId);
        item.organizationType = OrganizationType.Department;
        this.createOrEditOrganizationUnitModal.show(item);
    }

    unitCreated(ou: ORGANIZATION_UNIT_ENTITY): void {
        if (ou.parentId) {
            const unit = this._treeDataHelperService.findNode(this.treeData, { data: { id: ou.parentId } });
            if (!unit) return;
            unit.children.push({
                expandedIcon: 'fa fa-folder-open text-warning',
                collapsedIcon: 'fa fa-folder text-warning',
                selected: true,
                children: [],
                data: ou
            });

        } else {
            this.treeData.push({
                expandedIcon: 'fa fa-folder-open text-warning',
                collapsedIcon: 'fa fa-folder text-warning',
                selected: true,
                children: [],
                data: ou
            });
        }

        this.totalUnitCount += 1;
    }

    deleteUnit(id) {
        let node = this._treeDataHelperService.findNode(this.treeData, { data: { id: id } });
        if (!node) {
            return;
        }

        if (!node.data.parentId) {
            _remove(this.treeData, {
                data: {
                    id: id,
                },
            });
        }

        let parentNode = this._treeDataHelperService.findNode(this.treeData, { data: { id: node.data.parentId } });
        if (!parentNode) {
            return;
        }

        _remove(parentNode.children, {
            data: {
                id: id,
            },
        });
    }

    unitUpdated(ou: OrganizationUnitDto): void {
        let item = this._treeDataHelperService.findNode(this.treeData, { data: { id: ou.id } });
        if (!item) {
            return;
        }

        item.data.displayName = ou.displayName;
        item.label = ou.displayName;
        item.memberCount = ou.memberCount;
        item.roleCount = ou.roleCount;
    }

    membersAdded(data: IUsersWithOrganizationUnit): void {
        this.incrementMemberCount(data.ouId, data.userIds.length);
    }

    memberRemoved(data: IUserWithOrganizationUnit): void {
        this.incrementMemberCount(data.ouId, -1);
    }

    incrementMemberCount(ouId: number, incrementAmount: number): void {
        let item = this._treeDataHelperService.findNode(this.treeData, { data: { id: ouId } });
        item.data.memberCount += incrementAmount;
        item.memberCount = item.data.memberCount;
    }

    rolesAdded(data: IRolesWithOrganizationUnit): void {
        this.incrementRoleCount(data.ouId, data.roleIds.length);
    }

    roleRemoved(data: IRoleWithOrganizationUnit): void {
        this.incrementRoleCount(data.ouId, -1);
    }

    incrementRoleCount(ouId: number, incrementAmount: number): void {
        let item = this._treeDataHelperService.findNode(this.treeData, { data: { id: ouId } });
        item.data.roleCount += incrementAmount;
        item.roleCount = item.data.roleCount;
    }

    private getTreeDataFromServer(): void {
        this._organizationUnitService.oRGANIZATION_UNIT_Search().subscribe((result) => {
            this.totalUnitCount = result.length;
            this.treeData = this._arrayToTreeConverterService.createTree(
                result,
                'parentId',
                'id',
                null,
                'children',
                [
                    {
                        target: 'expandedIcon',
                        value: 'fa fa-folder-open text-warning',
                    },
                    {
                        target: 'collapsedIcon',
                        value: 'fa fa-folder text-warning',
                    },
                    {
                        target: 'selectable',
                        value: true,
                    },
                ]
            );
        });
    }

    private isEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return (
            customSettings.EntityHistory &&
            customSettings.EntityHistory.isEnabled &&
            _filter(
                customSettings.EntityHistory.enabledEntities,
                (entityType) => entityType === this._entityTypeFullName
            ).length === 1
        );
    }

    private getContextMenuItems(): any[] {
        const canManageOrganizationTree = this.isGranted('Pages.Administration.OrganizationUnits.Create');

        let items = [
            {
                label: this.l('Edit'),
                disabled: !canManageOrganizationTree,
                command: (event: any) => {
                    this.createOrEditOrganizationUnitModal.show(this.selectedOu.data);
                },
            },
            {
                label: this.l('AddUnit'),
                disabled: !canManageOrganizationTree,
                command: () => {
                    this.addUnit(this.selectedOu.data.id);
                },
            },
            {
                label: this.l('AddDepartment'),
                disabled: !canManageOrganizationTree,
                command: () => {
                    this.addDepartment(this.selectedOu.data.id);
                },
            },
            {
                label: this.l('Delete'),
                disabled: !canManageOrganizationTree,
                command: () => {
                    this.message.confirm(
                        this.l('OrganizationUnitDeleteWarningMessage', this.selectedOu.data.displayName),
                        this.l('AreYouSure'),
                        (isConfirmed) => {
                            if (isConfirmed) {
                                this._organizationUnitService
                                    .oRGANIZATION_UNIT_Del(this.selectedOu.data.id)
                                    .subscribe((result) => {
                                        if (result.result == '-1') {
                                            this.notify.success(result.errorDesc || this.l('FailedDeleted'));
                                        } else {
                                            this.deleteUnit(this.selectedOu.data.id);
                                            this.notify.success(this.l('SuccessfullyDeleted'));
                                            this.selectedOu = null;
                                            this.reload();
                                            this.ouSelected.emit(null);
                                        }
                                    });
                            }
                        }
                    );
                },
            },
        ];

        if (this.isEntityHistoryEnabled()) {
            items.push({
                label: this.l('History'),
                disabled: false,
                command: (event) => {
                    this.entityTypeHistoryModal.show({
                        entityId: this.selectedOu.data.id.toString(),
                        entityTypeFullName: this._entityTypeFullName,
                        entityTypeDescription: this.selectedOu.data.displayName,
                    });
                },
            });
        }

        return items;
    }

}
