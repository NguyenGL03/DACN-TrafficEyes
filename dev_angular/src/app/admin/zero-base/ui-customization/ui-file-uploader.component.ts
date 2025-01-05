import { HttpClient, HttpEventType, HttpHeaders, HttpRequest } from "@angular/common/http";
import { Component, ElementRef, Injector, Input, OnDestroy, OnInit, TemplateRef, ViewChild, ViewEncapsulation, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ControlComponent } from "@app/shared/core/controls/control.component";
import { AppConsts } from "@shared/AppConsts";

@Component({
    moduleId: module.id,
    selector: "ui-custom-file-uploader",
    templateUrl: "./ui-file-uploader.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => UiFileUploaderImageComponent),
        multi: true
    }]
})

export class UiFileUploaderImageComponent extends ControlComponent implements OnInit, OnDestroy {
    @Input() settingName: string;
    @Input() remoteServiceBaseUrl: string;
    @Input() fileName: string;
    @Input() previewImage: TemplateRef<any>;
    @ViewChild('fileControl') fileControl: ElementRef;

    ngModel: any;
    percentLoading: number = 0;

    constructor(injector: Injector, private httpClient: HttpClient) {
        super(injector);
    }

    writeValue(value: any): void {
        this.ngModel = value;
    }

    ngOnInit(): void {

    }
 
    importFileFromServer() {
        this.fileControl.nativeElement.click();
    }

    async uploadFile(event: any) {  

        let logoFile = this.s(this.settingName);
        const fileName = logoFile.substring(logoFile.lastIndexOf('/') + 1);
        const filePath = logoFile.substring(0, logoFile.lastIndexOf('/'));

        let scope = this;

        let tokenAuth = scope.getCookie('Abp.AuthToken');

        let headers = { 'Authorization': 'Bearer ' + tokenAuth, 'Accept': 'application/json, text/plain, */*', 'Access-Encoding': 'gzip, deflate', 'Access-Control-Allow-Origin': '*', 'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS', 'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token' };

        const endpoint = this.remoteServiceBaseUrl + '/api/Ultility/UploadLogo';

        let fData = new FormData();
        fData.append("action", 'upload');
        fData.append("method", 'ajax');
        fData.append("d", filePath);
        //FIXME - Need review
        fData.append("g", filePath);
        fData.append("fileName", fileName);
        fData.append("settingName", this.settingName);

        const el = this.fileControl.nativeElement;
        for (var inn = 0; inn < el.files.length; inn++) {
            fData.append("files[]", el.files[inn]);
        }

        const header = new HttpHeaders();

        Object.keys(headers).forEach(k => {
            header.append(k, headers[k]);
        })

        try {
            const request = new HttpRequest('POST', endpoint, fData, { headers: header, reportProgress: true, });

            this.httpClient.request(request).subscribe(event => {
                if (event.type === HttpEventType.UploadProgress) {
                    this.percentLoading = Math.round(event.loaded / event.total * 100);
                } else if (event.type === HttpEventType.Response) {
                    this.percentLoading = 0;
                    const img = $(this.previewImage);
                    let src = img.attr('src');
                    if (src == AppConsts.appBaseUrl + '/assets/common/images/Image_not_available.png') {
                        src = this.remoteServiceBaseUrl + this.s(this.settingName);
                    }
                    img.attr('src', src.substring(0, src.lastIndexOf('?')) + '?v=' + this.generateUUID());
                }
            })

        } catch (err) {
            console.log(err);
        }
    }

    getCookie(name: any) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + name + "=");
        if (parts.length == 2) return parts.pop().split(";").shift();
    }
}