import { Component, OnInit } from '@angular/core';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { CandidateSearchResultItemDto } from '../../models/candidate-search-result-item.dto';
import { CandidateSearchFilter } from '../../services/candidate-search.filter';
import { CandidateResource } from '../../services/candidate.resource';

@Component({
  selector: 'app-candidate-search',
  templateUrl: './candidate-search.component.html',
  styleUrls: ['./candidate-search.component.css']
})
export class CandidateSearchComponent extends BaseSearchComponent<CandidateSearchResultItemDto> implements OnInit {
  canAddNewCandidate: boolean;

  date: Date = new Date();
  maxDate = { year: new Date().getFullYear(), month: this.date.getMonth() + 1, day: this.date.getDate() };

  ngOnInit(): void {
    this.canAddNewCandidate = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR, UserRoleAliases.UNIVERSITY_USER);
    this.search();
  }

  constructor(
    public filter: CandidateSearchFilter,
    protected resource: CandidateResource,
    protected loadingIndicator: LoadingIndicatorService,
    private roleService: RoleService,
    public sharedService: SharedService
  ) {
    super(filter, resource, loadingIndicator);
    this.filter = new CandidateSearchFilter();
  }
}
