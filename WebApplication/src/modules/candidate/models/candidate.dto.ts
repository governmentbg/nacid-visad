import { AttachedFile } from 'src/infrastructure/models/attached-file.model';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { CandidatePassportDocumentDto } from './candidatePassportDocument';

export class CandidateDto {
  hasPreviousApplication: boolean;
  previousApplicationRegisterNumber: string;

  passportNumber: string;
  passportValidUntil: Date;

  firstName: string;
  lastName: string;
  otherNames: string;

  birthDate: Date | null;
  birthPlace: string;

  country: NomenclatureDto;
  nationality: NomenclatureDto;
  otherNationalities: NomenclatureDto[] = [];

  imgFile: AttachedFile;

  phone: string;
  mail: string;

  firstNameCyrillic: string;
  lastNameCyrillic: string;
  otherNamesCyrillic: string;

  document: CandidatePassportDocumentDto = new CandidatePassportDocumentDto();
}
