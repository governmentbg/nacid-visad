import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { DiplomaType } from '../models/diploma-type.model';

@Component({
	selector: 'app-diploma-type',
	templateUrl: './diploma-type.component.html',
})
export class DiplomaTypeComponent extends BaseNomenclatureComponent<DiplomaType> implements OnInit {

	ngOnInit(): void {
		this.init(DiplomaType, 'DiplomaType');
	}

}
