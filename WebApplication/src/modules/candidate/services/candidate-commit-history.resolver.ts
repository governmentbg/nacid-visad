import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { CandidateLotHistoryDto } from '../models/candidate-lot-history.dto';
import { CandidateResource } from './candidate.resource';

@Injectable()
export class CandidateCommitHistoryResolver implements Resolve<CandidateLotHistoryDto> {

  constructor(
    private resource: CandidateResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ): CandidateLotHistoryDto | Observable<CandidateLotHistoryDto> | Promise<CandidateLotHistoryDto> {
    const lotId = +route.params.lotId;
    if (isNaN(lotId)) {
      return EMPTY;
    }

    this.loadingIndicator.show();
    return this.resource.getHistory(lotId)
      .pipe(
        catchError(() => {
          return EMPTY;
        }),
        finalize(() => this.loadingIndicator.hide())
      );
  }

}
