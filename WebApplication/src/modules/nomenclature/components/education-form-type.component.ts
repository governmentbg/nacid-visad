import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { EducationFormType } from '../models/education-form-type.model';

@Component({
	selector: 'app-education-form-type',
	templateUrl: './../common/base-nomenclature.component.html',
})
export class EducationFormTypeComponent extends BaseNomenclatureComponent<EducationFormType> implements OnInit {

	ngOnInit(): void {
		this.init(EducationFormType, 'EducationFormType');
	}

}
