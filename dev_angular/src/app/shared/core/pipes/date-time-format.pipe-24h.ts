import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
    name: 'dateTimeFormatPipe24h',
})
export class DateTime24hFormatPipe implements PipeTransform {
    
    getDateFormatString(){
        return 'DD/MM/YYYY, HH:mm:ss';
    }

    momentToString(m : moment.Moment){
        return moment(m).format(this.getDateFormatString());
    }

    transform(value: moment.Moment) {
        if (!value) {
            return '';
        }
        return this.momentToString(value);
    }
}