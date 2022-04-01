import { Component, Input } from '@angular/core';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { MedicalCertificateDto } from 'src/modules/application/models/medical-certificate.to';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
	selector: 'app-medical-certificate-form',
	templateUrl: './medical-certificate-form.component.html',
})
export class MedicalCertificateComponent extends CommonFormComponent<MedicalCertificateDto>{
	@Input('submissionDate') set submissiondate(date: Date) {
		if (date != null) {
			let submissionDate = new Date(date);
			this.maxissuedDate = { year: submissionDate.getFullYear(), month: submissionDate.getMonth() + 1, day: submissionDate.getDate() };
			if (submissionDate.getMonth() == 0) {
				this.minissuedDate = { year: submissionDate.getFullYear() - 1, month: 12, day: submissionDate.getDate() - 1 };
			}
			else {
				this.minissuedDate = { year: submissionDate.getFullYear(), month: submissionDate.getMonth(), day: submissionDate.getDate() - 1 };
			}
		}
	}

	date = new Date();
	maxissuedDate = { year: this.date.getFullYear(), month: this.date.getMonth() + 1, day: this.date.getDate() };
	minissuedDate = { year: this.date.getFullYear(), month: this.date.getMonth(), day: this.date.getDate() - 1 };

	constructor(public sharedService: SharedService) {
		super()
	}
}
