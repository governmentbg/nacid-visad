import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { UserActivationDto } from '../models/user-activation.dto';

@Injectable()
export class UserActivationResource extends BaseResource {
	constructor(protected http: HttpClient,
		protected configuration: Configuration) {
		super(http, configuration, 'Activation');
	}

	activateUser(model: UserActivationDto): Observable<any> {
		return this.http.post<any>(`${this.baseUrl}`, model);
	}

	sendActivationLink(userId: number): Observable<void> {
		return this.http.get<void>(`${this.baseUrl}/userActivation?userId=${userId}`);
	}

	checkToken(token: string): Observable<void> {
		return this.http.get<void>(`${this.baseUrl}?token=${token}`)
	}
}
