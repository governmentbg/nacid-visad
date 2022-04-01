import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.util';
import { UserActivationResource } from '../resources/user-activation.resource';
import { UserLoginService } from './user-login.service';

@Injectable()
export class UserTokenGuard implements CanActivate {
	constructor(
		private router: Router,
		private config: Configuration,
		private userService: UserLoginService,
		private loadingIndicator: LoadingIndicatorService,
		private resource: UserActivationResource,
		private translateService: TranslateService,
		private toastrService: ToastrService
	) { }

	canActivate(route: ActivatedRouteSnapshot, _: RouterStateSnapshot): boolean {
		this.resource.checkToken(route.queryParams.token)
			.pipe(finalize(() => this.loadingIndicator.hide()))
			.subscribe(() => { },
				(err) => {
					this.router.navigate(['']);

					handleDomainError(
						err,
						[
							{ code: 'User_ActivationTokenAlreadyUsed', text: this.translateService.instant('toastr.userActivationTokenAlreadyUsed'), timeout: 15000 },
							{ code: 'User_TokenExpired', text: this.translateService.instant('toastr.userTokenExpired'), timeout: 15000 }
						],
						this.toastrService
					);
				});

		const params = route.queryParams;

		const token = localStorage.getItem(this.config.tokenProperty);
		if (token) {
			this.userService.logout();
		}

		if (params && params['token']) {
			return true;
		}

		this.router.navigate[''];
		return false;
	}
}
