import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { RoleService } from 'src/infrastructure/services/role.service';
import { ApplicationDto } from '../../models/application.dto';
import { ApplicationDraftDto } from '../../models/application/application-draft.dto';
import { ApplicationDraftResource } from '../../services/application-draft.resource';
import { ApplicationResource } from '../../services/application.resource';

@Component({
	selector: 'app-application-draft',
	templateUrl: './application-draft.component.html'
})
export class ApplicationDraftComponent implements OnInit {
	model = new ApplicationDto();
	canSubmit = false;
	isUniversityUser: boolean;

	copiedModel = new ApplicationDto();

	canAddPreviousApplication: boolean = true;

	private forms: { [key: string]: boolean } = {};

	commitStates = CommitState;

	constructor(
		private resource: ApplicationResource,
		private router: Router,
		private modal: NgbModal,
		private roleService: RoleService,
		private loadingIndicator: LoadingIndicatorService,
		private draftResource: ApplicationDraftResource,
		private toastrService: ToastrService,
		private activatedRoute: ActivatedRoute,
	) { }

	ngOnInit(): void {
		this.activatedRoute.data
			.subscribe((data: { model: ApplicationDraftDto }) => {
				this.model = JSON.parse(data.model.content);
				this.copiedModel = JSON.parse(data.model.content);
				this.model.state = CommitState.initialDraft;
				this.model.draftId = data.model.id;

				if (this.model.previousApplication != null && this.model.previousApplication.hasPreviousApplication == true) {
					this.canAddPreviousApplication = false;
				}
			});

		//when model is set, the document part is cleared because of educationQualification in education part
		setTimeout(() => {
			this.model.document = this.copiedModel.document;
		}, 2000);

		this.isUniversityUser = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);
	}

	createApplication(): void {
		if (!this.canSubmit) {
			return;
		}

		const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
		confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да изпратите заявлението към регистъра?';
		confirmationModal.result
			.then((result: boolean) => {
				if (result) {
					this.loadingIndicator.show();
					this.resource.createApplication(this.model)
						.pipe(
							finalize(() => this.loadingIndicator.hide())
						)
						.subscribe(() => {
							this.toastrService.success('Заявлението е изпратено успешно');
							this.router.navigate(['/application/search']);
						});
				}
			});
	}

	changeFormValidStatus(form: string, isValid: boolean): void {
		this.forms[form] = isValid;
		this.canSubmit = Object.keys(this.forms).findIndex(e => !this.forms[e]) < 0;
	}

	saveDraft(): void {
		const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
		confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да запазите данните на черновата?';
		confirmationModal.result
			.then((result: boolean) => {
				if (result) {
					const applicationDraft = new ApplicationDraftDto();
					applicationDraft.content = JSON.stringify(this.model);

					this.draftResource.saveDraft(this.model.draftId, applicationDraft).subscribe(() => {
						this.toastrService.success('Данните са запазени успешно');
						this.router.navigate(['/application', 'search']);
					});
				}
			});
	}

	deleteDraft(): void {
		const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
		confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да изтриете черновата?';
		confirmationModal.result
			.then((result: boolean) => {
				if (result) {
					this.draftResource.deleteDraft(this.model.draftId).subscribe(() => {
						this.toastrService.success('Успешно изтрита чернова');
						this.router.navigate(['/application', 'search']);
					});
				}
			});
	}

	cancel() {
		const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
		confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да излезете от страницата?';
		confirmationModal.result
			.then((result: boolean) => {
				if (result) {
					this.router.navigate(['/application', 'search']);
				}
			});
	}
}
