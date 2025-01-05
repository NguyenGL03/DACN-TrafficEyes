import { Injectable } from '@angular/core';
import { NotifyService } from 'abp-ng2-module';
import { StyleService } from './style.service';


@Injectable({
    providedIn: 'root',
})
export class CustomNotifyService extends NotifyService {
    constructor(private styleService: StyleService) {
        super();
    }
    info(message: string, title?: string, options?: any): void {
        options = this.addCustomStyles(options, 'info');
        super.info(message, title, options);
    }

    success(message: string, title?: string, options?: any): void {
        options = this.addCustomStyles(options, 'success');
        super.success(message, title, options);
    }

    warn(message: string, title?: string, options?: any): void {
        options = this.addCustomStyles(options, 'warn');
        super.warn(message, title, options);
    }

    error(message: string, title?: string, options?: any): void {
        options = this.addCustomStyles(options, 'error');
        super.error(message, title, options);
    }

    private addCustomStyles(options: any, type: string): any {
        options = options || {};
        options.customClass = `${options.customClass || ''} custom-${type}-toast`;
        return options;
    }
    setDynamicStyle(className: string, color: string) {
        this.styleService.setStyle(`.${className}`, `background-color: ${color} !important;`);
    }
}
