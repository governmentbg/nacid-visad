import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.util';
import { LoginDto } from '../models/login.dto';
import { UserLoginInfoDto } from '../models/user-login-info.dto';
import { UserLoginResource } from '../resources/user-login.resource';
import { UserLoginService } from '../services/user-login.service';

@Component({
  selector: 'user-login',
  templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {
  model: LoginDto = new LoginDto();
  hasInvalidCredentials = false;

  constructor(
    private router: Router,
    private toastrService: ToastrService,
    private resource: UserLoginResource,
    private userLoginService: UserLoginService,
    private loadingIndicator: LoadingIndicatorService,
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    if (this.userLoginService.isLogged) {
      this.router.navigate(['']);
    }
  }

  login(): void {
    this.loadingIndicator.show();
    this.resource.login(this.model)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe(
        (userLoginInfoDto: UserLoginInfoDto) => {
          this.userLoginService.login(userLoginInfoDto);
          this.router.navigate(['']);
        },
        (err) => handleDomainError(
          err,
          [
            { code: 'User_InvalidCredentials', text: this.translateService.instant('user.login.invalidCredentials') },
            { code: 'User_UserDeactivatedOrLocked', text: this.translateService.instant('user.login.deactivated') }
          ],
          this.toastrService
        )
      );
  }
}
