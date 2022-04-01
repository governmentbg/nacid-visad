import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { CandidateCommitDto } from '../../models/candidate-commit.dto';
import { CandidateCreateDto } from '../../models/candidate-create.dto';
import { CandidateResource } from '../../services/candidate.resource';

@Component({
  selector: 'app-candidate-new-modal',
  templateUrl: './candidate-new-modal.component.html'
})
export class CandidateNewModalComponent implements OnInit {
  model = new CandidateCreateDto();
  canSubmit = false;
  isModalWindow = true;
  lockInput = false;

  @Input() birthDate: Date;
  @Input() country: NomenclatureDto;

  private forms: { [key: string]: boolean } = {};

  constructor(
    private resource: CandidateResource,
    private modal: NgbModal,
    public activeModal: NgbActiveModal
  ) {
  }

  ngOnInit(): void {
    this.model.candidate.birthDate = this.birthDate
    this.model.candidate.country = this.country;
  }

  save(): void {
    if (!this.canSubmit) {
      return;
    }

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да създадете нов кандидат в регистъра?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.resource.createApplicationCandidate(this.model)
            .subscribe((model: CandidateCommitDto) => {
              this.activeModal.close(model)
            });
        }
      });
  }

  closeModal() {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    const confirmationMessage = "Сигурни ли сте, че искате да излезете?";
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.activeModal.close();
        }
      });
  }

  changeFormValidStatus(form: string, isValid: boolean): void {
    this.forms[form] = isValid;
    this.canSubmit = Object.keys(this.forms).findIndex(e => !this.forms[e]) < 0;
  }
}
