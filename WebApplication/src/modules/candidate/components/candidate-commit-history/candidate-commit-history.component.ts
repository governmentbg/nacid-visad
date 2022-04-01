import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommitStateEnumLocalization } from 'src/modules/enum-localization.const';
import { CandidateLotHistoryDto } from '../../models/candidate-lot-history.dto';

@Component({
  selector: 'app-candidate-commit-history',
  templateUrl: './candidate-commit-history.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CandidateCommitHistoryComponent implements OnInit {
  model: CandidateLotHistoryDto;

  enumLocalization = CommitStateEnumLocalization;
  lotId: number;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { model: CandidateLotHistoryDto }) => this.model = data.model);

    this.lotId = +this.activatedRoute.snapshot.paramMap.get('lotId');
  }

  backClicked() {
    this.router.navigate(['/candidate', 'lot', this.lotId, 'commit', this.model.actualCommitId]);
  }
}
