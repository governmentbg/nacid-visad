import { CommitState } from '../../../infrastructure/enums/commit-state.enum';
import { ApplicationLotResultType } from '../enums/application-lot-result-type.enum';

export class ApplicationCommitHistoryItemDto {
  commitId: number;
  lotId: number;

  state: CommitState;
  changeStateDescription: string;
  registerNumber: string;
  applicationLotResultType: ApplicationLotResultType;

  applicantName: string;
  candidateName: string;
  candidateBirthDate: Date;
  candidateCountry: string;

  number: number;
  createDate: Date;
}
