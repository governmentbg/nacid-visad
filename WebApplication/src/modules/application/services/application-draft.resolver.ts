import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ApplicationDraftDto } from '../models/application/application-draft.dto';
import { ApplicationDraftResource } from './application-draft.resource';

@Injectable()
export class ApplicationDraftResolver implements Resolve<ApplicationDraftDto> {

	constructor(
		private loadingIndicator: LoadingIndicatorService,
		private draftResource: ApplicationDraftResource
	) { }

	resolve(
		route: ActivatedRouteSnapshot,
		_: RouterStateSnapshot
	): ApplicationDraftDto | Observable<ApplicationDraftDto> | Promise<ApplicationDraftDto> {
		const draftId = +route.params.draftId;
		if (isNaN(draftId)) {
			return EMPTY;
		}

		this.loadingIndicator.show();
		return this.draftResource.getDraftApplication(draftId)
			.pipe(
				catchError(() => {
					return EMPTY;
				}),
				finalize(() => this.loadingIndicator.hide())
			);
	}

}
