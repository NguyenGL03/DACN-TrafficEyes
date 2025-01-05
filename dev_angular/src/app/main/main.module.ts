import { NgModule } from '@angular/core';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { CommonModule } from './common/common.module';
import { MainRoutingModule } from './main-routing.module'; 

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
    imports: [
        commonDeclarationImports,
        AppCommonModule,
        CommonModule,
        MainRoutingModule,
    ],
    declarations: [
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
    ],
})
export class MainModule { }
