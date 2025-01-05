import { ChangeDetectorRef, Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LanguageServiceProxy } from '@shared/service-proxies/service-proxies';
import { filter as _filter, map as _map } from 'lodash-es';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';
import { EditTextModalComponent } from './edit-text-modal.component';

@Component({
    templateUrl: './language-texts.component.html',
    animations: [appModuleAnimation()],
})
export class LanguageTextsComponent extends AppComponentBase implements OnInit {
    @ViewChild('targetLanguageNameCombobox', { static: true }) targetLanguageNameCombobox: ElementRef;
    @ViewChild('baseLanguageNameCombobox', { static: true }) baseLanguageNameCombobox: ElementRef;
    @ViewChild('sourceNameCombobox', { static: true }) sourceNameCombobox: ElementRef;
    @ViewChild('targetValueFilterCombobox', { static: true }) targetValueFilterCombobox: ElementRef;
    @ViewChild('textsTable', { static: true }) textsTable: ElementRef;
    @ViewChild('editTextModal', { static: true }) editTextModal: EditTextModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    sourceNames: string[] = [];
    languages: abp.localization.ILanguageInfo[] = [];
    targetLanguageName: string;
    sourceName: string;
    baseLanguageName: string;
    targetValueFilter: string;
    filterText: string;
    isChangePage: boolean = false;

    constructor(
        injector: Injector,
        private _languageService: LanguageServiceProxy,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private cdr: ChangeDetectorRef,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.sourceNames = _map(
            _filter(abp.localization.sources, (source) => source.type === 'MultiTenantLocalizationSource'),
            (value) => value.name
        );
        this.languages = abp.localization.languages;
        this.init();
    }

    getLanguageTexts(event?: LazyLoadEvent) {
        if (!this.paginator || !this.dataTable || !this.sourceName) {
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._languageService
            .getLanguageTexts(
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getSorting(this.dataTable),
                this.sourceName,
                this.baseLanguageName,
                this.targetLanguageName,
                this.targetValueFilter,
                this.filterText
            )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
                this.isChangePage = true;
            });
    }

    init(): void {
        this._activatedRoute.params.subscribe((params: Params) => {
            this.baseLanguageName = params['baseLanguageName'] || abp.localization.currentLanguage.name;
            this.targetLanguageName = params['name'];
            this.sourceName = params['sourceName'] || 'gAMSPro';
            this.targetValueFilter = params['targetValueFilter'] || 'ALL';
            this.filterText = params['filterText'] || '';
            this.isChangePage = false;
            this.reloadPage();
        });
    }

    reloadPage(): void {
        if (this.isChangePage) {
            this.paginator.changePage(this.paginator.getPage());
            return;
        }

        const self = this;
        const event: LazyLoadEvent = {
            first: this.paginator.first,
            rows: this.paginator.rows === 0 ? this.primengTableHelper.defaultRecordsCountPerPage : this.paginator.rows,
            forceUpdate: () => self.cdr.detectChanges()
        }
        this.getLanguageTexts(event);
    }

    applyFilters(event?: LazyLoadEvent): void {
        this._router.navigate([
            'app/admin/languages',
            this.targetLanguageName,
            'texts',
            {
                sourceName: this.sourceName,
                baseLanguageName: this.baseLanguageName,
                targetValueFilter: this.targetValueFilter,
                filterText: this.filterText,
            },
        ]);
        // if (this.paginator.getPage() !== 0) {
        //     this.paginator.changePage(0);
        //     this.isChangePage = true;
        // }
    }

    truncateString(text): string {
        return abp.utils.truncateStringWithPostfix(text, 32, '...');
    }

    refreshTextValueFromModal(): void {
        for (let i = 0; i < this.primengTableHelper.records.length; i++) {
            if (this.primengTableHelper.records[i].key === this.editTextModal.model.key) {
                this.primengTableHelper.records[i].targetValue = this.editTextModal.model.value;
                return;
            }
        }
    }

    copyToClipboard(value: string): void {
        navigator.clipboard.writeText(value);
        this.notify.success(this.l('CopiedSuccessfully'));
    }
}
