import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { LanguageDegree } from '../models/language-degree.model';

@Component({
	selector: 'app-language-degree',
	templateUrl: './../common/base-nomenclature.component.html',
})
export class LanguageDegreeComponent extends BaseNomenclatureComponent<LanguageDegree> implements OnInit {
	ngOnInit(): void {
		this.init(LanguageDegree, 'LanguageDegree');
	}
}
