import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps, UserRoleAliases } from 'src/infrastructure/constants/constants';
import { DomainError } from 'src/infrastructure/models/domain-error.model';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { Role } from '../../models/role.dto';
import { UserDto } from '../../models/user-create.dto';
import { RoleResource } from '../../resources/role.resource';
import { UserResource } from '../../resources/user.resource';

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html'
})
export class UserCreateComponent extends CommonFormComponent<UserDto> implements OnInit {
  model: UserDto = new UserDto();
  selectedRole: Role;
  emailRegex = RegExps.EMAIL_REGEX;
  showInstitutions: boolean = false;
  cyrillicRegExp = RegExps.CYRILLIC_NAMES_REGEX;

  roles$: Observable<Role[]>;

  constructor(
    private userResource: UserResource,
    private router: Router,
    private roleResource: RoleResource,
    private toastrService: ToastrService,
    private translateService: TranslateService,
    private modal: NgbModal,
    public sharedService: SharedService
  ) {
    super();

    this.validationTexts = [...this.validationTexts, { key: 'pattern', value: this.translateService.instant('validations.email') }];
  }

  ngOnInit(): void {
    this.roles$ = this.roleResource.getRoles();
  }

  create(): void {
    this.model.username = this.model.email;
    this.model.roleId = this.selectedRole.id;
    this.model.roleAlias = this.selectedRole.alias;

    this.userResource.create(this.model)
      .subscribe(() => {
        this.router.navigate(['user/search']);
        this.toastrService.success('Успешно създаден нов потребител, изпратен активационен линк', null, { timeOut: 10000 });
      },
        (err) => {
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
        }
      );
  }

  cancel(): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    const confirmationMessage = "Сигурни ли сте, че искате да излезете?";
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.router.navigate(['user/search']);
        }
      });
  }

  selectRole(role: Role) {
    let institutionName = "";

    if (role.alias === UserRoleAliases.UNIVERSITY_USER) {
      this.model.institution = new NomenclatureDto();
      this.model.institution = null;
      this.showInstitutions = true;
    }
    else if (role.alias === UserRoleAliases.ADMINISTRATOR) {
      institutionName = "НАЦИД";
      this.userResource.getUserInstitution(institutionName).subscribe((institution: NomenclatureDto) => {
        this.model.institution = institution;
        this.showInstitutions = true;
      });
    }
    else if (role.alias === UserRoleAliases.RESULT_SIGNER_USER || role.alias === UserRoleAliases.LOT_RESULT_USER) {
      institutionName = "МОН";
      this.userResource.getUserInstitution(institutionName).subscribe((institution: NomenclatureDto) => {
        this.model.institution = institution;
        this.showInstitutions = true;
      });
    }
    else if (role.alias === UserRoleAliases.CONTROL_USER) {
      institutionName = role.name;
      this.userResource.getUserInstitution(institutionName).subscribe((institution: NomenclatureDto) => {
        this.model.institution = institution;
        this.showInstitutions = true;
      });
    }
    else {
      this.model.institution = null;
      this.showInstitutions = false
    }
  }
}
