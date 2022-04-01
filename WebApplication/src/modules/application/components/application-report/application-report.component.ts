import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { ContentTypes, UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { ReportTypeEnumLocalization } from 'src/modules/enum-localization.const';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { SchoolYearResource } from 'src/modules/nomenclature/common/services/school-year.resource';
import { ReportType } from '../../enums/report-type.enum';
import { ApplicationReportDto } from '../../models/report/application-report.dto';
import { ApplicationReportSearchFilter } from '../../services/application-report-search.filter';
import { ApplicationReportResource } from '../../services/application-report.resource';

@Component({
	selector: 'app-application-report',
	templateUrl: './application-report.component.html',
	styleUrls: ['./application-report.component.css']
})
export class ApplicationReportComponent implements OnInit {
	report: ApplicationReportDto = new ApplicationReportDto();

	isUniversityUser: boolean = false;
	universityUserInstitution: NomenclatureDto;
	currentSchoolYear: NomenclatureDto = new NomenclatureDto();
	defaultSchoolYear: NomenclatureDto = new NomenclatureDto();
	reportType = ReportType;

	contentTypes = ContentTypes;
	enumLocalization = ReportTypeEnumLocalization;

	schoolYearName: string;
	reportTypeName: string;
	countryName: string;
	nationalityName: string;
	institutionName: string;
	educationalQualificationName: string;
	reportDate: Date;

	constructor(
		private resource: ApplicationReportResource,
		public filter: ApplicationReportSearchFilter,
		private configuration: Configuration,
		private roleService: RoleService,
		private loadingIndicator: LoadingIndicatorService,
		private schoolYearResource: SchoolYearResource
	) {
	}

	ngOnInit(): void {
		this.filter.clear();
		this.filter.reportType = ReportType.defaultReport;

		this.universityUserInstitution = JSON.parse(localStorage.getItem(this.configuration.institutionProperty));
		this.isUniversityUser = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);

		if (this.isUniversityUser) {
			this.filter.institution = this.universityUserInstitution;
			this.filter.institutionId = this.universityUserInstitution.id;
			this.filter.institutionName = this.universityUserInstitution.name;
		} else {
			this.filter.institution = null;
			this.filter.institutionId = undefined;
			this.filter.institutionName = null;
		}

		this.schoolYearResource.getDefaultYears().subscribe((schoolYear: NomenclatureDto) => {
			this.defaultSchoolYear = schoolYear;
			this.currentSchoolYear = schoolYear;
			this.filter.schoolYear = schoolYear;
			this.filter.schoolYearId = schoolYear.id;

			this.search();
		})

	}

	search(): void {
		this.filter.schoolYearName = this.filter.schoolYear?.name;

		this.loadingIndicator.show();
		this.resource.getFiltered(this.filter)
			.pipe(
				finalize(() => this.loadingIndicator.hide())
			)
			.subscribe((report: ApplicationReportDto) => {
				this.schoolYearName = this.filter.schoolYear?.name;
				this.reportTypeName = this.enumLocalization[this.filter.reportType];
				this.nationalityName = this.filter.nationality?.name;
				this.countryName = this.filter.country?.name;
				this.institutionName = this.filter.institution?.name;
				this.educationalQualificationName = this.filter.educationalQualification?.name;
				this.reportDate = new Date();
				this.filter.createdReportDate = this.reportDate;
				this.report = report;
			})
	}

	clearFilter(): void {
		this.filter.clear();
		this.filter.schoolYearId = this.defaultSchoolYear.id;
		this.filter.schoolYear = this.defaultSchoolYear;

		if (this.isUniversityUser) {
			this.filter.institution = this.universityUserInstitution;
			this.filter.institutionId = this.universityUserInstitution.id;
		}

		this.filter.reportType = ReportType.defaultReport;
		this.search();
	}

	onYearChange(schoolYear: NomenclatureDto): void {
		this.currentSchoolYear = schoolYear;
	}

	onReportTypeChange(reportType: ReportType): void {
		this.filter.clear();

		if (reportType !== ReportType.reportByCandidateWithMoreThanOneCertificate) {
			this.filter.schoolYearId = this.currentSchoolYear.id;
			this.filter.schoolYear = this.currentSchoolYear;
		}

		if (this.isUniversityUser) {
			this.filter.institution = this.universityUserInstitution;
			this.filter.institutionId = this.universityUserInstitution.id;
		}
	}
}
