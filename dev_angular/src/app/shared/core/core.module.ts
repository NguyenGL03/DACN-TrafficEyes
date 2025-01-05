import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { TreeDragDropService } from 'primeng/api';
import { CoreRoutingModule } from './core-routing.module';
import { CoreServiceProxyModule } from './core-service-proxy.module';

@NgModule({
    imports: [
        CoreServiceProxyModule,
        CoreRoutingModule,
        FormsModule,
        ReactiveFormsModule,
    ],
    declarations: [
    ],
    exports: [

    ],
    providers: [
        TreeDragDropService, 
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class CCoreModule { }
