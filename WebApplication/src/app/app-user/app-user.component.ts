import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { UserChangePasswordModalComponent } from 'src/modules/user/components/user-change-password-modal/user-change-password-modal.component';
import { UserLoginService } from 'src/modules/user/services/user-login.service';

@Component({
	selector: 'app-user',
	templateUrl: './app-user.component.html',
	styles: [
		`
		:host {
			font-weight: 600;
			cursor: pointer;
		}
		`
	]
})
export class AppUserComponent implements OnInit {
	fullname: string;

	constructor(
		private configuration: Configuration,
		private userLoginService: UserLoginService,
		private router: Router,
		private modal: NgbModal,
		config: NgbModalConfig
	) {
		config.backdrop = 'static';
		config.keyboard = false;
	}

	ngOnInit(): void {
		this.fullname = localStorage.getItem(this.configuration.fullnameProperty);
	}

	open() {
		this.modal.open(UserChangePasswordModalComponent, { centered: true });
	}

	logout(): void {
		const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
		const confirmationMessage = "Сигурни ли сте, че искате да излезете от системата?";
		confirmationModal.componentInstance.confirmationMessage = confirmationMessage;
		confirmationModal.result
			.then((result: boolean) => {
				if (result) {
					this.userLoginService.logout();
					this.router.navigate(['login']);
				}
			});
	}
}
