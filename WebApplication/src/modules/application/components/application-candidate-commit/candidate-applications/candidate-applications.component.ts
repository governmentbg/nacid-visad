import { Component, Input, OnInit } from '@angular/core';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { ApplicationLotResultType } from 'src/modules/application/enums/application-lot-result-type.enum';
import { ApplicationLotResultTypeEnumLocalization } from 'src/modules/enum-localization.const';

@Component({
	selector: 'app-candidate-applications',
	templateUrl: 'candidate-applications.component.html',
	styles: ['.load-more-wrapper{ display: flex; justify-content: center;}']
})

export class CandidateApplicationsComponent implements OnInit {
	@Input() applications: any[] = [];
	application: any;
	resultTypeLocalization = ApplicationLotResultTypeEnumLocalization;
	resultType = ApplicationLotResultType;
	isMonUser = false;

	constructor(private roleService: RoleService) {
	}

	ngOnInit(): void {
		this.isMonUser = this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER);

		if (this.applications.length > 0) {
			this.application = this.applications[0];
		}

	}
}
