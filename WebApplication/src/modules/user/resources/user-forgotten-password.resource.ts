import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { ForgottenPasswordDto } from '../models/forgotten-password.dto';
import { UserRecoveryPasswordDto } from '../models/user-recovery-password.dto';

@Injectable({
	providedIn: 'root'
})
export class UserForgottenPasswordResource extends BaseResource {

	constructor(
		protected http: HttpClient,
		protected configuration: Configuration
	) {
		super(http, configuration, 'ForgottenPassword')
	}

	sendForgottenPassword(model: ForgottenPasswordDto): Observable<any> {
		return this.http.post<void>(`${this.baseUrl}`, model);
	}

	recoverPassword(model: UserRecoveryPasswordDto): Observable<void> {
		return this.http.post<void>(`${this.baseUrl}/Recovery`, model);
	}
}
