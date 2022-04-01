import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ApplicationLotResultType } from '../enums/application-lot-result-type.enum';

export class ApplicationSearchResultItemDto {
  lotId: number;
  commitId: number;
  registerNumber: string;

  hasResult: boolean;
  resultType: ApplicationLotResultType | null;
  isSigned: boolean;
  filePdfUrl: string;

  candidateName: string;
  candidateNationality: string;
  candidateCyrillicName: string;
  otherNationalities: NomenclatureDto[] = [];
  birthDate: Date | null;
  candidateCountry: string;

  organizationName: string;
  speciality: string;
  educationalQualification: string;
  form: string;
  representative: string;
  specialization: string;

  applicantName: string;
  applicantMail: string;
  applicantPhone: string;
  mail: string;
}
