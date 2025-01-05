import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { LazyDropdownResponse } from "@app/shared/core/controls/dropdown-lazy-control/dropdown-lazy-control.component";
import { PopupBaseDefaultComponent } from "@app/shared/core/controls/popup-base/popup-base-default.component";
import { TL_USER_ENTITY, TlUserServiceProxy } from "@shared/service-proxies/service-proxies";
@Component({
    selector: "approve-authority-modal",
    templateUrl: "./approve-authority-modal.component.html",
    encapsulation: ViewEncapsulation.None
})
export class ApproveAuthorityModalComponent extends PopupBaseDefaultComponent {

    @ViewChild('editForm') editForm: ElementRef;

    @Output() onSubmitEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onCancelEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onRejectEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() onShowEvent: EventEmitter<any> = new EventEmitter<any>();

    @Input() isShowReturnType: boolean = false;
    @Input() visibleIsAuthority: boolean = false;
    @Input() isAuthority: boolean;

    selectedReturnType: string;
    buttonContent: string = this.l("Approve");
    item: string = "";
    isShowError = false;

    waiting: boolean;
    active: boolean = false;
    tlUsers: TL_USER_ENTITY[];
    tlUser: TL_USER_ENTITY;

    constructor(injector: Injector, private tlUserService: TlUserServiceProxy) {
        super(injector);
    }

    InitAndShow(initVal: string = null) {
        this.selectedReturnType = initVal;
        this.show();
    }

    getUser(data?: LazyDropdownResponse): void {
        const filterInput: TL_USER_ENTITY = new TL_USER_ENTITY();
        filterInput.skipCount = data.state?.skipCount;
        filterInput.totalCount = data.state?.totalCount;
        filterInput.maxResultCount = data.state?.maxResultCount;
        filterInput.tlFullName = data.state?.filter;
        this.tlUserService.tL_USER_GET_List_v2(filterInput)
            .subscribe(result => {
                data.callback(result)
            })
    }

    show() {
        this.active = true;
        this.visible = true;
    }

    onShown() {
        this.onShowEvent.emit(null);
    }

    close() {
        this.active = false;
        this.visible = false;
    }

    onSubmit() {
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            return;
        }
        var returnVal = {
            item: this.item,
            returnType: this.selectedReturnType,
            isAuthority: this.isAuthority
        }
        this.onSubmitEvent.emit(returnVal);
        this.item = '';
        this.isShowError = false;
        this.close();
    }

    onReject() {
        if ((this.editForm as any).form.invalid) {
            this.isShowError = true;
            return;
        }
        var returnVal = {
            item: this.item,
            returnType: this.selectedReturnType
        }
        this.onRejectEvent.emit(returnVal);
        this.item = '';
        this.isShowError = false;
        this.close();
    }

    onCancel() {
        this.onCancelEvent.emit(null);
        this.close();
    }
    RefreshTitle(title: string) {
        this.title = title;
    }
    RefreshButtonContent(button: string) {
        this.buttonContent = button;
    }
    enableSubmit = true;

    onKeyUp() {
        this.enableSubmit = /^(?!\s*$).+/.test(this.item);
    }
}
