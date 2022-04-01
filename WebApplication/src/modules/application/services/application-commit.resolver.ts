import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ApplicationCommitDto } from '../models/application-commit.dto';
import { ApplicationResource } from './application.resource';

@Injectable()
export class ApplicationCommitResolver implements Resolve<ApplicationCommitDto> {

  constructor(
    private resource: ApplicationResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ): ApplicationCommitDto | Observable<ApplicationCommitDto> | Promise<ApplicationCommitDto> {
    const lotId = +route.params.lotId;
    const commitId = +route.params.commitId;
    if (isNaN(lotId) || isNaN(commitId)) {
      return EMPTY;
    }

    this.loadingIndicator.show();
    return this.resource.getCommit(lotId, commitId)
      .pipe(
        catchError(() => {
          return EMPTY;
        }),
        finalize(() => this.loadingIndicator.hide())
      );
  }

}
