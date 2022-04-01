import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { CandidateApplicationsDto } from '../models/candidate-applications.dto';
import { CandidateResource } from './candidate.resource';

@Injectable()
export class CandidateCommitResolver implements Resolve<CandidateApplicationsDto> {

  constructor(
    private resource: CandidateResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ): CandidateApplicationsDto | Observable<CandidateApplicationsDto> | Promise<CandidateApplicationsDto> {
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
