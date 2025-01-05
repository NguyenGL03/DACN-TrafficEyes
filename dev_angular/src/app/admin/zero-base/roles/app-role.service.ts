import { ChangeDetectorRef, Inject, Injectable, Optional } from '@angular/core';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DOCUMENT } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL, GetRoleForEditOutput } from '@shared/service-proxies/service-proxies';
import { AppRoleItem } from './app-role';

@Injectable()
export class AppRoleService {

    baseUrl: string;
    _appSessionService: AppSessionService;
    _http;
    cdr: ChangeDetectorRef;

    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl: string,
        appSessionService: AppSessionService,
        @Inject(DOCUMENT) private document: Document
    ) {
        this.baseUrl = baseUrl;
        this._appSessionService = appSessionService;
        this._http = http;
    }


    initChildsMenu(response: GetRoleForEditOutput): AppRoleItem[] {
        var appMenus: AppRoleItem[] = [];
        var dist = {};
        response.permissions.forEach(x => {
            var appMenu: AppRoleItem = new AppRoleItem(x.name, x.displayName);

            if (response.actions.includes(x.name.split('.')[x.name.split('.').length - 1])) {
                appMenu.isDisplay = false;
            }
            if (response.grantedPermissionNames.includes(x.name)) {
                appMenu.checked = true;
            }
            appMenu.isRootAction = x.isRootAction;
            appMenu.description = x.description;
            appMenu.isGrantedByDefault = x.isGrantedByDefault;
            appMenu.parentId = x.parentName;
            appMenus.push(appMenu);
            dist[appMenu.name] = appMenu;
            appMenu.items = [];
        })

        appMenus.forEach((x) => {

            var parent = dist[x.parentId];
            if (parent) {
                parent.items.push(x);
                x.parentId = parent.name;
            }
            x.parent = parent;
        })

        appMenus.forEach(element => {
            if (element.isRootAction && !element.parentId) {

                for (var i of response.actions) {

                    if (element.items.some(x => x.name == element.name + '.' + i)) {
                        break;
                    }
                    var rootAction = new AppRoleItem(element.name + '.' + i, element.name + '.' + i, []);
                    rootAction.isDisplay = false;

                    if (appMenus.filter(x => x.checked && response.actions.includes(x.name.split('.')[x.name.split('.').length - 1])).length > 0) {
                        rootAction.checked = false;
                    }
                    rootAction.parentId = element.name;
                    element.items.push(rootAction);
                }
            }
        });



        var appRootItem = new AppRoleItem(response.role.displayName, response.role.displayName, appMenus);

        return this.SortListActionAndDefineLeaf(appRootItem, response.actions).items;
    }


    getMenus(response: GetRoleForEditOutput, appRoleItem: AppRoleItem = null): AppRoleItem {
        var appMenus = this.initChildsMenu(response);

        appRoleItem = new AppRoleItem(response.role.displayName, response.role.displayName, appMenus.filter(x => x.parentId == null));
        return appRoleItem;
    }


    private SortListActionAndDefineLeaf(item: AppRoleItem, actions: string[]): AppRoleItem {
        if (item.isRootAction) {
            item.items.sort(function (a, b) {
                var name1 = a.name.toUpperCase();
                var name2 = b.name.toUpperCase();
                if (name1 < name2) {
                    return -1;
                }
                if (name1 > name2) {
                    return 1;
                }
                return 0;
            });
            item.isLeaf = this.DefineLeafNode(item, actions);
        }
        item.items.forEach(item => {
            return this.SortListActionAndDefineLeaf(item, actions);
        });
        return item;
    }

    private DefineLeafNode(item: AppRoleItem, actions: string[]): boolean {
        if (item.isRootAction && item.items.length == actions.length) {
            return true;
        }
        return false;
    }

    checkByParentAction(item: AppRoleItem, action: AppRoleItem, checked: boolean): void {
        var actionName = action.name.split('.')[action.name.split('.').length - 1];

        if (item.name.split('.')[item.name.split('.').length - 1] === actionName) {
            item.checked = checked;
            if (item.parent) {
                item.parent.checked = true;
            }
        }
        item.items.forEach(x => { return this.checkByParentAction(x, action, checked); });
    }

    setOnCheckParentToAction(item: AppRoleItem, roleForEdit: GetRoleForEditOutput) {
        for (var i = 0; i < roleForEdit.actions.length; i++) {
            (this.document.getElementById(item.name + '.' + roleForEdit.actions[i]) as any).checked = item.checked;
        }
    }

    checkByParent(item: AppRoleItem): void {
        if (item.items.length > 0) {
            item.items.forEach(x => {
                x.checked = item.checked;
            });
        }
        item.items.forEach(x => { return this.checkByParent(x); });
    }
}
