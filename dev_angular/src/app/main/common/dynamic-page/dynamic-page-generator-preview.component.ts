import { AfterViewInit, Component, Injector, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { InputType } from "@app/shared/core/controls/primeng-table/prime-table/primte-table.interface";
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { DYNAMIC_PAGE_ENTITY } from "@shared/service-proxies/service-proxies";

@Component({
    selector: 'dynamic-page-generator-preview',
    templateUrl: './dynamic-page-generator-preview.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})

export class DynamicPageGeneratorPreviewComponent extends DefaultComponentBase implements OnInit, AfterViewInit {


    _page: DYNAMIC_PAGE_ENTITY = new DYNAMIC_PAGE_ENTITY();
    @Input() set page(data: DYNAMIC_PAGE_ENTITY) {
        console.log(data)
        this._page = data;
    }
    get page() {
        return this._page
    }

    constructor(injector: Injector) {
        super(injector);
    }

    get type(): typeof InputType {
        return InputType;
    }

    ngOnInit(): void {

    }

    ngAfterViewInit(): void {

    }
}
