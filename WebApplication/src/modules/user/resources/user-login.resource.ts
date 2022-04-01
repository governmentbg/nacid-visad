import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { LoginDto } from '../models/login.dto';
import { UnauthorizedUserInfoDto } from '../models/unauthorized-user-info.dto';
import { UserLoginInfoDto } from '../models/user-login-info.dto';

@Injectable()
export class UserLoginResource {
	constructor(
		private http: HttpClient,
		private configuration: Configuration
	) { }

	getUnauthorizedUserInformation(): Observable<UnauthorizedUserInfoDto> {
		return this.http.get<UnauthorizedUserInfoDto>(`${this.configuration.restUrl}/Login`);
	}

	login(model: LoginDto): Observable<UserLoginInfoDto> {
		return this.http.post<UserLoginInfoDto>(`${this.configuration.restUrl}/Login`, model);
	}
}
