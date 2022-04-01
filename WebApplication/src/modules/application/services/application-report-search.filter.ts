import { Injectable } from '@angular/core';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ReportType } from '../enums/report-type.enum';

@Injectable()
export class ApplicationReportSearchFilter extends BaseSearchFilter {
	reportType: ReportType;
	createdReportDate: Date;

	institutionId: number;
	institution: NomenclatureDto;
	institutionName: string;

	schoolYearId: number;
	schoolYear: NomenclatureDto;
	schoolYearName: string;

	educationalQualificationId: number;
	educationalQualification: NomenclatureDto;
	educationalQualificationName: string;

	countryId: number;
	country: NomenclatureDto;
	countryName: string;

	nationalityId: number;
	nationality: NomenclatureDto;
	nationalityName: string;

	constructor() {
		super(0);

		this.reportType = ReportType.defaultReport;
	}
}
