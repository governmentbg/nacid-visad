import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EAuthResponseStatus } from './eAuth-response-status.enum';
import { UsernameUpdateService } from './username-update-service';

@Component({
  selector: 'eauth-redirect-response',
  template: '<h3>{{ errorMessage }}</h3>',
  styles: ['h3 { text-align: center}']
})
export class EAuthRedirectResponseComponent implements OnInit {

  responseStatus: any;
  userName: string;
  errorMessage: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private usernameUpdateService: UsernameUpdateService) {
  }

  ngOnInit() {
    this.responseStatus = EAuthResponseStatus[this.route.snapshot.queryParams.responseStatus];
    this.userName = this.route.snapshot.queryParams.name;

    if (this.responseStatus === EAuthResponseStatus.Success) {
      const serviceUrl = sessionStorage.getItem('serviceUrl');
      const serviceAction = sessionStorage.getItem('serviceAction');

      sessionStorage.setItem('userName', this.userName);
      this.usernameUpdateService.emit(this.userName);
      this.router.navigate([serviceUrl], { queryParams: { action: serviceAction } });
    } else {
      this.setErrorMessage();
    }
  }

  setErrorMessage() {
    if (this.responseStatus === EAuthResponseStatus.AuthenticationFailed) {
      this.errorMessage = 'Проблем при автентикацията';
    }
    if (this.responseStatus === EAuthResponseStatus.CanceledByUser) {
      this.errorMessage = 'Процеса е прекъснат от потребител';
    }
    if (this.responseStatus === EAuthResponseStatus.InvalidResponseXML) {
      this.errorMessage = 'Грешка в генерирания XML';
    }
    if (this.responseStatus === EAuthResponseStatus.InvalidSignature) {
      this.errorMessage = 'Невалиден подпис';
    }
    if (this.responseStatus === EAuthResponseStatus.MissingEGN) {
      this.errorMessage = 'Липсва ЕГН';
    }
    if (this.responseStatus === EAuthResponseStatus.NotDetectedQES) {
      this.errorMessage = 'Подписът не е КЕП';
    }
  }
}
