import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { ApplicationFileType } from '../models/application-file-type.model';

@Injectable()
export class ApplicationFileTypeResource extends BaseResource {
	constructor(
		protected http: HttpClient,
		protected configuration: Configuration
	) {
		super(http, configuration);
	}

	getApplicationFileTypes(qualificationName: string): Observable<ApplicationFileType[]> {
		return this.http.get<ApplicationFileType[]>(`${this.baseUrl}/applicationfiletype/filtered?qualificationName=${qualificationName}`);
	}

	selectApplicationFileTypes(alias: string): Observable<ApplicationFileType> {
		return this.http.get<ApplicationFileType>(`${this.baseUrl}/applicationfiletype/selectFile?alias=${alias}`);
	}
}
