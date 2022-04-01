import { CandidateCommitHistoryItemDto } from './candidate-commit-history-item.dto';

export class CandidateLotHistoryDto {
  commits: CandidateCommitHistoryItemDto[];
  actualCommitId: number;
}
