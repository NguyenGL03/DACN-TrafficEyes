import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { ApproveCustomGroupComponent } from '@app/shared/common/approve-group/approve-custom-group/approve-custom-group.component';
import { ApproveGroupComponent } from '@app/shared/common/approve-group/approve-group/approve-group.component';
import { InputType, PrimeTableOption, TextAlign } from '@app/shared/core/controls/primeng-table/prime-table/primte-table.interface';
import { ToolbarComponent } from '@app/shared/core/controls/toolbar/toolbar.component';
import { WebConst } from '@app/shared/core/utils/consts/WebConsts';
import { APPROVE_GROUP_ENTITY, ApproveGroupServiceProxy, AttachFileServiceProxy, CM_ATTACH_FILE_ENTITY, CM_ATTACH_FILE_INPUT, DYNAMIC_PRIME_TABLE_ENTITY } from '@shared/service-proxies/service-proxies';
import { ChangeDetectionComponent } from './change-detection.component';

//REVIEW - Cần xem lại perform re-render
@Component({ template: '' })
export abstract class DefaultComponentBase extends ChangeDetectionComponent {

    @ViewChild('appToolbar') appToolbar: ToolbarComponent;
    @ViewChild('approveGroup') approveGroup: ApproveCustomGroupComponent;
    @ViewChild('approveCustomGroup') approveCustomGroup: ApproveCustomGroupComponent;
    @ViewChild('approveGroupComponent') approveGroupComponent: ApproveGroupComponent;

    private attachFileService: AttachFileServiceProxy;
    private approveGroupServiceProxy: ApproveGroupServiceProxy;

    constructor(injector: Injector) {
        super(injector);
        this.attachFileService = injector.get(AttachFileServiceProxy);
        this.ref = injector.get(ElementRef);
        this.approveGroupServiceProxy = injector.get(ApproveGroupServiceProxy);
    }

    rootPage() {
        return '/app/admin/dashboard';
    }

    getRemoteFile(url: string) {
        return this.attachFileService['baseUrl'] + '/' + url;
    }

    getFilterInputInRoute(getFilterInput): any {
        this.activeRoute.queryParams.subscribe(response => {
            var str = response['filterInput'];
            if (getFilterInput) {
                getFilterInput(str);
            }
        })
    }

    initFilter() {
        this.getFilterInputInRoute((filterJson) => {
            if (filterJson) {
                (this as any).filterInput = JSON.parse(filterJson);
            }
        });
    }

    getCurrentFunctionId() {
        let currentFunctionId = window.location.href;
        //REVIEW - Cần xem lại cách hoạt động
        currentFunctionId = currentFunctionId.substring(currentFunctionId.indexOf(WebConst.UrlPrefix));
        currentFunctionId = currentFunctionId.substring(currentFunctionId.indexOf(WebConst.MainUrlPrefix));
        if (currentFunctionId.endsWith('-add')) {
            currentFunctionId = currentFunctionId.substring(0, currentFunctionId.length - 4);
        }
        else if (currentFunctionId.indexOf('-edit;') > 0) {
            currentFunctionId = currentFunctionId.substring(0, currentFunctionId.indexOf('-edit;'));
        }
        else if (currentFunctionId.indexOf('-view;') > 0) {
            currentFunctionId = currentFunctionId.substring(0, currentFunctionId.indexOf('-view;'));
        }
        return currentFunctionId;
    }

    createFileModel(response: any[], typeToList: any): any[] {
        let childs: any[] = [];
        Object.keys(typeToList).forEach(fileType => {
            let items = response.filter(x => x['TYPE'] == fileType);
            let details = typeToList[fileType];
            for (let i = 0; i < items.length; i++) {

                let file = details[i]['filE_ATTACHMENT'];
                if (!file) {
                    file = new CM_ATTACH_FILE_ENTITY();
                    file.patH_OLD = '';
                    file.filE_NAME_OLD = '';
                    file.patH_NEW = '';
                    file.filE_NAME_NEW = '';
                }

                file.attacH_ID = details[i]['attacH_ID'];
                file.type = fileType;
                file.reF_ID = items[i].REF_ID;
                childs.push(file);
            }
        })

        return childs;
    }

    getFileMultiChildren(refMasterId: string, master: any, childs: any[], onGetFileSuccess = undefined) {
        setTimeout(() => {
            this.attachFileService.cM_ATTACH_FILE_By_RefMaster(refMasterId).subscribe(response => {
                var fileMaster = response.filter(x => x.reF_ID == refMasterId);
                if (fileMaster.length) {
                    master['filE_ATTACHMENT'] = fileMaster[0];
                    master['attacH_ID'] = fileMaster[0].attacH_ID;
                }

                if (childs) {

                    let idToChild = {};
                    childs.forEach(x => {
                        x.childs.forEach(y => {
                            idToChild[y[x.childIdName]] = y;
                        })
                    })

                    response.forEach(file => {
                        var child = idToChild[file.reF_ID];
                        if (child) {
                            child['filE_ATTACHMENT'] = file;
                            child['attacH_ID'] = child.attacH_ID;
                        }
                    })
                }

                if (onGetFileSuccess) {
                    onGetFileSuccess(response);
                }
            })
        })
    }

    getAllFiles(attFile: any): string[] {
        if (!attFile) {
            return [];
        }
        let results = [];

        let names = (attFile.filE_NAME_NEW || '').split('|');
        let paths = (attFile.patH_NEW || '').split('|');

        let m = Math.min(names.length, paths.length);

        for (let i = 0; i < m; i++) {
            results.push(paths[i] + '/' + names[i]);
        }

        return results;
    }

    getFile(refMasterId: string, master: any, childs: any[] = undefined, childIdName = undefined, onGetFileSuccess = undefined, masterFileAttachmentName = 'filE_ATTACHMENT', detailFileAttachmentName = 'filE_ATTACHMENT') {
        this.attachFileService.cM_ATTACH_FILE_By_RefMaster(refMasterId).subscribe(response => {
            master['olD_FILE_PATHS'] = [];
            response.forEach(x => {
                master['olD_FILE_PATHS'].push(...this.getAllFiles(x));
            })

            var fileMaster = response.filter(x => x.reF_ID == refMasterId);
            if (fileMaster.length) {
                master['filE_ATTACHMENT'] = fileMaster[0];
                master['attacH_ID'] = fileMaster[0].attacH_ID;
            }

            if (childs) {
                response.forEach(file => {
                    var child = childs.filter(x => x[childIdName] == file.reF_ID);
                    if (child.length > 0) {
                        child[0]['filE_ATTACHMENT'] = child[0][detailFileAttachmentName] = file;
                        child[0]['attacH_ID'] = child[0].attacH_ID;
                    }
                })
            }

            if (onGetFileSuccess) {
                onGetFileSuccess(response);
            }
        });
    }

    getFileByRefIds(items: any[], itemIdName, onGetFileSuccess: any = undefined) {
        if (items.length == 0) {
            return;
        }
        var refIds = items.map(x => x['itemIdName']);
        this.attachFileService.cM_ATTACH_FILE_By_RefId(refIds).subscribe(response => {
            response.forEach(file => {
                var child = items.filter(x => x[itemIdName] == file.reF_ID);
                if (child.length > 0) {
                    child[0]['filE_ATTACHMENT_OLD'] = child[0]['filE_ATTACHMENT'] = file.filE_NAME;
                    child[0]['attacH_ID'] = child[0].attacH_ID;
                }
            });

            if (onGetFileSuccess) {
                onGetFileSuccess(response);
            }
        });
    }

    excelMapping(x: any) {
        let r: any = { ...x };
        r.toJSON = function () {
            let data = {};
            let scope = this;
            Object.keys(this).filter(x => x != "toJSON").forEach(function (k) {
                if (k) {
                    data[k] = scope[k];
                }
            })
            return data;
        };
        return r;
    }

    updateFileMultiChildren(input: any, refMaster: string, type: string, childs: any[]) {

        let file = new CM_ATTACH_FILE_INPUT();
        file.attachFile = input['filE_ATTACHMENT'];

        let newFilePaths = [];

        if (!file.attachFile) {
            file.attachFile = new CM_ATTACH_FILE_ENTITY();

            file.attachFile.filE_NAME_NEW = '';
            file.attachFile.patH_NEW = '';
            file.attachFile.filE_NAME_OLD = '';
            file.attachFile.patH_OLD = '';
        }

        newFilePaths.push(...this.getAllFiles(input['filE_ATTACHMENT']));
        file.attachFile.type = type;
        file.attachFile.attacH_ID = input['attacH_ID'];

        if (childs) {
            if (childs.length > 0) {
                childs.forEach(x => {
                    newFilePaths.push(...this.getAllFiles(x));
                });
            }
        }

        file.childs = [];
        if (childs) {
            if (childs.length > 0) {
                file.ids = refMaster + ',' + childs.map(x => x.reF_ID).join(',');
            } else {
                file.ids = refMaster;
            }
        }
        else {
            file.ids = refMaster;
        }

        file.oldFiles = input['olD_FILE_PATHS'];
        file.newFiles = newFilePaths;
        file.childs = childs;

        this.attachFileService.moveTmpFile(file).subscribe(attachFile => {
            this.attachFileService.cM_ATTACH_FILE_Upd(attachFile).subscribe(response => {
                input['olD_FILE_PATHS'] = [];
                input['olD_FILE_PATHS'].push(...this.getAllFiles(attachFile.attachFile));
                input['filE_ATTACHMENT'] = attachFile.attachFile;

                if (childs) {
                    childs.forEach(x => {
                        x['filE_ATTACHMENT'] = attachFile.childs.firstOrDefault(a => a.attacH_ID == x['attacH_ID']);
                    })
                }
            });
        });
    }

    addFileMultiChildren(input: any, refMaster: string, type: string, childs: any[]) {
        let file = new CM_ATTACH_FILE_INPUT();
        file.attachFile = input['filE_ATTACHMENT'];
        let newFilePaths = [];
        newFilePaths.push(...this.getAllFiles(input['filE_ATTACHMENT']));

        if (!file.attachFile) {
            file.attachFile = new CM_ATTACH_FILE_ENTITY();

            file.attachFile.filE_NAME_NEW = '';
            file.attachFile.patH_NEW = '';
            file.attachFile.filE_NAME_OLD = '';
            file.attachFile.patH_OLD = '';
        }

        if (childs) {
            if (childs.length > 0) {
                childs.forEach(x => {
                    newFilePaths.push(...this.getAllFiles(x));
                });
            }
        }

        file.attachFile.type = type;
        file.childs = childs;

        if (childs) {
            if (childs.length > 0) {
                file.ids = refMaster + ',' + childs.map(x => x.reF_ID).join(',');
            } else {
                file.ids = refMaster;
            }
        }
        else {
            file.ids = refMaster;
        }

        this.attachFileService.moveTmpFile(file).subscribe(fileNew => {
            this.attachFileService.cM_ATTACH_FILE_Ins(fileNew).subscribe(response => {
            });
        })
    }

    addFile(input: any, type: string, childs: any[], refIds, childType: string = undefined) {
        let file = new CM_ATTACH_FILE_INPUT();
        file.attachFile = input['filE_ATTACHMENT'];

        if (!file.attachFile) {
            file.attachFile = new CM_ATTACH_FILE_ENTITY();

            file.attachFile.filE_NAME_NEW = '';
            file.attachFile.patH_NEW = '';
            file.attachFile.filE_NAME_OLD = '';
            file.attachFile.patH_OLD = '';
        }

        file.attachFile.type = type;
        file.childs = [];
        file.ids = refIds;

        if (childs) {
            childs.forEach(x => {
                let child = x['filE_ATTACHMENT'];
                if (!child) {
                    child = new CM_ATTACH_FILE_ENTITY();
                    child.filE_NAME_NEW = '';
                    child.patH_NEW = '';
                    child.filE_NAME_OLD = '';
                    child.patH_OLD = '';
                }
                child.attacH_ID = x['attacH_ID'];
                child.type = childType;
                file.childs.push(child);
            })
        }

        this.attachFileService.moveTmpFile(file).subscribe(fileNew => {
            this.attachFileService.cM_ATTACH_FILE_Ins(fileNew).subscribe(response => { });
        })
    }

    updateFile(input: any, type: string, childs: any[], refIds, childType: string = undefined) {
        let file = new CM_ATTACH_FILE_INPUT();
        file.attachFile = input['filE_ATTACHMENT'];

        console.log(file.attachFile);

        let newFilePaths = [];

        if (!file.attachFile) {
            file.attachFile = new CM_ATTACH_FILE_ENTITY();

            file.attachFile.filE_NAME_NEW = '';
            file.attachFile.patH_NEW = '';
            file.attachFile.filE_NAME_OLD = '';
            file.attachFile.patH_OLD = '';
        }

        newFilePaths.push(...this.getAllFiles(input['filE_ATTACHMENT']));
        file.attachFile.type = type;
        file.attachFile.attacH_ID = input['attacH_ID'];

        if (childs) {
            childs.forEach(x => {
                newFilePaths.push(...this.getAllFiles(x['filE_ATTACHMENT']));
            })
        }

        file.childs = [];
        file.ids = refIds;

        file.oldFiles = input['olD_FILE_PATHS'];
        file.newFiles = newFilePaths;

        if (childs) {
            childs.forEach(x => {
                let child = x['filE_ATTACHMENT'];
                if (!child) {
                    child = new CM_ATTACH_FILE_ENTITY();
                    child.filE_NAME_NEW = '';
                    child.patH_NEW = '';
                    child.filE_NAME_OLD = '';
                    child.patH_OLD = '';
                }
                child.attacH_ID = x['attacH_ID'];
                child.type = childType;
                file.childs.push(child);
            })
        }

        this.attachFileService.moveTmpFile(file).subscribe(attachFile => {
            this.attachFileService.cM_ATTACH_FILE_Upd(attachFile).subscribe(response => {
                input['olD_FILE_PATHS'] = [];
                // init old file
                input['olD_FILE_PATHS'].push(...this.getAllFiles(attachFile.attachFile));
                input['filE_ATTACHMENT'] = attachFile.attachFile;

                if (childs) {
                    childs.forEach(x => {
                        x['filE_ATTACHMENT'] = attachFile.childs.firstOrDefault(a => a.attacH_ID == x['attacH_ID']);
                    })
                }
            });
        })
    }

    showErrorMessage(message: string) {
        this.notify.error(this.l(message));
    }

    addNewSuccess() {
        this.notify.info(this.l('InsertSuccessfully'));
    }

    updateSuccess() {
        this.notify.info(this.l('UpdateSuccessfully'));
    }

    approveSuccess() {
        this.notify.info(this.l('ApproveSuccessfully'));
    }

    onChangeProperty(propertyName) {
        this['editForm'].controls[propertyName]?.setValue(this['inputModel'][propertyName]);
    }

    // set saving(value: boolean) {
    //     value ? this.hideLoading() : this.showLoading
    // }

    formatGetTwoDecimalPlaces(input: number): number {
        if (input > 0) {
            let result: number = 0;
            result = parseFloat(input.toFixed(2));
            return result;
        }
        else return 0;
    }

    // 11/04/2023
    addApproveGroup(input: any, reqId: string) {
        let approveGroup = new APPROVE_GROUP_ENTITY();
        approveGroup.reQ_ID = reqId;

        // if (this.approveGroup)
        //     approveGroup.grouP_APPROVE = this.approveGroup.getGroups();
        // else if (this.approveCustomGroup)
        //     approveGroup.grouP_APPROVE = this.approveCustomGroup.getGroups();

        approveGroup.title = input['TITLE'];
        this.approveGroupServiceProxy.cM_APPROVE_GROUP_NEW_Ins(approveGroup).subscribe(response => {
            if (response.result != '0') {
                this.showErrorMessageApprove(response.errorDesc);
            } else {
                this.removeMessageApprove();
            }
        });
    }

    updApproveGroup(input: any, reqId: string) {
        let approveGroup = new APPROVE_GROUP_ENTITY();
        approveGroup.reQ_ID = reqId;

        if (this.approveGroup)
            approveGroup.grouP_APPROVE = this.approveGroup.getGroups();
        else if (this.approveCustomGroup)
            approveGroup.grouP_APPROVE = this.approveCustomGroup.getGroups();

        approveGroup.title = input['TITLE'];
        this.approveGroupServiceProxy.cM_APPROVE_GROUP_NEW_Upd(approveGroup).subscribe(response => {
            if (response.result != '0') {
                this.showErrorMessageApprove(response.errorDesc);
            } else {
                this.removeMessageApprove();
            }
        });
    }

    getApproveGroup(input: any, reqId: string) {
        this.approveGroupServiceProxy.cM_APPROVE_GROUP_BY_TITLE_ID(reqId).subscribe(response => {
            console.log("CM_APPROVE_GROUP_BY_TITLE_ID RESPONSE:", response);
            input['TITLE'] = [];
            input['TITLE'].push(...response);
            this.updateView();
        });
        this.approveGroupServiceProxy.cM_APPROVE_GROUP_NEW_ById(reqId).subscribe(response => {
            console.log("cM_APPROVE_GROUP_NEW_ById response:", response);
            input['approvE_GROUP'] = [];
            input['approvE_GROUP'].push(...response);
            this.updateView();
        });
    }

    getApproveGroupView(input: any, reqId: string) {
        this.approveGroupServiceProxy.cM_APPROVE_GROUP_WORKFLOW_ById(reqId).subscribe(response => {
            input['approvE_GROUP_VIEW'] = [...response];
        });
    }

    clone(object: any): any {
        return { ...object }
    }

    set saving(value: boolean) {
        if (value) abp.ui.setBusy();
        if (!value) abp.ui.clearBusy();

        if (this.appToolbar) this.appToolbar.setEnable(!value);
    }

    //TODO - Khởi tạo các options cho PrimeTable
    handleGenerateOptions(item: DYNAMIC_PRIME_TABLE_ENTITY[]): PrimeTableOption<any>[] {
        const options = []
        item.forEach(_item => {
            const table: PrimeTableOption<any> = {
                columns: _item.columns.map(column => {
                    const inAlign = Object.keys(TextAlign).indexOf(column.align)
                    const inInputType = Object.keys(InputType).indexOf(column.inputType)
                    return ({
                        ...column,
                        inputType: inInputType >= 0 && Object.values(InputType)[inInputType],
                        align: inAlign >= 0 && Object.values(TextAlign)[inAlign],
                        onOpenModal: () => {
                            if (column.selectorModal) this[column.selectorModal].show();
                        }
                    })
                }),
                config: _item.config
            }
            options.push(table)
        })
        return options;
    }
}
