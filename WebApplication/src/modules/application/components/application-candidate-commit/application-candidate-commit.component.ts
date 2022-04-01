import { Component, EventEmitter, Input, Output } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { CandidateCommitDto } from 'src/modules/candidate/models/candidate-commit.dto';
import { CandidateDto } from 'src/modules/candidate/models/candidate.dto';
import { CandidateResource } from 'src/modules/candidate/services/candidate.resource';

@Component({
  selector: 'app-application-candidate-commit',
  templateUrl: './application-candidate-commit.component.html'
})
export class ApplicationCandidateCommitComponent {
  @Input() model: CandidateCommitDto = new CandidateCommitDto();
  @Input() canEdit: boolean = false;
  @Input() applicationCommitId = 0;

  applicationCommitState: CommitState;
  @Input('applicationCommitState') set applicationCommitStateSetter(state: CommitState) {
    if (!state) {
      return;
    }

    this.applicationCommitState = state;
    this.canStartModification = state === CommitState.initialDraft || state === CommitState.modification;
  }

  @Output() candidateCommitChanged: EventEmitter<number> = new EventEmitter();

  isEditMode = false;
  hasValidData = false;
  canStartModification = false;

  private originalObject: CandidateDto;


  constructor(
    private resource: CandidateResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  edit(): void {
    this.isEditMode = true;
    this.originalObject = JSON.parse(JSON.stringify(this.model.candidatePart.entity));
  }

  cancelEdit(): void {
    this.isEditMode = false;
    this.model.candidatePart.entity = JSON.parse(JSON.stringify(this.originalObject));
    this.originalObject = null;
  }

  save(): void {
    if (!this.hasValidData) {
      return;
    }

    this.loadingIndicator.show();
    this.resource.finishApplicationCommitModification(this.model.id, this.model.candidatePart.entity, this.applicationCommitId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((model: CandidateCommitDto) => {
        this.isEditMode = false;
        this.originalObject = null;
        this.model = model;
        this.candidateCommitChanged.emit(this.model.id);
      });
  }
}
