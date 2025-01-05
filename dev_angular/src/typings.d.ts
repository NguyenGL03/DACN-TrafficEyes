///<reference path="../node_modules/abp-web-resources/Abp/Framework/scripts/abp.d.ts"/>
///<reference path="../node_modules/abp-web-resources/Abp/Framework/scripts/libs/abp.jquery.d.ts"/>
///<reference path="../node_modules/abp-web-resources/Abp/Framework/scripts/libs/abp.signalr.d.ts"/>

// Typings reference file, see links for more information
// https://github.com/typings/typings
// https://www.typescriptlang.org/docs/handbook/writing-declaration-files.html

declare let System: any;

declare let KTApp: any; // Related to Metronic
declare let KTOffcanvas: any; // Related to Metronic
declare let KTMenu: any; // Related to Metronic
declare let KTToggle: any; // Related to Metronic
declare let KTUtil: any; // Related to Metronic
declare let KTHeader: any; // Related to Metronic
declare let KTScrolltop: any; // Related to Metronic
declare let StripeCheckout: any;

declare namespace abp {
    namespace ui {
        function setBusy(elm?: any, text?: any, optionsOrPromise?: any): void;
    }
}

interface Array<T> {
    firstOrDefault: (callbackfn: (value: T, index: number, array: T[]) => boolean, option1?: any) => T;
    sum: (callbackfn?: (value: T, index: number, array: T[]) => number) => number;
    sumWDefault: (callbackfn?: (value: T, index: number, array: T[]) => number, valDefault?: any = 0) => number;
    distinct: () => T[]
}
interface JQuery {
    select2(options?: any): JQuery;
    flatpickr(options?: any): JQuery;
    dialog(options?: any): JQuery;
    autocomplete(option?: any): JQuery;
    printThis(option?: any): JQuery;
    print(option?: any): JQuery;

    tooltip(option?: any): JQuery;
    resizable(option?: any): JQuery;
}
/**
 * rtl-detect
 */

declare module 'rtl-detect';
