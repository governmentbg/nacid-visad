import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class CandidateSearchResultItemDto {
  lotId: number;
  commitId: number;
  // state: CommitState
  name: string;
  nationality: string;
  birthPlace: string;
  birthDate: Date;
  cyrillicName: string;
  mail: string;
  phone: string;
  applicationsCount: number;
  otherNationalities: NomenclatureDto[] = [];
  country: string;
}
