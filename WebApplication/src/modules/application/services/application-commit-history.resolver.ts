import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ApplicationLotHistoryDto } from '../models/application-lot-history.dto';
import { ApplicationResource } from './application.resource';

@Injectable()
export class ApplicationCommitHistoryResolver implements Resolve<ApplicationLotHistoryDto> {

  constructor(
    private resource: ApplicationResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ): ApplicationLotHistoryDto | Observable<ApplicationLotHistoryDto> | Promise<ApplicationLotHistoryDto> {
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
