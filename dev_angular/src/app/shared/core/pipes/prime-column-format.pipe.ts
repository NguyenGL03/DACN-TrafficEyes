import { Pipe, PipeTransform } from '@angular/core';
import moment from 'moment';
import { ColumnType } from '../controls/primeng-table/prime-table/primte-table.interface';

@Pipe({
    name: 'primeFormatPipe',
})
export class PrimeFormatPipe implements PipeTransform {
    transform(value: any, type: ColumnType) {
        if (!value || !type) return value;
        switch (type) {
            case ColumnType.Date:
                return this.formatDate(value);
            case ColumnType.DateTime:
                return this.formatDateTime(value);
            case ColumnType.DateTime24h:
                return this.formatDateTime24h(value);
            case ColumnType.Curency || 'money':
                return this.formatMoney(value);
            default:
                return value
        }
    }

    formatDate(value: moment.Moment): string {
        return moment(value).format('DD/MM/YYYY');
    }

    formatDateTime(value: moment.Moment): string {
        return moment(value).format('DD/MM/YYYY, h:mm:ss a');
    }
    formatDateTime24h(value: moment.Moment): string {
        return moment(value).format('DD/MM/YYYY, HH:mm:ss');
    }
    formatMoney(value: number): string {
        return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }

}