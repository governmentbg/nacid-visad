import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { CreatorUserDto } from './creator-user.dto';

export class CandidateCommitHistoryItemDto {
  id: number;
  lotId: number;

  state: CommitState;

  candidateName: string;

  number: number;
  createDate: Date;
  country: string;
  birthDate: Date;
  candidateCyrillicName: string;
  mail: string;
  creatorUser: CreatorUserDto;
  changeStateDescription: string;
}
