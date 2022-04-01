import { ApplicationSearchResultItemDto } from 'src/modules/application/models/application-search-result-item.dto';
import { CandidateCommitDto } from './candidate-commit.dto';

export class CandidateApplicationsDto {
	candidateCommit: CandidateCommitDto;
	applications: ApplicationSearchResultItemDto[];
	hasOtherCommits: boolean;
}
