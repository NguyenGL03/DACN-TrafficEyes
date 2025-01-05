import { NgModule } from '@angular/core';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { commonDeclarationImports } from '@app/shared/core/utils/CommonDeclarationModule';
import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { TreeDragDropService } from 'primeng/api';
import { NgxBootstrapDatePickerConfigService } from '../../assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { AdminRoutingModule } from './admin-routing.module';
import { CommonModule } from './common/common.module';
import { ZeroBaseModule } from './zero-base/zero-base.module';


@NgModule({
    imports: [
        commonDeclarationImports,
        AppCommonModule,
        CommonModule,
        ZeroBaseModule,
        AdminRoutingModule,
    ],
    declarations: [],
    exports: [],
    providers: [
        TreeDragDropService,
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ],
})
export class AdminModule { }
