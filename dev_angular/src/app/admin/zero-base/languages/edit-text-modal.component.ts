import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LanguageServiceProxy, UpdateLanguageTextInput } from '@shared/service-proxies/service-proxies';
import { find as _find } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'editTextModal',
    templateUrl: './edit-text-modal.component.html',
})
export class EditTextModalComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    model: UpdateLanguageTextInput = new UpdateLanguageTextInput();

    key: string;
    baseText: string;
    baseLanguage: abp.localization.ILanguageInfo;
    targetLanguage: abp.localization.ILanguageInfo;

    active = false;
    saving = false;
    isEditing: boolean = false;

    constructor(injector: Injector, private _languageService: LanguageServiceProxy) {
        super(injector);
    }

    show(
        baseLanguageName: string,
        targetLanguageName: string,
        sourceName: string,
        key: string,
        baseText: string,
        targetText: string
    ): void { 
        this.isEditing = !!key;
        this.model.mode = 'edit';
        this.model.sourceName = sourceName;
        this.model.key = key;
        this.model.languageName = targetLanguageName;
        this.model.value = targetText;

        this.baseText = baseText;
        this.baseLanguage = _find(abp.localization.languages, (l) => l.name === baseLanguageName);
        this.targetLanguage = _find(abp.localization.languages, (l) => l.name === targetLanguageName);

        this.active = true;

        this.modal.show();
    }

    onShown(): void {
        document.getElementById('TargetLanguageDisplayName').focus();
    }

    save(): void {
        this.showLoading(); 
        this._languageService
            .updateLanguageText(this.model)
            .pipe(finalize(() => (this.hideLoading())))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    private findLanguage(name: string): abp.localization.ILanguageInfo {
        return _find(abp.localization.languages, (l) => l.name === name);
    }
}
