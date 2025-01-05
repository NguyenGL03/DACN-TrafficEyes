import { HttpClient } from "@angular/common/http";
import { Component, ElementRef, EventEmitter, Inject, Injector, Input, OnDestroy, OnInit, Optional, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";
// import { ControlComponent, createCustomInputControlValueAccessor } from "@app/admin/core/controls/control.component";
import { AppConsts } from "@shared/AppConsts";
import { API_BASE_URL, AttachFileServiceProxy, CM_IMAGE_ENTITY, UltilityServiceProxy } from "@shared/service-proxies/service-proxies";
import { ImageCarouselUploadModalComponent } from "./image-carousel-upload-modal.component";
import { ControlComponent, createCustomInputControlValueAccessor } from "../control.component";
import { DefaultComponentBase } from "@app/utilities/default-component-base";
import { AppComponentBase } from "@shared/common/app-component-base";
// import imageCompression from 'browser-image-compression'
declare const ProgressBar;
declare var $: JQueryStatic;


@Component({
    moduleId: module.id,
    selector: "image-carousel",
    templateUrl: "./image-carousel.component.html",
    encapsulation: ViewEncapsulation.None,
    providers: [createCustomInputControlValueAccessor(ImageCarouselComponent)]
})

export class ImageCarouselComponent extends ControlComponent implements OnInit, OnDestroy {
    writeValue(value: any): void {
        throw new Error("Method not implemented.");
    }

    imageSources: string[];
    imageList: CM_IMAGE_ENTITY[];
    idProgress: string;
    serverUrl: string;
    currentImgSrc: string;
    currentIndex: number = 0;
    opacity: number = 1;

    // private injector: Injector;
    constructor(
        injector: Injector,
        private attachFileService: AttachFileServiceProxy,
        private sanitizer: DomSanitizer,
        private ultilityService: UltilityServiceProxy,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector);
        this.injector = injector;
        this.ultilityService = this.injector.get(UltilityServiceProxy);
        this.idProgress = 'f' + this.generateUUID();
        // this.serverUrl = AppConsts.remoteImageBaseUrl;
        this.imageList = [];

        this.imageSources = [];
    }

    httpClient: HttpClient;
    @Input() title: string = this.l("ImageAttached");
    @Input() imageTitle: string;
    @Input() imageName: string;

    @Input() disabled: boolean;
    @Input() folderName: string;
    @Input() showButtonOnly: boolean = false;
    @Input() limit: number = parseInt(this.s("gAMSProCore.MaxImagePicker"));
    @Input() maxWidth: number = 700;
    @Input() maxHeight: number = 700;
    @Input() widthInput: string //Phucvh 17/10/22 Custom Size Image
    @Input() heightInput: string //Phucvh 17/10/22 Custom Size Image
    @Input() opacityInput: number //Phucvh 17/10/22 Custom Size Image
    refId: string
    indexInGrid: number

    @ViewChild('fileControl') fileControl: ElementRef;
    @ViewChild('loading') loading: ElementRef;
    @ViewChild('imageUpload') imageUploadModal: ImageCarouselUploadModalComponent;
    @Output() onSelectImage: EventEmitter<any> = new EventEmitter<any>();

    loadingProgress: any;

    afterViewInit() {
        //
        setTimeout(() => {
            this.loadingProgress = new ProgressBar.Circle(this.loading.nativeElement, {
                strokeWidth: 6,
                easing: 'easeInOut',
                duration: 1400,
                color: '#639bb7',
                trailColor: '#eee',
                trailWidth: 1,
                svgStyle: null
            });
        }, 1000)


    }

    //Lấy hình ảnh từ db về (k hiện modal)
    getImage(refId) {
        this.refId = refId;
        this.attachFileService.cM_IMAGE_ByRefId(this.refId).subscribe(resp => {
            resp.forEach(x => {
                x.basE64 = 'data:image/jpeg;base64,' + x.basE64;
            })
            this.imageList = resp;
            this.updateImageSource();
            this.currentIndex = 0;
            this.currentImgSrc = this.imageSources[this.currentIndex];

        })
    }

    //Lấy hình ảnh dựa vào mã tài sản
    getImageByAssetCode(assCode) {
        this.attachFileService.cM_IMAGE_ByAssCode(assCode).subscribe(resp => {
            resp.forEach(x => {
                if (x.basE64) {
                    x.basE64 = 'data:image/jpeg;base64,' + x.basE64;
                }
            })
            this.imageList = resp;
            this.updateImageSource();
        });
    }

    //Lấy hình ảnh gần nhất của tài sản
    getImageByAssetNear(assId, date, isShow = false) {
        this.attachFileService.cM_IMAGE_GetNearAsset(assId, date || "").subscribe(resp => {
            resp.forEach(x => {
                x.basE64 = 'data:image/jpeg;base64,' + x.basE64;
            })
            this.imageList = resp;
            this.updateImageSource();
            if (isShow) {
                this.showUpLoadModal();
            }
        });
    }

    //Lấy hình ảnh đầu tiên của tài sản
    getImageByAssetFirst(assId, isShow = false) {
        this.attachFileService.cM_IMAGE_GetFirstAsset(assId, undefined).subscribe(resp => {
            resp.forEach(x => {
                x.basE64 = 'data:image/jpeg;base64,' + x.basE64;
            })
            this.imageList = resp;
            this.updateImageSource();
            if (isShow) {
                this.showUpLoadModal();
            }
        });
    }


    ngOnInit(): void {
        console.log("ngOnInit");
    }

    showSlide(index) {
        this.opacity = 0.25;
        this.currentImgSrc = this.imageSources[index];
        this.currentIndex = index;

        this.fadeIn();
    }

    fadeIn() {
        setTimeout(() => {
            this.opacity = 1;

        }, 200);
    }

    async uploadFile(fileInput) {
        const options = {
            maxSizeMB: 4,
        }
        try {
            if (fileInput.target.files.length + this.imageUploadModal.imageSources.length > this.limit) {
                alert(this.l('UpLoadLimit{0}File', this.limit));
                return;
            }

            var _length = this.limit - this.imageUploadModal.imageSources.length;

            for (let i = 0; i < fileInput.target.files.length; i++) {
                let reader = new FileReader();
                reader.readAsDataURL(fileInput.target.files[i]);
                reader.onload = async () => {
                    var f = new CM_IMAGE_ENTITY();

                    this.resizeImage(reader.result.toString(), this.maxWidth, this.maxHeight).then(compressed => {
                        if (this.imageTitle) {

                            this.addTextToImage(compressed, this.imageTitle, (param) => {
                                f.basE64 = param as string;
                                f.imagE_NAME = this.imageName;
                                this.imageUploadModal.imageList.push(f);
                                this.imageUploadModal.updateImageSource();
                            });
                        }
                        else {
                            f.basE64 = compressed as string;
                            f.imagE_NAME = this.imageName;
                            this.imageUploadModal.imageList.push(f);
                            this.imageUploadModal.updateImageSource();

                        }
                    })
                    f.filE_NAME = this.generateUUID() + '.' + fileInput.target.files[i].name.split('.').pop();
                    f.path = null;
                };
            }
        }
        catch (e) {
            console.log(e);
        }
    }
    urltoFile(url, filename, mimeType) {
        return (fetch(url)
            .then(function (res) {
                return res.arrayBuffer();
            })
            .then(function (buf) {
                return new File([buf], filename, { type: mimeType });
            })
        );
    }
    async resizeImage(src, maxWidth, maxHeight) {
        var targetFileSizeKb = 4000;
        var maxDeviation = 1;
        let originalFile = await this.urltoFile(src, 'test.png', 'image/png');
        if (originalFile.size / 1000 < targetFileSizeKb)
            return src; // File is already smaller

        let low = 0.0;
        let middle = 0.5;
        let high = 1.0;

        let result = src;

        let file = originalFile;

        while ((file.size / 1000 - targetFileSizeKb) > maxDeviation) {
            const canvas = document.createElement("canvas");
            const context = canvas.getContext("2d");
            const img = document.createElement('img');

            const promise = new Promise<void>((resolve, reject) => {
                img.onload = () => resolve();
                img.onerror = reject;
            });

            img.src = result;

            await promise;

            canvas.width = Math.round(img.width * middle);
            canvas.height = Math.round(img.height * middle);
            context.scale(canvas.width / img.width, canvas.height / img.height);
            context.drawImage(img, 0, 0);
            file = await this.urltoFile(canvas.toDataURL(), 'test.png', 'image/png');

            middle = (low + high) / 2;
            result = canvas.toDataURL();
        }
        return result;
    }

    toDataURL(url, callback) {
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {
            var reader = new FileReader();
            reader.onloadend = function () {
                callback(reader.result);
            }
            reader.readAsDataURL(xhr.response);
        };
        xhr.open('GET', url);
        xhr.responseType = 'blob';
        xhr.send();
    }
    sanitizeImageUrl(imageUrl: string): SafeUrl {
        return this.sanitizer.bypassSecurityTrustUrl(imageUrl);
    }
    //Lấy ảnh từ imageList rồi hiện lên view
    updateImageSource() {
        var list = [];
        this.imageList.forEach(img => {
            if (img.basE64 != null) {
                list.push(img.basE64);
            }
            else {
                if (img.path != null) {
                    list.push(this.sanitizeImageUrl(this.serverUrl + img.path));
                }
            }
        })

        this.imageSources = list;

        this.currentIndex = 0;
        this.currentImgSrc = this.imageSources[this.currentIndex];


    }

    showLoading() {
        $(this.loading.nativeElement).show();
    }

    hideLoading() {
        this.loadingProgress.set(0);
    }


    //Lưu hình xuống db
    saveToDb(refId: string, folderName: string) {
        this.refId = refId;
        this.folderName = folderName;

        if (refId != undefined &&
            refId != null &&
            refId != '') {
            this.attachFileService.uploadImageFiles(this.refId, folderName, this.imageList).subscribe(resp => {
                console.log(resp);
            });
        } else {
            console.error("RefId trống");
        }
    }

    //Lưu hình (hình truyền từ bên ngoài) xuống db
    saveImageModelToDb(refId: string, images: CM_IMAGE_ENTITY[], folderName: string) {
        this.folderName = folderName;
        this.attachFileService.uploadImageFiles(refId, folderName, images).subscribe(resp => {
            console.log(resp);
        });
    }

    showUpLoadModal() {
        this.imageUploadModal.show(this);
    }

    //Lấy hình ảnh tử db và hiện upload modal lên
    getImageThenShowUpLoadModel(refId: string) {
        this.refId = refId;
        this.attachFileService.cM_IMAGE_ByRefId(this.refId).subscribe(resp => {
            resp.forEach(x => {
                if (x.basE64) {
                    x.basE64 = 'data:image/jpeg;base64,' + x.basE64;
                }
            })
            this.imageList = resp;
            this.updateImageSource();
            this.imageUploadModal.show(this);
        })
    }

    //Nạp trực tiếp ảnh vào upload modal và hiện lên
    pushImageThenShowUpLoadModel(images: CM_IMAGE_ENTITY[]) {

        if (images == null || images == undefined) {
            images = [];
        }

        this.imageList = images;
        this.updateImageSource();
        this.imageUploadModal.show(this);
    }

    ngOnDestroy(): void {
        console.log('ngOnDestroy');
    }
}
