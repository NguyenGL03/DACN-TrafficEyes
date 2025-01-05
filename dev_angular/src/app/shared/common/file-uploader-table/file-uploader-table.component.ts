import { HttpClient, HttpEventType, HttpHeaders, HttpRequest, HttpResponse } from "@angular/common/http";
import { Component, ElementRef, Inject, Injector, Input, OnDestroy, OnInit, Optional, Output, ViewChild, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ControlComponent } from "@app/shared/core/controls/control.component";
import { API_BASE_URL, AttachFileServiceProxy, CM_ATTACH_FILE_ENTITY, UltilityServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { FileUploaderTableModalComponent } from "./file-uploader-table-modal.component";
 

@Component({
    moduleId: module.id,
    selector: "file-uploader-table",
    templateUrl: "./file-uploader-table.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => FileUploaderTableComponent),
        multi: true
    }]
})
export class FileUploaderTableComponent extends ControlComponent implements OnInit, OnDestroy {

    writeValue(value: any): void {
        this._ngModel = value;
    }

    //Note : file đính kèm nằm trên lưới thì xài component này
    injector: Injector;
    private ultilityService: UltilityServiceProxy;
    private fileDownloadService: FileDownloadService;
    private attachFileService: AttachFileServiceProxy;
    constructor(injector: Injector, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(injector);
        this.linkServer = baseUrl + this.linkServer;
        this.idProgress = 'f' + this.generateUUID();
        this.injector = injector;
        this.ultilityService = this.injector.get(UltilityServiceProxy);
        this.fileDownloadService = this.injector.get(FileDownloadService);
        this.fileName = '';
    }

    httpClient: HttpClient;
    idProgress: string;
    linkServer: string = '/RoxyFileman/UPLOAD_W_T';

    @Input() multiFile: boolean = true;

    @Input() disabled: boolean;
    @Input() disabledShowFile: boolean = true;

    @Input() inpCss: string = 'form-control';
    @Input() accept: string;

    @ViewChild('fileControl') fileControl: ElementRef;
    @ViewChild('loading') loading: ElementRef;
    @ViewChild('fileMultiEditable') fileMultiModal: FileUploaderTableModalComponent;
    @ViewChild('control1') control1: ElementRef;
    //static fileUploaderMultiModal: FileUploaderMultiModalComponent;

    static g: string;
    errorMessage: string = '';

    loadingProgress: any;
    showFile() {
        this.fileMultiModal.show(this);
    }
    afterViewInit() {
        // 
        // setTimeout(() => {
        //     this.loadingProgress = new ProgressBar.Circle(this.loading.nativeElement, {
        //         strokeWidth: 6,
        //         easing: 'easeInOut',
        //         duration: 1400,
        //         color: '#639bb7',
        //         trailColor: '#eee',
        //         trailWidth: 1,
        //         svgStyle: null
        //     });
        // }, 1000)
    }

    fileName: string;

    _ngModel: CM_ATTACH_FILE_ENTITY;
    public get ngModel() {
        return this._ngModel;
    }
    @Input() @Output() public set ngModel(value) {
        if (value) {
            this.fileName = value.filE_NAME_OLD;
        }
        else {
            this.fileName = '';
        }
        this._ngModel = value;
        // this.control1.nativeElement.value = this.fileName;
    }
    @Input() folderUpload: string;

    ngOnInit(): void {

    }

    deleteFile() {
        this.ngModel = undefined;
        this.fileName = '';
        this.control1.nativeElement.value = this.fileName;

    }

    beforeUnload($event) {

    }

    downloadFile(file: CM_ATTACH_FILE_ENTITY = undefined) {

        if (!file) {
            file = this.ngModel;
        }

        if ((this.multiFile && file == this.ngModel) || !file.filE_NAME_OLD) {
            return;
        }

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

    async uploadFile(evt) {
        if (!evt.target.value) {
            return;
        }
        await this.fileUpload(undefined);
    }

    getCookie(name) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + name + "=");
        if (parts.length == 2) return parts.pop().split(";").shift();
    }

    updateUploadProgress(evt, i) {
        if (evt.lengthComputable) {
            var percentComplete = evt.loaded / evt.total;
            this.loadingProgress.animate(percentComplete);
        }
        console.log('updateUploadProgress')
    }

    uploadComplete(e, i) {
        if (this.multiFile) {
            this.fileMultiModal.uploadComplete(e, i);
            return;
        }
        this.ngModel = e.body.result.cM_ATTACH_FILE_ENTITY;
        this.ngModel['toJSON'] = this.toJSON;
        this.hideLoading();

    }

    uploadError(e, i) {
        this.hideLoading();
        console.log('uploadError')
    }

    uploadCanceled(e, i) {
        this.hideLoading();
        console.log('uploadCanceled')
    }

    showLoading() {
        // $(this.loading.nativeElement).show();
    }

    hideLoading() {
        this.loadingProgress.set(0);
        // $(this.loading.nativeElement).hide();
    }


    progressbar: any;

    deleteFiles(fileNames: string[]) {
        this.ultilityService.dEL_F(fileNames).subscribe(() => {

        })
    }
    isShowError: boolean = false;
    errors: string;
    async fileUpload(i) {
        this.httpClient = this.injector.get(HttpClient);
        let scope = this;

        let el = scope.fileControl.nativeElement;
        var MaxSize = 500;
        // var MaxSize = Number.parseInt(this.s("gAMSProCore.FileSizeAttach"));
        // var MaxlenghtName = Number.parseInt(this.s("gAMSProCore.MaxFilenameLength"));
        // var Maxfiles = Number.parseInt(this.s("gAMSProCore.NumberOfFiles"));
        let tokenAuth = scope.getCookie('Abp.AuthToken');

        let headers = { 'Authorization': 'Bearer ' + tokenAuth, 'Accept': 'application/json, text/plain, */*', 'Access-Encoding': 'gzip, deflate', 'Access-Control-Allow-Origin': '*', 'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS', 'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token' };


        const endpoint = scope.linkServer;

        let fData = new FormData();
        fData.append("action", 'upload');
        fData.append("method", 'ajax');
        fData.append("d", scope.folderUpload);
        this.errors = '';

        for (var inn = 0; inn < el.files.length; inn++) {
            console.log(el.files);
            console.log(el.files[inn].name.split('.').pop());
            //BAODNQ 27/10/2022 : Chuyển toàn bộ đuôi những file upload lên thành chữ thường
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

            //let sub = this.httpClient.request(req).toPromise();

            // let event = await sub;

            // scope.uploadComplete(event, undefined);

            this.httpClient.request(req).subscribe(event => {
                if (event.type === HttpEventType.UploadProgress) {
                    console.log(event);
                    const percentDone = Math.round(event.loaded * 10 / event.total) / 10;
                    console.log(`File is ${percentDone}% uploaded.`);

                    // if (!this.progressbar) {
                    //     this.progressbar = new ProgressBar.Circle('#' + this.idProgress, {
                    //         strokeWidth: 6,
                    //         easing: 'easeInOut',
                    //         duration: 1400,
                    //         color: '#639bb7',
                    //         trailColor: '#eee',
                    //         trailWidth: 1,
                    //         svgStyle: null
                    //     });
                    // }
                    // try {
                    //     this.progressbar.animate(percentDone);
                    // }
                    // catch {

                    // }

                } else if (event instanceof HttpResponse) {
                    scope.uploadComplete(event, undefined);
                    // sub.unsubscribe();
                }
            })
            // console.log(event);


            // 

        } catch (err) {
            console.log(err);
        }

        //    .post(endpoint, fData, { headers: headers, reportProgress: true, });
        //   .catch((e) => this.handleError(e));

        // Object.keys(headers).forEach(k => {
        //     http.setRequestHeader(k, headers[k]);
        // })
        // http.setRequestHeader("Accept", "*/*");
        //   http.setRequestHeader("Authorization", 'Bearer ' + tokenAuth);

        //http.fetch(fData);
        //    http.send(fData);

    }

    // getMultiFile


    getMultiFile(event) {
        var $scope = this;
        $scope.ngModel = event;
    }

    ngOnDestroy(): void {
        console.log('file picker destroy');

    }
}