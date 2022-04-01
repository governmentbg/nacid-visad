import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { RoleService } from 'src/infrastructure/services/role.service';
import { CandidateApplicationDataDto } from 'src/modules/candidate/models/candidate-application-data.dto';
import { CandidateCommitDto } from 'src/modules/candidate/models/candidate-commit.dto';
import { ApplicationDto } from '../../models/application.dto';
import { ApplicationDraftDto } from '../../models/application/application-draft.dto';
import { ApplicationDraftResource } from '../../services/application-draft.resource';
import { ApplicationResource } from '../../services/application.resource';

@Component({
  selector: 'app-application-new',
  templateUrl: './application-new.component.html'
})
export class ApplicationNewComponent implements OnInit {
  model = new ApplicationDto();
  canSubmit = false;
  isUniversityUser: boolean;

  canAddPreviousApplication: boolean = true;

  private forms: { [key: string]: boolean } = {};

  commitStates = CommitState;

  constructor(
    private resource: ApplicationResource,
    private router: Router,
    private modal: NgbModal,
    private roleService: RoleService,
    private loadingIndicator: LoadingIndicatorService,
    private draftResource: ApplicationDraftResource,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.getApplicantData();
    this.isUniversityUser = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);
  }

  setCandidateCommit(candidateCommit: CandidateCommitDto): void {
    this.model.candidateCommitId = candidateCommit.id;
    this.model.candidateCommit = candidateCommit;
  }

  setCandidateApplicationData(candidateApplicationData: CandidateApplicationDataDto): void {
    if (candidateApplicationData.previousApplication != null && candidateApplicationData.previousApplication.hasPreviousApplication == true) {
      this.model.previousApplication = candidateApplicationData.previousApplication;
      this.canAddPreviousApplication = false;
    }

    if (candidateApplicationData.diploma != null && candidateApplicationData.diploma.diplomaFiles.length > 0) {
      this.model.diploma = candidateApplicationData.diploma;

      this.model.diploma.diplomaFiles.forEach(diploma => {
        diploma.disabled = true;
      });
    }
  }

  createApplication(): void {
    if (!this.canSubmit) {
      return;
    }

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да изпратите заявлението към регистъра?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.resource.createApplication(this.model)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe(() => {
              this.toastrService.success('Заявлението е изпратено успешно');
              this.router.navigate(['/application/search']);
            });
        }
      });
  }

  changeFormValidStatus(form: string, isValid: boolean): void {
    this.forms[form] = isValid;
    this.canSubmit = Object.keys(this.forms).findIndex(e => !this.forms[e]) < 0;
  }

  getApplicantData() {
    this.resource.getApplicantData()
      .subscribe(data => {
        this.model.applicant.firstName = data.firstName;
        this.model.applicant.lastName = data.lastName;
        this.model.applicant.mail = data.mail;
        this.model.applicant.position = data.position;
        this.model.applicant.institution = data.institution;
        this.model.applicant.phone = data.phone
        if (data.middleName !== null) {
          this.model.applicant.middleName = data.middleName
        }
      });
  }

  createDraft(): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да запазите данните в чернова?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          const applicationDraft = new ApplicationDraftDto();
          applicationDraft.content = JSON.stringify(this.model);

          this.draftResource.createDraft(applicationDraft).subscribe(() => {
            this.toastrService.success('Данните са записани успешно');
            this.router.navigate(['/application', 'search']);
          });
        }
      });
  }

  cancel() {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да излезете от страницата?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.router.navigate(['/application', 'search']);
        }
      });
  }
}
