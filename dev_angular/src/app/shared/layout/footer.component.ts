import { Component, Injector, OnInit, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppConsts } from '@shared/AppConsts';
import { ThemeAssetContributorFactory } from '@shared/helpers/ThemeAssetContributorFactory';
import moment from 'moment';

@Component({
    templateUrl: './footer.component.html',
    selector: 'footer-bar',
})
export class FooterComponent extends AppComponentBase implements OnInit {
    @Input() useBottomDiv = true;

    releaseDate: string;
    webAppGuiVersion: string;
    footerStyle = 'footer py-4 d-flex flex-lg-column';

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
        this.releaseDate = moment(this.appSession.application.releaseDate).format('MMDDYYYY');
        this.webAppGuiVersion = AppConsts.WebAppGuiVersion;

        let themeAssetContributor = ThemeAssetContributorFactory.getCurrent();
        if (themeAssetContributor) {
            this.footerStyle = themeAssetContributor.getFooterStyle();
        }
    }
}
