import { Injectable } from '@angular/core';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';

@Injectable()
export class CandidateSearchFilter extends BaseSearchFilter {
  name: string;
  countryId: number | null;
  country: string;
  birthPlace: string;
  birthDate: Date;
  birthDateFrom: Date;
  birthDateTo: Date;
  nationalityId: number | null;
  nationality: string;
  passportNumber: string;
  mail: string;
  phone: string;
  // institutionId: number;
  // institution: string;

  constructor() {
    super(10);
  }
}
