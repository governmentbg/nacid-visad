import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { CandidateCommitDto } from 'src/modules/candidate/models/candidate-commit.dto';
import { ApplicantDto } from './applicant.dto';
import { DiplomaDto } from './diploma.dto';
import { DocumentDto } from './document.dto';
import { EducationDto } from './education.dto';
import { MedicalCertificateDto } from './medical-certificate.to';
import { PreviousApplicationDto } from './previous-application.dto';
import { RepresentativeDto } from './representative.dto';
import { TaxAccountDto } from './tax-account.dto';
import { TrainingDto } from './training.dto';

export class ApplicationDto {
  applicant: ApplicantDto;

  candidateCommitId: number;
  candidateCommit: CandidateCommitDto;

  education: EducationDto;
  training: TrainingDto;
  taxAccount: TaxAccountDto;
  document: DocumentDto;
  diploma: DiplomaDto;
  representative: RepresentativeDto;
  previousApplication: PreviousApplicationDto;
  medicalCertificate: MedicalCertificateDto;

  draftId: number | null;
  state: CommitState;

  constructor() {
    this.applicant = new ApplicantDto();
    this.education = new EducationDto();
    this.training = new TrainingDto();
    this.taxAccount = new TaxAccountDto();
    this.document = new DocumentDto();
    this.diploma = new DiplomaDto();
    this.representative = new RepresentativeDto();
    this.previousApplication = new PreviousApplicationDto();
    this.medicalCertificate = new MedicalCertificateDto();
  }
}
