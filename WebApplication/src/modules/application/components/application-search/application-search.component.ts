import { Component } from '@angular/core';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { ApplicationLotResultTypeEnumLocalization } from 'src/modules/enum-localization.const';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ApplicationLotResultType } from '../../enums/application-lot-result-type.enum';
import { SearchFilterEndResultType } from '../../enums/search-filter-end-result-type.enum';
import { ApplicationSearchResultItemDto } from '../../models/application-search-result-item.dto';
import { ApplicationDto } from '../../models/application.dto';
import { ApplicationDraftDto } from '../../models/application/application-draft.dto';
import { ApplicationDraftResource } from '../../services/application-draft.resource';
import { ApplicationSearchFilter } from '../../services/application-search.filter';
import { ApplicationResource } from '../../services/application.resource';

@Component({
  selector: 'app-application-search',
  templateUrl: './application-search.component.html',
  styleUrls: ['./application-search.component.css']
})
export class ApplicationSearchComponent extends BaseSearchComponent<ApplicationSearchResultItemDto> {
  resultTypeLocalization = ApplicationLotResultTypeEnumLocalization;
  searchResultTypes = SearchFilterEndResultType;
  universityUserInstitution: NomenclatureDto;
  resultType = ApplicationLotResultType;
  drafts: ApplicationDto[] = [];
  isAdvancedSearch = false;

  isUniversityUser: boolean = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);
  isMonUser: boolean = this.roleService.hasRole(UserRoleAliases.LOT_RESULT_USER);
  canSign: boolean = this.roleService.hasRole(UserRoleAliases.RESULT_SIGNER_USER);

  date: Date = new Date();
  maxDate = { year: new Date().getFullYear(), month: this.date.getMonth() + 1, day: this.date.getDate() };

  ngOnInit(): void {
    this.universityUserInstitution = JSON.parse(localStorage.getItem(this.configuration.institutionProperty));

    if (this.isUniversityUser) {
      this.filter.institution = this.universityUserInstitution.name;
      this.filter.institutionId = this.universityUserInstitution.id;

      this.draftResource.getUserDraftApplications().subscribe((drafts: ApplicationDraftDto[]) => {
        drafts.forEach(draft => {
          let parsedDraft = new ApplicationDto();
          parsedDraft = JSON.parse(draft.content);
          parsedDraft.draftId = draft.id;
          this.drafts.push(parsedDraft);
        });
      });
    } else {
      this.filter.institution = null;
      this.filter.institutionId = undefined;
    }

    this.search();
  }

  constructor(
    public filter: ApplicationSearchFilter,
    protected resource: ApplicationResource,
    protected loadingIndicator: LoadingIndicatorService,
    private roleService: RoleService,
    private configuration: Configuration,
    private draftResource: ApplicationDraftResource
  ) {
    super(filter, resource, loadingIndicator);
    this.filter = new ApplicationSearchFilter();
  }

  expandAdvancedSearch(): void {
    this.isAdvancedSearch = !this.isAdvancedSearch;

    this.clearAdvancedSearch();
  }

  clearAdvancedSearch(): void {
    this.filter.candidateCountry = null;
    this.filter.candidateCountryId = null;
    this.filter.candidateBirthPlace = null;
    this.filter.fromDate = null;
    this.filter.toDate = null;
    this.filter.faculty = null;
    this.filter.facultyId = null;
    this.filter.speciality = null;
    this.filter.specialityId = null;
    this.filter.specialityName = null;
    this.filter.academicDegree = null;
    this.filter.academicDegreeId = null;
  }

  clearFilter(): void {
    if (this.isUniversityUser) {
      this.clearAdvancedSearch();

      this.filter.candidateName = null;
      this.filter.registerNumber = null;
      this.filter.endResult = null;
      super.search();
    } else {
      super.clearFilter();
    }
  }

  onSpecialityChange(speciality: any): void {
    if (this.filter.faculty) {
      return;
    }
    else {
      this.filter.faculty = new NomenclatureDto();
      this.filter.faculty = speciality.institution;
    }
  }
}
