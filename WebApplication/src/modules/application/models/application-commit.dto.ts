import { CandidateCommitDto } from 'src/modules/candidate/models/candidate-commit.dto';
import { CommitState } from '../../../infrastructure/enums/commit-state.enum';
import { PartDto } from '../../../infrastructure/models/part.dto';
import { ApplicantDto } from './applicant.dto';
import { ApplicationLotResultDto } from './application-lot-result.dto';
import { DiplomaDto } from './diploma.dto';
import { DocumentDto } from './document.dto';
import { EducationDto } from './education.dto';
import { MedicalCertificateDto } from './medical-certificate.to';
import { PreviousApplicationDto } from './previous-application.dto';
import { RepresentativeDto } from './representative.dto';
import { TaxAccountDto } from './tax-account.dto';
import { TrainingDto } from './training.dto';

export class ApplicationCommitDto {
  id: number;
  lotId: number;
  state: CommitState;

  hasResult: boolean;
  lotResult: ApplicationLotResultDto;

  applicantPart: PartDto<ApplicantDto>;

  candidateCommitId: number;
  candidateCommit: CandidateCommitDto;

  educationPart: PartDto<EducationDto>;
  trainingPart: PartDto<TrainingDto>;
  taxAccountPart: PartDto<TaxAccountDto>;
  documentPart: PartDto<DocumentDto>;
  diplomaPart: PartDto<DiplomaDto>;
  representativePart: PartDto<RepresentativeDto>;
  previousApplicationPart: PartDto<PreviousApplicationDto>;
  medicalCertificatePart: PartDto<MedicalCertificateDto>;

  changeStateDescription: string;
  hasOtherCommits: boolean;
}
