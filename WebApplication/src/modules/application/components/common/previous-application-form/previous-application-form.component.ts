import { Component, Input } from '@angular/core';
import { RegExps } from 'src/infrastructure/constants/constants';
import { PreviousApplicationDto } from 'src/modules/application/models/previous-application.dto';
import { CandidateResource } from 'src/modules/candidate/services/candidate.resource';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
	selector: 'app-previous-application-form',
	templateUrl: './previous-application-form.component.html',
})
export class PreviousApplicationFormComponent extends CommonFormComponent<PreviousApplicationDto> {
	yearRegex = RegExps.NUMBER_REGEX;

	@Input() canAddPreviousApplication: boolean = false;
	@Input() lotId: number;
	@Input() applicationCommitId: number;

	maxYear = new Date().getFullYear();

	constructor(
		protected resource: CandidateResource) {
		super()
	}

	togglePreviousApplication(hasPreviousApplication: boolean): void {
		this.model.hasPreviousApplication = hasPreviousApplication;
		if (!hasPreviousApplication) {
			this.model.hasPreviousApplication = false;
		}
	}

	filterYear(event: any, model: string, shouldFilter = true) {
		if (!shouldFilter) {
			return;
		}
		const pattern = new RegExp(RegExps.NUMBER_REGEX);
		const inputChar = String.fromCharCode(event.charCode);

		if ((!model || model.length === 0) && inputChar === '+') {
			return;
		} else {
			if (!pattern.test(inputChar)) {
				event.preventDefault();
			}
		}
	}
}
