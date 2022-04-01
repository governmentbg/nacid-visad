import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { PartDto } from 'src/infrastructure/models/part.dto';
import { CandidateDto } from './candidate.dto';

export class CandidateCommitDto {
  id: number;
  lotId: number;
  state: CommitState;

  candidatePart: PartDto<CandidateDto>;
  changeStateDescription: string;
}
