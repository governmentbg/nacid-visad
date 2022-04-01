import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ReCaptchaV3Service } from 'ng-recaptcha';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.utils';
import { HomeResource } from './home.resource';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  accessCode: string;

  constructor(
    private toastrService: ToastrService,
    private route: ActivatedRoute,
    private resource: HomeResource,
    private reCaptcha3: ReCaptchaV3Service,
    private translateService: TranslateService,
    private loadingIndicator: LoadingIndicatorService) { }

  ngOnInit(): void {
    this.route.queryParams
      .subscribe(params => {
        if (params.accessCode !== null && params.accessCode !== undefined) {
          this.accessCode = params.accessCode;
        }
      });
  }

  submitForm() {
    this.loadingIndicator.show();

    this.reCaptcha3.execute('importantAction').subscribe((token) => {
      this.resource.getPdfUrl(this.accessCode, token)
        .pipe(
          finalize(() => {
            this.loadingIndicator.hide();
          })
        )
        .subscribe(
          (response) => {
          window.location.href = response;
        },
        (err: any) => handleDomainError(
          err,
          [
            { code: 'Application_Annulled', text: this.translateService.instant('common.applicationAnnuled') },
            { code: 'Application_InvalidAccessCode', text: this.translateService.instant('common.invalidCode') }
          ],
          this.toastrService
        ))
    })
  }

  onChange() {
  }
}
