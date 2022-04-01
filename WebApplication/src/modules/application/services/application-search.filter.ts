import { Injectable } from '@angular/core';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { SearchFilterEndResultType } from '../enums/search-filter-end-result-type.enum';

@Injectable()
export class ApplicationSearchFilter extends BaseSearchFilter {
  registerNumber: string;
  institution: string;
  institutionId: number;
  fromDate: Date;
  toDate: Date;
  speciality: string;
  specialityName: string;
  specialityId: number;
  academicDegree: string;
  academicDegreeId: number;
  faculty: NomenclatureDto;
  facultyId: number;

  endResult: SearchFilterEndResultType | null;

  candidateName: string;
  candidateBirthPlace: string;
  candidateCountry: string;
  candidateCountryId: number;

  constructor() {
    super(10);
    this.endResult = null;
  }
}
