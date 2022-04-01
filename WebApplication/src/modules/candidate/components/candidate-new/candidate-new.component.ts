import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { CommitInfoDto } from 'src/modules/application/models/commit-info.dto';
import { CandidateCreateDto } from '../../models/candidate-create.dto';
import { CandidateResource } from '../../services/candidate.resource';

@Component({
  selector: 'app-candidate-new',
  templateUrl: './candidate-new.component.html'
})
export class CandidateNewComponent {
  model = new CandidateCreateDto();
  canSubmit = false;

  private forms: { [key: string]: boolean } = {};

  constructor(
    private resource: CandidateResource,
    private router: Router,
    private modal: NgbModal
  ) { }

  save(): void {
    if (!this.canSubmit) {
      return;
    }

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да създадете запис в регистъра?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.resource.createCandidate(this.model)
            .subscribe((model: CommitInfoDto) => this.router.navigate(['/candidate', 'lot', model.lotId, 'commit', model.commitId]));
        }
      });
  }

  changeFormValidStatus(form: string, isValid: boolean): void {
    this.forms[form] = isValid;
    this.canSubmit = Object.keys(this.forms).findIndex(e => !this.forms[e]) < 0;
  }

  close() {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    const confirmationMessage = "Сигурни ли сте, че искате да излезете?";
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.router.navigate(['/application', 'search'])
        }
      });
  }
}
