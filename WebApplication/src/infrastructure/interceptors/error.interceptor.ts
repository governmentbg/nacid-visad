import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { EMPTY, Observable, throwError as observableThrowError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { UserLoginService } from 'src/modules/user/services/user-login.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
	constructor(
		private router: Router,
		private userLoginService: UserLoginService,
		private loadingIndicatorService: LoadingIndicatorService,
		private toastr: ToastrService,
		private translateService: TranslateService
	) { }

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

		return next.handle(req).pipe(
			catchError((err: any) => {
				if (err instanceof HttpErrorResponse) {
					if (err.status === 401) {
						this.userLoginService.logout();
						this.router.navigate(['login']);

						this.loadingIndicatorService.hide();

						return EMPTY;
					} else if (err.status === 403) {
						this.toastr.error(this.translateService.instant('toastr.forbidden'));
					}

					return observableThrowError(err);
				}
			}));
	}
}
