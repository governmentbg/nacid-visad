import { SignService } from '@abb/ab-docs-utils';
import { Location } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { finalize, mergeMap } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { PartPanelComponent } from '../../../../infrastructure/components/part-panel/part-panel.component';
import { CommitState } from '../../../../infrastructure/enums/commit-state.enum';
import { ApplicationLotResultType } from '../../enums/application-lot-result-type.enum';
import { ApplicationCommitDto } from '../../models/application-commit.dto';
import { ApplicationLotResultDto } from '../../models/application-lot-result.dto';
import { ApplicationResource } from '../../services/application.resource';
import { ApplicationLotResultModalComponent } from '../application-lot-result-modal/application-lot-result-modal.component';
import { PartsEditWarningModalComponent } from '../parts-edit-warning-modal/parts-edit-warning-modal.component';

@Component({
  selector: 'app-application-commit',
  templateUrl: './application-commit.component.html',
  styleUrls: ['./application-commit.component.css']
})
export class ApplicationCommitComponent implements OnInit {
  @ViewChildren(PartPanelComponent) parts: PartPanelComponent[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private resource: ApplicationResource,
    private router: Router,
    private location: Location,
    private loadingIndicator: LoadingIndicatorService,
    private modal: NgbModal,
    private roleService: RoleService,
    private signService: SignService,
    private cd: ChangeDetectorRef
  ) { }

  model: ApplicationCommitDto;
  header: string;
  changeStateDescription: string;

  actions: { action: () => void, name: string, btnClass: string, isActive: boolean }[] = [];
  commitStates = CommitState;
  resultType = ApplicationLotResultType;

  canAddResult: boolean;
  canSign: boolean = false;
  showApplicant: boolean = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR);
  isUniversity: boolean = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);
  canApprove = false;
  isSigner: boolean = this.roleService.hasRole(UserRoleAliases.RESULT_SIGNER_USER);

  private commitStateConfig = {
    modification: {
      header: 'Върнато за редакция',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.finishModification.bind(this),
            'Сигурни ли сте, че искате да изпратите промените за вписване?',
            false
          ),
          name: 'Изпрати отново',
          btnClass: 'btn-primary',
          isActive: this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER)
        }
      ]
    },
    actual: {
      header: 'Изпратено за вписване',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.startModification.bind(this),
            'Сигурни ли сте, че искате да върнете заявлението за редакция?',
            true
          ),
          name: 'Върни за редакция',
          btnClass: 'btn-primary',
          isActive: this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER)
        }
        // {
        //   action: this.executeActionOnConfirmation.bind(
        //     this,
        //     this.erase.bind(this),
        //     'Сигурни ли сте, че искате да изтриете заявлението?',
        //     true
        //   ),
        //   name: 'Изтрий заявление',
        //   btnClass: 'btn-danger',
        //   isActive: this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER)
        // }
      ]
    },
    actualWithModification: {
      header: 'Актуално състояние в процес на промяна',
      actions: []
    },
    history: {
      header: 'Предишно вписване',
      actions: []
    },
    deleted: {
      header: 'Изтрито заявление',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.revertErased.bind(this),
            'Сигурни ли сте, че искате да възстановите изтритото заявление?',
            false
          ),
          name: 'Възстановяване на заявление',
          btnClass: 'btn-primary',
          isActive: this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER)
        }
      ]
    },
    approved: {
      header: 'Одобрено',
      actions: []
    },
    annulled: {
      header: 'Анулирано',
      actions: [
      ]
    },
    refusedSign: {
      header: 'Отказано подписване',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.startModification.bind(this),
            'Сигурни ли сте, че искате да върнете заявлението за редакция?',
            true
          ),
          name: 'Върни за редакция',
          btnClass: 'btn-primary',
          isActive: this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER)
        },
      ]
    }
  };

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { commit: ApplicationCommitDto }) => this.setData(data.commit));

    this.canAddResult = this.roleService.hasRole(UserRoleAliases.RESULT_SIGNER_USER);
    this.canApprove = this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER);
    this.canSign = !this.model.lotResult.isSigned && this.roleService.hasRole(UserRoleAliases.RESULT_SIGNER_USER);
  }

  startModification(): void {
    this.loadingIndicator.show();
    this.resource.startModification(this.model.lotId, this.changeStateDescription)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => this.setData(data));
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
    this.resource.finishModification(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => {
        this.router.navigate(['/application', 'search']);
      });
  }

  erase(): void {
    this.loadingIndicator.show();
    this.resource.eraseApplication(this.model.lotId, this.changeStateDescription)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => this.setData(data));
  }

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
      .subscribe((data: ApplicationCommitDto) => this.setData(data));
  }

  revertErased(): void {
    this.loadingIndicator.show();
    this.resource.revertErasedApplication(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => this.setData(data));
  }

  delete(): void {
    this.loadingIndicator.show();
    this.resource.deleteLot(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe(() => this.router.navigate(['/application', 'search']));
  }

  addResult(): void {
    const resultModal = this.modal.open(ApplicationLotResultModalComponent, { size: 'lg' });
    resultModal.componentInstance.lotId = this.model.lotId;
    resultModal.componentInstance.commitId = this.model.id;
    resultModal.result
      .then((result: ApplicationLotResultDto | null) => {
        if (!result) {
          return;
        }

        this.model.hasResult = true;
        this.model.lotResult = result;
      });
  }

  approveApplication(): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = "Сигурни ли сте, че искате да одобрите заявлението?";

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.resource.approveApplication(this.model.lotId)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe((data: ApplicationCommitDto) => {
              this.router.navigate(['/application', 'search']);
            });
        }
      })
  }

  sign(): Subscription {
    return this.resource.getApplicationLotResultSigningInformation(this.model.lotResult.id)
      .pipe(
        mergeMap((result: any) => this.signService.signOpenXml([result], "", "", "")),
        mergeMap((content: string[]) => this.resource.updateApplicationLotResultFile(this.model.lotResult.id, content[0])),
      )
      .subscribe((model: ApplicationLotResultDto) => {
        this.setModel(model);
      });
  }

  private executeActionOnConfirmation(action: () => void, confirmationMessage: string, showTextArea: boolean): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.showTextArea = showTextArea;
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;
    confirmationModal.componentInstance.passDescription.subscribe((description: string) => {
      this.changeStateDescription = description;
    });
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          action();
        }
      });
  }

  annulment(): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = "Сигурни ли сте, че искате да анулирате заявлението?";
    confirmationModal.componentInstance.showTextArea = true;
    confirmationModal.componentInstance.passDescription.subscribe((description: string) => {
      this.changeStateDescription = description;
    });

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.resource.annulment(this.model.lotId, this.changeStateDescription)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe(() => {
              this.router.navigate(['/application', 'search']);
            });
        }
      })
  }

  refuseSign(): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = "Сигурни ли сте, че искате да откажите подписването на заявлението?";
    confirmationModal.componentInstance.showTextArea = true;
    confirmationModal.componentInstance.textAreaTitle = "Указания";
    confirmationModal.componentInstance.passDescription.subscribe((description: string) => {
      this.changeStateDescription = description;
    });

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.resource.refuseSign(this.model.lotId, this.changeStateDescription)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe(() => {
              this.router.navigate(['/application', 'search']);
            });
        }
      })
  }

  private setData(data: ApplicationCommitDto): void {
    this.model = data;

    const commitConfig = this.commitStateConfig[CommitState[data.state]];
    this.header = commitConfig.header;
    this.actions = commitConfig.actions.filter(e => e.isActive);

    this.changeLocation(data.lotId, data.id);
  }

  private setModel(model: ApplicationLotResultDto): void {
    this.model.lotResult = model;
    this.canSign = !this.model.lotResult.isSigned && this.roleService.hasRole(UserRoleAliases.RESULT_SIGNER_USER);
    this.cd.markForCheck();
  }

  private changeLocation(lotId: number, commitId: number): void {
    const url = this.router.createUrlTree(['/application', 'lot', lotId, 'commit', commitId]).toString();
    this.location.replaceState(url);
  }

  private getPartsInEditMode(): string[] {
    return this.parts
      .filter(e => e.isEditMode)
      .map(e => e.header);
  }

  goToSearch() {
    this.router.navigate(['/application', 'search']);
  }

  backClicked() {
    this.router.navigate(['/application/', 'lot', this.model.lotId, 'history']);
  }
}
