import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, throwError as observableThrowError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserLoginService } from 'src/modules/user/services/user-login.service';
import { Configuration } from '../configuration/configuration';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

	constructor(
		private router: Router,
		private configuration: Configuration,
		private userLoginService: UserLoginService
	) { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		const accessToken = localStorage.getItem(this.configuration.tokenProperty);

		// IE caches get requests
		const isIEOrEdge = /msie\s|trident\/|edge\//i.test(window.navigator.userAgent);
		if (isIEOrEdge && request.method === 'GET') {
			request = request.clone({
				setHeaders: {
					'Cache-Control': 'no-cache',
					Pragma: 'no-cache'
				}
			});
		}

		request = request.clone({
			setHeaders: {
				Authorization: `Bearer ${accessToken}`
			}
		});

		return next.handle(request).pipe(
			catchError((err: any) => this.handleAuthError(err, this.router))
		);
	}

	private handleAuthError(err: HttpErrorResponse, router: Router): Observable<any> {
		if (err.status === 401) {
			this.userLoginService.logout();
			return of(err.message);
		}

		return observableThrowError(err);
	}
}
