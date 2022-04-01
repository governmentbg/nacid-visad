import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { Language } from '../models/language.model';

@Component({
	selector: 'app-language',
	templateUrl: './../common/base-nomenclature.component.html',
})
export class LanguageComponent extends BaseNomenclatureComponent<Language> implements OnInit {
	ngOnInit(): void {
		this.init(Language, 'Language');
	}
}
