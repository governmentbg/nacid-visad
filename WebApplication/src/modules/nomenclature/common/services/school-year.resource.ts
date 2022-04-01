import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

@Injectable()
export class SchoolYearResource extends BaseResource {
	constructor(
		protected http: HttpClient,
		protected configuration: Configuration
	) {
		super(http, configuration, 'SchoolYear');
	}

	getDefaultYears(): Observable<NomenclatureDto> {
		return this.http.get<NomenclatureDto>(`${this.baseUrl}/default`);
	}
}
