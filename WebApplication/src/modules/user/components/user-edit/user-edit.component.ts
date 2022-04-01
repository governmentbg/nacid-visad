import { Location } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps, UserRoleAliases } from 'src/infrastructure/constants/constants';
import { DomainError } from 'src/infrastructure/models/domain-error.model';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { UserStatus } from '../../enums/user-status.enum';
import { Role } from '../../models/role.dto';
import { UserEditDto } from '../../models/user-edit.dto';
import { RoleResource } from '../../resources/role.resource';
import { UserActivationResource } from '../../resources/user-activation.resource';
import { UserResource } from '../../resources/user.resource';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html'
})
export class UserEditComponent extends CommonFormComponent<UserEditDto> implements OnInit {
  model: UserEditDto = new UserEditDto();
  private originalModel: UserEditDto;

  roles$: Observable<Role[]>;
  userStatus = UserStatus;
  isEditMode = false;

  universityUser = UserRoleAliases.UNIVERSITY_USER;
  emailRegex = RegExps.EMAIL_REGEX;
  cyrillicRegExp = RegExps.CYRILLIC_NAMES_REGEX;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    private activationResource: UserActivationResource,
    private loadingIndicator: LoadingIndicatorService,
    private userResource: UserResource,
    private roleResource: RoleResource,
    private translateService: TranslateService,
    public sharedService: SharedService,
    private modal: NgbModal,
    private location: Location
  ) {
    super();

    this.validationTexts = [...this.validationTexts, { key: 'pattern', value: this.translateService.instant('validations.email') }];
  }

  ngOnInit(): void {
    this.roles$ = this.roleResource.getRoles();

    this.route.params.subscribe((params: Params) => {
      const userId = +params.id;
      if (isNaN(userId) || !userId) {
        this.router.navigate(['']);
      }

      this.loadingIndicator.show();
      this.userResource.getUserDtoById(userId)
        .pipe(
          finalize(() => this.loadingIndicator.hide())
        )
        .subscribe((model: UserEditDto) => this.model = model);
    });
  }

  roleCompare(role: Role, newRole: Role): boolean {
    return role?.id === newRole?.id;
  }

  edit(): void {
    this.originalModel = JSON.parse(JSON.stringify(this.model));
    this.isEditMode = true;
  }

  cancelEdit(): void {
    this.model = JSON.parse(JSON.stringify(this.originalModel));
    this.finishEdit();
  }

  changeUserActiveStatus(): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = this.model.status == this.userStatus.active ?
      "Сигурни ли сте, че искате да деактивирате потребителя?" : "Сигурни ли сте, че искате да активирате потребителя?";

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.userResource.changeUserActiveStatus(this.model.id)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe((status: UserStatus) => {
              this.model.status = status;
              this.finishEdit();
            });
        }
      });
  }

  save(): void {
    this.loadingIndicator.show();
    this.userResource.editUserData(this.model)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe(() => {
        this.toastrService.success(this.translateService.instant('toastr.userEditSuccess'));
        this.finishEdit();
      }, (err) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 422) {
            const data = err.error as DomainError;

            for (let i = 0; i <= data.errorMessages.length - 1; i++) {
              const domainErrorCode = data.errorMessages[i].domainErrorCode;

              if (domainErrorCode === 'User_EmailTaken') {
                this.toastrService.error(this.translateService.instant('toastr.userEmailTaken'));
              }
            }
          }
        }
      });
  }

  sendActivationLink(userId: number) {
    this.activationResource.sendActivationLink(userId).subscribe(() => {
      this.toastrService.success(this.translateService.instant('toastr.userActivationLink'))
    })
  }

  private finishEdit(): void {
    this.originalModel = null;
    this.isEditMode = false;
  }

  backClicked() {
    this.location.back();
  }
}
