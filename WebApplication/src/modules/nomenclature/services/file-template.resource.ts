import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { FileTemplate } from '../models/file-template.model';

@Injectable()
export class FileTemplateResource extends BaseResource {
	constructor(
		protected http: HttpClient,
		protected configuration: Configuration
	) {
		super(http, configuration);
	}

	getall(): Observable<FileTemplate[]> {
		return this.http.get<FileTemplate[]>(`${this.baseUrl}/filetemplate`);
	}

	add(entity: FileTemplate): Observable<FileTemplate> {
		return this.http.post<FileTemplate>(`${this.baseUrl}/filetemplate`, entity);
	}

	update(entity: FileTemplate): Observable<FileTemplate> {
		return this.http.put<FileTemplate>(`${this.baseUrl}/filetemplate`, entity);
	}

	delete(id: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/filetemplate/${id}`);
	}
}
