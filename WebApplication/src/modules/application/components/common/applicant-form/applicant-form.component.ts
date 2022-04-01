import { Component } from '@angular/core';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { ApplicantDto } from 'src/modules/application/models/applicant.dto';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
  selector: 'app-applicant-form',
  templateUrl: './applicant-form.component.html'
})
export class ApplicantFormComponent extends CommonFormComponent<ApplicantDto> {
  emailRegExp = `[^@]+@[^@]+\.[a-zA-Z]{2,}`;
  canChooseInstitution: boolean;

  ngOnInit() {
    this.canChooseInstitution = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);
  }

  constructor(private roleService: RoleService) {
    super();

    this.validationTexts = [...this.validationTexts, { key: 'pattern', value: 'Моля, попълнете валиден адрес на ел. поща' }];
  }
}
