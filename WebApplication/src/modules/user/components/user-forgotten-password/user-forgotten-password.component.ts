import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.util';
import { ForgottenPasswordDto } from '../../models/forgotten-password.dto';
import { UserForgottenPasswordResource } from '../../resources/user-forgotten-password.resource';

@Component({
  selector: 'app-user-forgotten-password',
  templateUrl: './user-forgotten-password.component.html'
})
export class UserForgottenPasswordComponent extends CommonFormComponent<ForgottenPasswordDto> {
  model: ForgottenPasswordDto = new ForgottenPasswordDto();

  emailRegex = RegExps.EMAIL_REGEX;

  constructor(
    private resource: UserForgottenPasswordResource,
    private toastrService: ToastrService,
    private loadingIndicator: LoadingIndicatorService,
    private translateService: TranslateService,
    private router: Router
  ) {
    super();

    this.validationTexts = [...this.validationTexts, { key: 'pattern', value: this.translateService.instant('validations.email') }];
  }

  sendForgottenPassword(): void {
    this.loadingIndicator.show();

    this.resource.sendForgottenPassword(this.model)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe(() => {
        this.toastrService.success(this.translateService.instant('toastr.userLinkForActivation'));
        this.router.navigate(['/login']);

        this.model = new ForgottenPasswordDto();
      },
        (err) => handleDomainError(
          err,
          [
            { code: 'User_InvalidCredentials', text: this.translateService.instant('toastr.userInvalidEmail'), timeout: 5000 },
            { code: 'User_CannotRestoreUserPassword', text: this.translateService.instant('toastr.userDeactivated'), timeout: 5000 }
          ],
          this.toastrService
        )
      );
  }
}
