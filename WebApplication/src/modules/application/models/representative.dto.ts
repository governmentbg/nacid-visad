import { RepresentativeType } from '../enums/representative-type.enum';
import { RepresentativeDocumentFile } from './representative-document-file.dto';

export class RepresentativeDto {
  hasRepresentative: boolean = false;
  type: RepresentativeType = RepresentativeType.individual;
  firstName: string;
  lastName: string;
  identificationCode: string;
  mail: string;
  phone: string;
  note: string;

  applicationForCertificate: RepresentativeDocumentFile;
  letterOfAttorney: RepresentativeDocumentFile;
  submissionDate: Date;
}
