import { ApplicationCommitHistoryItemDto } from './application-commit-history-item.dto';
import { ApplicationLotResultDto } from './application-lot-result.dto';

export class ApplicationLotHistoryDto {
  hasResult: boolean;
  result: ApplicationLotResultDto;
  commits: ApplicationCommitHistoryItemDto[];
  actualCommitId: number;
}
