import { HttpClient, HttpEventType, HttpHeaders, HttpRequest, HttpResponse } from "@angular/common/http";
import { Component, ElementRef, EventEmitter, Inject, Injector, Input, OnDestroy, OnInit, Optional, Output, ViewChild, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ControlComponent } from "@app/shared/core/controls/control.component";
import { API_BASE_URL, CM_ATTACH_FILE_ENTITY, UltilityServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { FileUploaderMultiModalComponent } from "./file-uploader-multi-modal.component";

@Component({
    moduleId: module.id,
    selector: "file-uploader",
    templateUrl: "./file-uploader.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => FileUploaderComponent),
        multi: true
    }]
})

export class FileUploaderComponent extends ControlComponent implements OnInit, OnDestroy {
    private ultilityService: UltilityServiceProxy;
    private fileDownloadService: FileDownloadService;

    constructor(injector: Injector, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(injector);
        this.injector = injector;
        this.linkServer = baseUrl + this.linkServer;
        this.idProgress = 'f' + this.generateUUID();
        this.ultilityService = this.injector.get(UltilityServiceProxy);
        this.fileDownloadService = this.injector.get(FileDownloadService);
        this.fileName = '';

        if ($('file-multi-modal').length == 0) {
            FileUploaderComponent.fileUploaderMultiModal = undefined;
        }

        if (FileUploaderComponent.fileUploaderMultiModal) {
            this.showMultiFile = false;
        }
    }

    static fileUploaderMultiModal: FileUploaderMultiModalComponent;
    static g: string;

    fileName: string;
    progressbar: any;
    isShowError: boolean = false;
    errors: string;

    httpClient: HttpClient;
    idProgress: string;
    linkServer: string = '/RoxyFileman/UPLOAD_W_T';
    errorMessage: string = '';
    showMultiFile: boolean = true;
    _ngModel: CM_ATTACH_FILE_ENTITY;
    loadingProgress: any;

    @Input() folderUpload: string;
    @Input() multiFile: boolean = true;
    @Input() disabled: boolean;
    @Input() accept: string = '.xlsx,.xls,.doc,.docx,.pdf,.rar,.jpeg,.png';
    @Input() isBBBG: boolean = false; // Cho chọn file ở BBBG giao nhận tài sản và không xoá file từ người tạo (truyền true) (Xuất SD TS, DCTS, Thu hồi TS)
    @Input() disabledUploadFile: boolean = false; // Cho chọn file ở BBBG giao nhận tài sản và không xoá file từ người tạo (truyền true) (Xuất SD TS, DCTS, Thu hồi TS)
    @Output() onSaveFile: EventEmitter<any> = new EventEmitter<any>();
    @Output() docToHTML: EventEmitter<any> = new EventEmitter<any>();
    @Output() onClickShowFile: EventEmitter<any> = new EventEmitter<any>();
    @Output() onUploadComplete: EventEmitter<any> = new EventEmitter<any>();
    @Output() onUploadAlternative: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('fileControl') fileControl: ElementRef;
    @ViewChild('filePathControl') filePathControl: ElementRef;
    @ViewChild('loading') loading: ElementRef;
    @ViewChild('fileMultiModal') fileMultiModal: FileUploaderMultiModalComponent;
    @Input() inputModel: any;
    @Input() alternativeUpload: boolean = false;

    @Input() @Output()
    public set ngModel(value) {
        this.fileName = value ? value.filE_NAME_OLD : '';
        this._ngModel = value;
        if (this.inputModel) this.inputModel['filE_ATTACHMENT'] = value;
    }

    public get ngModel() {
        return this._ngModel;
    }


    writeValue(value: any) {
        this.ngModel = value;
    }

    showFile() {
        this.onClickShowFile.emit();
        this.fileMultiModal.show(this);
    }

    afterViewInit() {
        if (!FileUploaderComponent.fileUploaderMultiModal) {
            FileUploaderComponent.fileUploaderMultiModal = this.fileMultiModal;
        }
        else {
            this.fileMultiModal = FileUploaderComponent.fileUploaderMultiModal;
        }
    }


    ngOnInit(): void {

    }

    deleteFile() {
        this.ngModel = undefined;
        this.fileName = '';
    }

    downloadFile(file: CM_ATTACH_FILE_ENTITY = undefined) {

        if (!file) file = this.ngModel;

        if ((this.multiFile && file == this.ngModel) || !file.filE_NAME_OLD) return;


        if (file) {
            this.ultilityService.downloadFile(file.patH_NEW + '/' + file.filE_NAME_NEW).subscribe(f => {
                f.fileName = file.filE_NAME_OLD;
                this.fileDownloadService.downloadTempFile(f);
            })
        }
    }

    importFileFromServer() {
        this.fileControl.nativeElement.click();
    }

    async uploadFile(event: any) {
        if (!event.target.value) return;
        if (this.alternativeUpload) {
            this.onUploadAlternative.emit(event);
            return;
        }
        await this.fileUpload();
    }

    getCookie(name: string) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + name + "=");
        if (parts.length == 2) return parts.pop().split(";").shift();
    }

    updateUploadProgress(event: any, i) {
        if (event.lengthComputable) {
            const percentComplete = event.loaded / event.total;
            this.loadingProgress.animate(percentComplete);
        }
    }

    uploadComplete(e: any, i) {
        if (this.multiFile) {
            this.fileMultiModal.uploadComplete(e, i);
            this.onUploadComplete.emit(this.ngModel);
            return;
        }
        this.ngModel = e.body.result.cM_ATTACH_FILE_ENTITY;
        this.ngModel['toJSON'] = this.toJSON;
        this.onUploadComplete.emit(this.ngModel);
        this.hideLoading();
    }

    deleteFiles(fileNames: string[]) {
        this.ultilityService.dEL_F(fileNames).subscribe(() => {

        })
    }

    async fileUpload() {
        this.httpClient = this.injector.get(HttpClient);
        let scope = this;

        let el = scope.fileControl.nativeElement;
        var MaxSize = Number.parseInt(this.s("gAMSProCore.FileSizeAttach"));
        let tokenAuth = scope.getCookie('Abp.AuthToken');

        let headers = { 'Authorization': 'Bearer ' + tokenAuth, 'Accept': 'application/json, text/plain, */*', 'Access-Encoding': 'gzip, deflate', 'Access-Control-Allow-Origin': '*', 'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS', 'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token' };

        const endpoint = scope.linkServer;

        let fData = new FormData();
        fData.append("action", 'upload');
        fData.append("method", 'ajax');
        fData.append("d", scope.folderUpload);
        this.errors = '';
        for (var inn = 0; inn < el.files.length; inn++) {
            // Chuyển toàn bộ đuôi những file upload lên thành chữ thường
            let fileExtension = el.files[inn].name.split('.').pop().toLowerCase();
            if (!this.accept.includes(fileExtension)) {
                this.isShowError = true;
                this.errors = this.l('FileInvalid');
                return;
            }

            if (el.files[inn].size > (MaxSize * 1024)) {
                this.isShowError = true;
                this.errors = this.l('MaxFileLengthInvalid');
                return;
            }
        }
        for (var inn = 0; inn < el.files.length; inn++) {
            fData.append("files[]", el.files[inn]);
        }

        let h = new HttpHeaders();

        Object.keys(headers).forEach(k => h.append(k, headers[k]))

        try {
            const req = new HttpRequest('POST', endpoint, fData, { headers: h, reportProgress: true, });
            this.httpClient.request(req).subscribe(event => {
                if (event.type === HttpEventType.UploadProgress) {
                    const percentDone = Math.round(event.loaded * 10 / event.total) / 10;
                    console.log(`File is ${percentDone}% uploaded.`);
                } else if (event instanceof HttpResponse) {
                    scope.uploadComplete(event, undefined);
                }
            })
        } catch (err) {
            console.log(err);
        }

    }

    async fileUploadWord(el: any) {
        this.httpClient = this.injector.get(HttpClient);
        let scope = this;
        var MaxSize = Number.parseInt(this.s("gAMSProCore.FileSizeAttach"));
        let tokenAuth = scope.getCookie('Abp.AuthToken');
        let headers = { 'Authorization': 'Bearer ' + tokenAuth, 'Accept': 'application/json, text/plain, */*', 'Access-Encoding': 'gzip, deflate', 'Access-Control-Allow-Origin': '*', 'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS', 'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token' };
        const endpoint = scope.linkServer + "_HTML";
        let fData = new FormData();
        fData.append("action", 'upload');
        fData.append("method", 'ajax');
        fData.append("d", scope.folderUpload);
        this.errors = '';
        for (var inn = 0; inn < el.files.length; inn++) {
            // Chuyển toàn bộ đuôi những file upload lên thành chữ thường
            let fileExtension = el.files[inn].name.split('.').pop().toLowerCase();
            if (!this.accept.includes(fileExtension)) {
                this.isShowError = true;
                this.errors = this.l('FileInvalid');

                return;
            }

            if (el.files[inn].size > (MaxSize * 1024)) {
                this.isShowError = true;
                this.errors = this.l('MaxFileLengthInvalid');

                return;
            }
        }
        for (var inn = 0; inn < el.files.length; inn++) {
            fData.append("files[]", el.files[inn]);
        }

        let h = new HttpHeaders();

        Object.keys(headers).forEach(k => {
            h.append(k, headers[k]);
        })

        try {
            const req = new HttpRequest('POST', endpoint, fData, { headers: h, reportProgress: true, });
            this.httpClient.request(req).subscribe(event => {
                if (event.type === HttpEventType.Response) {
                    this.docToHTML.emit(event.body)
                }
            })
        } catch (err) {
            console.log(err);
        }
    }

    getMultiFile(event) {
        var $scope = this;
        $scope.ngModel = event;
    }
}