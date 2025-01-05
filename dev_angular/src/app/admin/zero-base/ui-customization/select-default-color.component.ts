import { Component, EventEmitter, Injector, Input, Output, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { HostSettingsEditDto, HostSettingsServiceProxy } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/utils/local-storage.service';

@Component({
    templateUrl: './select-default-color.component.html',
    animations: [appModuleAnimation()],
    encapsulation: ViewEncapsulation.None,
    selector: 'select-default-color',
})
export class selectDefaultColorComponent extends AppComponentBase {
    @Input() hostSettings: HostSettingsEditDto;
    @Output() emitFigma: EventEmitter<any> = new EventEmitter<any>()

    selectedFigma: any

    constructor(
        injector: Injector,
        private _hostSettingsService: HostSettingsServiceProxy,
        private localStorageService: LocalStorageService

    ) {
        super(injector);
    }

    ngOnInit() {
        this.localStorageService.getItem('theme', (err, value) => {
            this.selectedFigma = value
        })
    }

    selectingTheme(figma: string) {
        console.log("selectDefaultColorComponent ~ figma:", figma)
        switch (figma) {
            case 'theme1':
                this.figma1()
                this.selectedFigma = "theme1"
                break;

            case 'theme2':
                this.figma2()
                this.selectedFigma = "theme2"
                break;

            default:
                break;
        }
    }

    saveHostSettings(): void {
        this.notify.success(this.l('SavedSuccessfully'));
        this.localStorageService.setItem('theme', this.hostSettings.commonThemeSettings.theme)
        this._hostSettingsService.updateAllSettings(this.hostSettings)
            .subscribe((result) => {
                setTimeout(() => window.location.reload(), 1000);
            });
    }

    figma1() {
        const theme = this.hostSettings.commonThemeSettings
        //theme
        theme.theme = 'theme1'
        //main
        theme.borderColor = 'rgba(255, 255, 255, 1)'
        theme.svgColor = 'rgba(51, 51, 51, 1)'
        //header
        theme.headerBackground = 'rgba(255, 255, 255, 0.5)'
        theme.headerBorderColor = 'rgba(255, 255, 255, 1)'
        theme.headerColorText = '#000000'
        theme.headerLogoBgColor = 'rgba(255, 255, 255, 0)'
        theme.headerLogoBorderColor = 'rgba(255, 255, 255, 0)'
        theme.headerWrapperBgColor = 'rgba(255, 255, 255, 0)'
        theme.headerWrapperBorderColor = 'rgba(255, 255, 255, 0)'
        //sidebar
        theme.sidebarColorText = 'rgba(51, 51, 51, 1)'
        theme.sideBarChildBgColor = 'rgba(53, 119, 219, 0.05)'
        theme.sidebarBackground = 'rgba(255, 255, 255, 0.5)'
        theme.sidebarColorTextActive = 'rgba(53, 119, 219, 1)'
        //main
        theme.primaryColor = 'rgba(53, 119, 219, 1)'
        theme.webBackground = 'rgba(238, 241, 243, 1)'
        theme.textColor = 'rgba(51, 51, 51, 1)'
        //card
        theme.cardBackground = 'rgba(255, 255, 255, 0.5)'
        //table
        theme.tableHeadBgColor = 'rgba(53, 119, 219, 0.1)'
        theme.tableBodyOddBgColor = 'rgba(255, 255, 255, 1)'
        theme.tableBodyEvenBgColor = 'rgba(255, 255, 255, 1)'
        theme.tableMainColor = 'rgba(53, 119, 219, 1)'
        theme.tableBodyTextColor = 'rgba(51, 51, 51, 1)'
        theme.tableBorderColor = 'rgba(221, 231, 242, 1)'
        theme.tableHighlightColor = 'rgba(53, 119, 219, 1)'
        //input
        theme.inputLabelColor = 'rgba(51, 51, 51, 1)'
        theme.inputBorderColor = 'rgba(221, 231, 242, 1)'
        theme.inputTextColor = 'rgba(51, 51, 51, 1)'
        theme.inputIconColor = 'rgba(53, 119, 219, 1)'
        theme.inputBgcolor = 'rgba(255, 255, 255, 1)'
        // toolbar
        theme.btnToolbarBgColor = 'rgba(255, 255, 255, 0.5)'
        theme.btnToolbarBorderColor = 'rgba(255, 255, 255, 1)'
        theme.btnToolbarTextColor = 'rgba(51, 51, 51, 1)'

        // Dark
        //main
        theme.darkBorderColor = 'rgba(255, 255, 255, 0.2)'
        theme.darkSvgColor = 'rgba(255, 255, 255, 1)'
        //header
        theme.darkHeaderBackground = 'rgba(44, 52, 58, 1)'
        theme.darkHeaderBorderColor = 'rgba(255, 255, 255, 0.2)'
        theme.darkHeaderColorText = 'rgba(255, 255, 255, 1)'
        theme.darkHeaderLogoBgColor = 'rgba(44, 52, 58, 1)'
        theme.darkHeaderLogoBorderColor = 'rgba(255, 255, 255, 0)'
        theme.darkHeaderWrapperBgColor = 'rgba(44, 52, 58, 1)'
        theme.darkHeaderWrapperBorderColor = 'rgba(255, 255, 255, 0)'
        //sidebar
        theme.darkSidebarColorText = 'rgba(255, 255, 255, 1)'
        theme.darkSideBarChildBgColor = 'rgba(53, 119, 219, 0.3)'
        theme.darkSidebarBackground = 'rgba(44, 52, 58, 1)'
        theme.darkSidebarColorTextActive = 'rgba(255, 255, 255, 1)'
        //main
        theme.darkPrimaryColor = 'rgba(53, 119, 219, 1)'
        theme.darkWebBackground = 'rgba(25, 29, 35, 1)'
        theme.darkTextColor = 'rgba(255, 255, 255, 1)'
        //card
        theme.darkCardBackground = 'rgba(44, 52, 58, 1)'
        //table
        theme.darkTableHeadBgColor = 'rgba(53, 119, 219, 0.3)'
        theme.darkTableBodyOddBgColor = 'rgba(44, 52, 58, 1)'
        theme.darkTableBodyEvenBgColor = 'rgba(44, 52, 58, 1)'
        theme.darkTableMainColor = 'rgba(255, 255, 255, 1)'
        theme.darkTableBodyTextColor = 'rgba(255, 255, 255, 1)'
        theme.darkTableBorderColor = 'rgba(255, 255, 255, 0.2)'
        theme.darkTableHighlightColor = 'rgba(53, 119, 219, 0.3)'
        //input
        theme.darkInputLabelColor = 'rgba(255, 255, 255, 1)'
        theme.darkInputBorderColor = 'rgba(255, 255, 255, 0.2)'
        theme.darkInputTextColor = 'rgba(255, 255, 255, 1)'
        theme.darkInputIconColor = 'rgba(53, 119, 219, 1)'
        theme.darkInputBgcolor = 'rgba(63, 73, 80, 1)'
        // toolbar
        theme.darkBtnToolbarBgColor = 'rgba(53, 119, 219, 0.3)'
        theme.darkBtnToolbarBorderColor = 'rgba(255, 255, 255, 0.2)'
        theme.darkBtnToolbarTextColor = 'rgba(255, 255, 255, 1)'
    }

    figma2() {
        const theme = this.hostSettings.commonThemeSettings
        //theme
        theme.theme = 'theme2'
        //main
        theme.borderColor = 'rgba(255, 255, 255, 0)'
        theme.svgColor = 'rgba(255, 255, 255, 1)'
        //header
        theme.headerBorderColor = 'rgba(255, 255, 255, 0)'
        theme.headerBackground = 'rgba(255, 255, 255, 0)'
        theme.headerLogoBorderColor = 'rgba(255, 255, 255, 0.2)'
        theme.headerWrapperBgColor = 'rgba(53, 119, 219, 0.5)'
        theme.headerWrapperBorderColor = 'rgba(255, 255, 255, 0.2)'
        theme.headerColorText = 'rgba(51, 51, 51, 1)'
        theme.headerLogoBgColor = 'rgba(53, 119, 219, 0.5)'
        //sidebar
        theme.sideBarChildBgColor = 'rgba(53, 119, 219, 0.4)'
        theme.sidebarBackground = 'rgba(255, 255, 255, 0)'
        theme.sidebarColorText = 'rgba(255, 255, 255, 1)'
        theme.sidebarColorTextActive = 'rgba(255, 255, 255, 1)'
        //main
        theme.primaryColor = 'rgba(255, 255, 255, 1)'
        theme.webBackground = 'rgba(3, 93, 191, 1)'
        theme.textColor = 'rgba(255, 255, 255, 1)'
        //card
        theme.cardBackground = 'rgba(255, 255, 255, 1)'
        //table
        theme.tableHeadBgColor = 'rgba(237, 245, 255, 1)'
        theme.tableBodyOddBgColor = 'rgba(255, 255, 255, 1)'
        theme.tableBodyEvenBgColor = 'rgba(255, 255, 255, 1)'
        theme.tableMainColor = 'rgba(53, 119, 219, 1)'
        theme.tableBodyTextColor = 'rgba(51, 51, 51, 1)'
        theme.tableBorderColor = 'rgba(203, 215, 228, 1)'
        theme.tableHighlightColor = 'rgba(53, 119, 219, 1)'
        //input
        theme.inputLabelColor = 'rgba(51, 51, 51, 1)'
        theme.inputBorderColor = 'rgba(221, 231, 242, 1)'
        theme.inputTextColor = 'rgba(51, 51, 51, 1)'
        theme.inputIconColor = 'rgba(53, 119, 219, 1)'
        theme.inputBgcolor = 'rgba(255, 255, 255, 1)'
    }

}
