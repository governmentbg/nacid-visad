import { Location } from '@angular/common';
import { Component, OnInit, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { PartPanelComponent } from 'src/infrastructure/components/part-panel/part-panel.component';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { RoleService } from 'src/infrastructure/services/role.service';
import { PartsEditWarningModalComponent } from 'src/modules/application/components/parts-edit-warning-modal/parts-edit-warning-modal.component';
import { ApplicationSearchResultItemDto } from 'src/modules/application/models/application-search-result-item.dto';
import { CandidateApplicationsDto } from '../../models/candidate-applications.dto';
import { CandidateCommitDto } from '../../models/candidate-commit.dto';
import { CandidateResource } from '../../services/candidate.resource';

@Component({
  selector: 'app-candidate-commit',
  templateUrl: './candidate-commit.component.html'
})
export class CandidateCommitComponent implements OnInit {
  @ViewChildren(PartPanelComponent) parts: PartPanelComponent[] = [];

  changeStateDescription: string;
  canEditCandidate = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR, UserRoleAliases.LOT_RESULT_USER);

  constructor(
    private activatedRoute: ActivatedRoute,
    private resource: CandidateResource,
    private router: Router,
    private location: Location,
    private loadingIndicator: LoadingIndicatorService,
    private modal: NgbModal,
    private roleService: RoleService
  ) { }

  model: CandidateCommitDto;
  header: string;
  actions: { action: () => void, name: string, btnClass: string, isActive: boolean, style: string }[] = [];
  commitStates = CommitState;
  applications: ApplicationSearchResultItemDto[] = [];
  hasOtherCommits: boolean;
  canErase = this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER, UserRoleAliases.ADMINISTRATOR);

  private commitStateConfig = {
    modification: {
      header: 'В процес на редакция',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.finishModification.bind(this),
            'Сигурни ли сте, че искате да запишете промените?',
            false
          ),
          name: 'Запиши',
          btnClass: 'btn-success',
          isActive: this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER, UserRoleAliases.ADMINISTRATOR)
        },
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.cancelModification.bind(this),
            'Сигурни ли сте, че искате да откажете промените?',
            false
          ),
          name: 'Отказ',
          btnClass: 'btn-danger',
          isActive: this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER, UserRoleAliases.ADMINISTRATOR)
        }
      ]
    },
    actual: {
      header: 'Актуално състояние',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.startModification.bind(this),
            'Сигурни ли сте, че искате да редактирате записа?',
            true
          ),
          name: 'Редактирай',
          btnClass: 'btn-primary',
          isActive: this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER, UserRoleAliases.ADMINISTRATOR)
        }
      ]
    },
    actualWithModification: {
      header: 'Актуално състояние',
      actions: []
    },
    history: {
      header: 'Предишен запис',
      actions: []
    },
    deleted: {
      header: 'Изтрит',
      actions: []
    }
  };

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { dto: CandidateApplicationsDto }) => {
        this.setData(data.dto.candidateCommit);
        this.applications = data.dto.applications;
        this.hasOtherCommits = data.dto.hasOtherCommits;
      });
  }

  startModification(): void {
    this.loadingIndicator.show();
    this.resource.startModification(this.model.lotId, this.changeStateDescription)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: CandidateCommitDto) => this.setData(data));
  }

  finishModification(): void {
    const partsInEditMode = this.getPartsInEditMode();
    if (partsInEditMode.length > 0) {
      const modalRef = this.modal.open(PartsEditWarningModalComponent, { backdrop: 'static' });
      modalRef.componentInstance.partsInEditMode = partsInEditMode;
      modalRef.result.then();
      return;
    }

    this.loadingIndicator.show();
    this.resource.finishModification(this.model.lotId, this.model.candidatePart.id, this.model.candidatePart.entity)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: CandidateCommitDto) => this.setData(data));
  }

  // erase(): void {
  //   const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
  //   confirmationModal.componentInstance.showTextArea = true;
  //   confirmationModal.componentInstance.confirmationMessage = "Сигурни ли сте, че искате да изтриете записа?";
  //   confirmationModal.componentInstance.passDescription.subscribe((description: string) => {
  //     this.changeStateDescription = description;
  //   })
  //   confirmationModal.result
  //     .then((result: boolean) => {
  //       if (result) {
  //         this.loadingIndicator.show();
  //         this.resource.eraseApplication(this.model.lotId, this.changeStateDescription)
  //           .pipe(
  //             finalize(() => this.loadingIndicator.hide())
  //           )
  //           .subscribe((data: CandidateCommitDto) => this.setData(data));
  //       }
  //     });
  // }

  cancelModification(): void {
    const partsInEditMode = this.getPartsInEditMode();
    if (partsInEditMode.length > 0) {
      const modalRef = this.modal.open(PartsEditWarningModalComponent, { backdrop: 'static' });
      modalRef.componentInstance.partsInEditMode = partsInEditMode;
      modalRef.result.then();
      return;
    }

    this.loadingIndicator.show();
    this.resource.cancelModification(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: CandidateCommitDto) => this.setData(data));
  }

  revertErased(): void {
    this.loadingIndicator.show();
    this.resource.revertErasedApplication(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: CandidateCommitDto) => this.setData(data));
  }

  delete(): void {
    this.loadingIndicator.show();
    this.resource.deleteLot(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe(() => this.router.navigate(['/candidate', 'search']));
  }

  private executeActionOnConfirmation(action: () => void, confirmationMessage: string, showTextArea: boolean): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.showTextArea = showTextArea;
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;
    confirmationModal.componentInstance.passDescription.subscribe((description: string) => {
      this.changeStateDescription = description;
    })
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          action();
        }
      });
  }

  private setData(data: CandidateCommitDto): void {
    this.model = data;

    const commitConfig = this.commitStateConfig[CommitState[data.state]];
    this.header = commitConfig.header;
    this.actions = commitConfig.actions.filter(e => e.isActive);

    this.changeLocation(data.lotId, data.id);
  }

  private changeLocation(lotId: number, commitId: number): void {
    const url = this.router.createUrlTree(['/candidate', 'lot', lotId, 'commit', commitId]).toString();
    this.location.replaceState(url);
  }

  private getPartsInEditMode(): string[] {
    return this.parts
      .filter(e => e.isEditMode)
      .map(e => e.header);
  }

  goToSearch(): void {
    this.router.navigate(['/candidate/search']);
  }

  backClicked(): void {
    this.router.navigate(['/candidate/', 'lot', this.model.lotId, 'history']);
  }
}
