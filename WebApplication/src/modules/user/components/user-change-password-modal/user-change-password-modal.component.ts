import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.util';
import { UserChangePasswordDto } from '../../models/user-change-password.dto';
import { UserResource } from '../../resources/user.resource';

@Component({
  selector: 'app-user-change-password-modal',
  templateUrl: './user-change-password-modal.component.html',
  styleUrls: ['./user-change-password-modal.component.css']
})
export class UserChangePasswordModalComponent extends CommonFormComponent<UserChangePasswordDto> {
  model: UserChangePasswordDto = new UserChangePasswordDto();

  arePasswordsEqual: boolean = false;

  passwordRegex = RegExps.PASSWORD_REGEX;

  constructor(
    private resource: UserResource,
    private modalClass: NgbActiveModal,
    private toastrService: ToastrService,
    private translateService: TranslateService) {
    super();

    this.validationTexts = [...this.validationTexts, { key: 'pattern', value: this.translateService.instant('validations.password') }];
  }

  change(): void {
    if (this.model.newPassword === this.model.newPasswordAgain) {
      this.resource.changePassword(this.model)
        .subscribe(() => {
          this.toastrService.success(this.translateService.instant('toastr.userChangePassword'));
          this.close();
        },
          (err) => handleDomainError(
            err,
            [
              { code: 'User_ChangePasswordOldPasswordMismatch', text: this.translateService.instant('toastr.userInvalidOldPassword'), timeout: 5000 },
              { code: 'User_ChangePasswordNewPasswordMismatch', text: this.translateService.instant('toastr.userChangePasswordNewPasswordMismatch'), timeout: 5000 }
            ],
            this.toastrService
          )
        );
    } else {
      this.toastrService.error(this.translateService.instant('toastr.userPasswordsDoesNotMatch'));
    }
  }

  comparePasswords(newPassword: string): void {
    if (this.model.newPassword != newPassword) {
      this.arePasswordsEqual = false;
    }
    else {
      this.arePasswordsEqual = true;
    }
  }

  close(): void {
    this.modalClass.close();
  }
}
