import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { ApplicationReportDto } from '../models/report/application-report.dto';
import { ApplicationReportSearchFilter } from './application-report-search.filter';

@Injectable()
export class ApplicationReportResource extends BaseResource {

	constructor(
		protected http: HttpClient,
		protected configuration: Configuration
	) {
		super(http, configuration, 'Report');
	}

	getFiltered(filter?: ApplicationReportSearchFilter): Observable<ApplicationReportDto> {
		return this.http.get<ApplicationReportDto>(`${this.baseUrl}${this.composeQueryString(filter)}`);
	}
}
