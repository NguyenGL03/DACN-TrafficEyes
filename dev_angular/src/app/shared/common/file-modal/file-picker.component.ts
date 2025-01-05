import { Component, Injector, Input, OnInit, Output, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";

import { UltilityServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { ControlComponent } from "../../core/controls/control.component";

declare var $: JQueryStatic;


@Component({
    selector: "file-modal",
    templateUrl: "./file-picker.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => FilePickerComponent),
        multi: true
    }]
})
export class FilePickerComponent extends ControlComponent implements OnInit {

    _ngModel: string;

    @Input() disabled: boolean;

    @Input() multiFile: boolean = true;

    @Input() accept: any;

    public get ngModel(): any {

        return this._ngModel;
    }

    @Input() @Output() public set ngModel(value) {
        this._ngModel = value;
    }

    @Input() inpCss: string;
    @Input() folderUpload: string;
    @Input() disabledUploadFile: boolean = false; //Cho chọn file ở BBBG giao nhận tài sản và không xoá file từ người tạo (truyền true) (Xuất SD TS, DCTS, Thu hồi TS)
    constructor(
        injector: Injector,
        private ultilityService: UltilityServiceProxy,
        private fileDownloadService: FileDownloadService,
    ) {
        super(injector)
    }

    ngOnInit(): void {
        if (!$('#roxyCustomPanel').length) {
            $('body').append(`<div id="roxyCustomPanel" style="display: none;">
            <iframe src="/assets/fileman/index.html?integration=custom" id="roxy-fileman" style="width:100%;height:100%" frameborder="0"></iframe>
        </div>`);
            ($('#roxyCustomPanel') as any).dialog().dialog('close');
        }
    }

    writeValue(value: any) {
        this._ngModel = value;
    }

    importFileFromServer() {

        ($('#roxyCustomPanel>iframe#roxy-fileman') as any).attr('folder-upload', this.folderUpload);

        var windowParent: any = window.parent;
        windowParent.refreshFilemanWindow(this.multiFile);

        ($('#roxyCustomPanel') as any).dialog({ modal: true, width: 875, height: 600 });
        var $scope = this;


        windowParent.onSelectCustomRoxy = function (filePath: string) {
            if (filePath.startsWith('/')) {
                filePath = filePath.substr(1);
            }
            if (filePath.startsWith('\\')) {
                filePath = filePath.substr(1);
            }
            $scope.ngModel = filePath;
            ($('#roxyCustomPanel') as any).dialog('close');
        };

        windowParent.onSelectCustomRoxyMulti = function (filePaths: string) {

            let paths = [];
            for (let filePath of filePaths) {
                if (filePath.startsWith('/')) {
                    filePath = filePath.substr(1);
                }
                if (filePath.startsWith('\\')) {
                    filePath = filePath.substr(1);
                }

                paths.push(filePath);
            }
            var value = paths.join(',');
            $scope.ngModel = value;
            ($('#roxyCustomPanel') as any).dialog('close');
        };
    }
    getMultiFile(event) {
        var $scope = this;
        $scope.ngModel = event;
    }
    deleteFile() {
        var $scope = this;
        $scope.ngModel = undefined;

    }
    downloadFile() {
        if (!this.multiFile) {
            if (this.ngModel) {
                this.ultilityService.downloadFile(this.ngModel).subscribe(file => {
                    this.fileDownloadService.downloadTempFile(file);
                })
            }
        }
    }
}
