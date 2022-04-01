import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { ApplicationDraftDto } from '../models/application/application-draft.dto';

@Injectable()
export class ApplicationDraftResource extends BaseResource {
	constructor(
		http: HttpClient,
		configuration: Configuration
	) {
		super(http, configuration, 'Draft');
	}

	getUserDraftApplications(): Observable<ApplicationDraftDto[]> {
		return this.http.get<ApplicationDraftDto[]>(this.baseUrl);
	}

	getDraftApplication(id: number): Observable<ApplicationDraftDto> {
		return this.http.get<ApplicationDraftDto>(`${this.baseUrl}/${id}`);
	}

	createDraft(draft: ApplicationDraftDto): Observable<void> {
		return this.http.post<void>(this.baseUrl, draft);
	}

	saveDraft(id: number, draft: ApplicationDraftDto): Observable<void> {
		return this.http.put<void>(`${this.baseUrl}/${id}`, draft);
	}

	deleteDraft(id: number): Observable<void> {
		return this.http.delete<void>(`${this.baseUrl}/${id}`);
	}
}
