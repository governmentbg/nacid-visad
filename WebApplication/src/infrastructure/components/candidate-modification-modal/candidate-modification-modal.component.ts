import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
	selector: 'app-candidate-modification-modal',
	templateUrl: 'candidate-modification-modal.component.html'
})

export class CandidateModificationModalComponent {
	@Input() confirmationMessage: string;

	constructor(public modal: NgbActiveModal) { }
}
