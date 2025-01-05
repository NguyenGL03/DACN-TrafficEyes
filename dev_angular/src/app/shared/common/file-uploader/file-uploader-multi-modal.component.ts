import { AfterViewInit, Component, EventEmitter, Inject, Injector, Input, OnInit, Optional, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { ChangeDetectionComponent } from '@app/utilities/change-detection.component';
import { API_BASE_URL, CM_ATTACH_FILE_ENTITY, UltilityServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { PopupFrameComponent } from '../modals/popup-frames/popup-frame.component';
import { FileUploaderComponent } from './file-uploader.component';

@Component({
    selector: "file-multi-modal",
    templateUrl: "./file-uploader-multi-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class FileUploaderMultiModalComponent extends ChangeDetectionComponent implements OnInit, AfterViewInit {
    @ViewChild('popupFrame') modal: PopupFrameComponent;

    @Input() disabled: boolean;
    @Input() folderUpload: string;
    @Input() errors: string = '';
    @Input() isBBBG: boolean = false; // Cho chọn file ở BBBG giao nhận tài sản và không xoá file từ người tạo (truyền true) (Xuất SD TS, DCTS, Thu hồi TS)
    @Input() disabledUploadFile: boolean = false; // Cho chọn file ở BBBG giao nhận tài sản và không xoá file từ người tạo (truyền true) (Xuất SD TS, DCTS, Thu hồi TS)

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSaveFile: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    listFile: CM_ATTACH_FILE_ENTITY[];
    ngModel: CM_ATTACH_FILE_ENTITY;
    saving: boolean;
    fileUploader: FileUploaderComponent;
    baseUrl: string;

    constructor(
        injector: Injector,
        private ultilityService: UltilityServiceProxy,
        private fileDownloadService: FileDownloadService,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector)
        this.baseUrl = baseUrl;
    }

    ngOnInit(): void {

    }

    ngAfterViewInit(): void {

    }

    selectedFiles() {
    }

    show(fileUploader: FileUploaderComponent): void {
        this.fileUploader = fileUploader;
        const multiFilePath = fileUploader.ngModel;

        if (multiFilePath && multiFilePath.filE_NAME_OLD.trim()) {
            let fileOlds = fileUploader.ngModel.filE_NAME_OLD.split('|');
            let fileNames = fileUploader.ngModel.filE_NAME_NEW.split('|');
            let paths = fileUploader.ngModel.patH_NEW.split('|');
            let isDeleteFiles = fileUploader.ngModel['isDeleteFile'] ? fileUploader.ngModel['isDeleteFile'].split('|') : null
            let m = Math.min(fileOlds.length, fileNames.length);
            m = Math.min(m, paths.length);
            this.listFile = [];
            for (let i = 0; i < m; i++) {
                let item = new CM_ATTACH_FILE_ENTITY();
                item.filE_NAME_NEW = fileNames[i];
                item.filE_NAME_OLD = fileOlds[i];
                item.patH_NEW = paths[i];
                if (isDeleteFiles) {
                    item['isDeleteFile'] = (isDeleteFiles[i] === 'true' && !this.disabledUploadFile)
                }

                this.listFile.push(item);
            }
        } else {
            this.listFile = [];
        }

        this.modal.show();
    }

    showFilePicker() {
        $(this.fileUploader.fileControl.nativeElement).click();
    }

    uploadComplete(e: any, i: any) {
        let result: CM_ATTACH_FILE_ENTITY = e.body.result.cM_ATTACH_FILE_ENTITY;

        if (!this.listFile) this.ngModel = result;

        let paths = (result.patH_NEW || '').split('|');
        let names = (result.filE_NAME_NEW || '').split('|');
        let fileNameOlds = (result.filE_NAME_OLD || '').split('|');
        let m = Math.min(paths.length, names.length, fileNameOlds.length);
        for (let i = 0; i < m; i++) {
            if (this.listFile.filter(x => x.patH_NEW + x.filE_NAME_NEW == paths[i] + '/' + names[i]).length == 0) {
                let file = new CM_ATTACH_FILE_ENTITY();
                file.patH_NEW = paths[i];
                file.filE_NAME_NEW = names[i];
                file.filE_NAME_OLD = fileNameOlds[i];
                if (this.isBBBG) {
                    file['isDeleteFile'] = "true"
                }
                this.listFile.push(file);
            }
        }

    }

    save() {
        this.showLoading();
        let file = new CM_ATTACH_FILE_ENTITY();

        if (this.listFile && this.listFile.length) {
            file.patH_NEW = this.listFile.map(x => x.patH_NEW).join('|');
            file.filE_NAME_NEW = this.listFile.map(x => x.filE_NAME_NEW).join('|');
            file.filE_NAME_OLD = this.listFile.map(x => x.filE_NAME_OLD).join('|');
        } else {
            file.patH_NEW = '';
            file.filE_NAME_NEW = '';
            file.filE_NAME_OLD = '';
        }

        this.fileUploader.ngModel = file;
        this.onSaveFile.emit();
        this.close();
    }

    close() {
        this.active = false;
        this.hideLoading();
        this.modal.close();
    }

    download(file: CM_ATTACH_FILE_ENTITY) {
        this.fileUploader.downloadFile(file);
    }

    deleteFile(filePath: CM_ATTACH_FILE_ENTITY) {
        this.listFile = this.listFile.filter(e => e !== filePath);
    }

    downloadFile(file: CM_ATTACH_FILE_ENTITY = undefined) {

        if (!file) file = this.ngModel;

        if (file) {
            this.ultilityService.downloadFile(file.patH_NEW + '/' + file.filE_NAME_NEW).subscribe(f => {
                f.fileName = file.filE_NAME_OLD;
                this.fileDownloadService.downloadTempFile(f);
            })
        }
    }
}