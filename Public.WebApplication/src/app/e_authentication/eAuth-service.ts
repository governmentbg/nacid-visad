import { Injectable } from '@angular/core';
import { EAuthSendRequestComponent } from './eAuth-send-request.component';
import { SamlRequestService } from './saml-request.service';
import { UsernameUpdateService } from './username-update-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration } from 'src/infrastructure/configuration/configuration';

@Injectable({
  providedIn: 'root'
})
export class EAuthService {

  constructor(
    private modalService: NgbModal,
    private samlRequestService: SamlRequestService,
    private usernameUpdateService: UsernameUpdateService,
    private config: Configuration) {
  }

  authenticate(serviceType: string, serviceUrl: string, serviceAction: string) {
    sessionStorage.setItem('serviceUrl', serviceUrl);
    sessionStorage.setItem('serviceAction', serviceAction);
    this.samlRequestService.getSamlRequest(serviceType, this.config.portalAlias)
      .subscribe(data => {
        const modalInstance = this.modalService.open(EAuthSendRequestComponent);
        modalInstance.componentInstance.samlRequest = data;
      });
  }

  isUserLoggedIn() {
    return sessionStorage.getItem('userName') !== null;
  }

  logOut() {
    sessionStorage.removeItem('userName');
    this.usernameUpdateService.emit(undefined);
  }
}