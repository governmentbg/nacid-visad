import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationLotResultTypeEnumLocalization, CommitStateEnumLocalization } from 'src/modules/enum-localization.const';
import { ApplicationLotResultType } from '../../enums/application-lot-result-type.enum';
import { ApplicationLotHistoryDto } from '../../models/application-lot-history.dto';

@Component({
  selector: 'app-application-commit-history',
  templateUrl: './application-commit-history.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ApplicationCommitHistoryComponent implements OnInit {
  model: ApplicationLotHistoryDto;

  enumLocalization = CommitStateEnumLocalization;
  lotId: number;

  resultType = ApplicationLotResultType;
  resultTypeLocalization = ApplicationLotResultTypeEnumLocalization;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { model: ApplicationLotHistoryDto }) => this.model = data.model);

    this.lotId = +this.activatedRoute.snapshot.paramMap.get('lotId');
  }

  backClicked() {
    this.router.navigate(['/application', 'lot', this.lotId, 'commit', this.model.actualCommitId]);
  }
}
