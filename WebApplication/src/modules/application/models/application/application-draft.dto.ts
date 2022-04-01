import { CommitState } from 'src/infrastructure/enums/commit-state.enum';

export class ApplicationDraftDto {
	id: number | null;
	content: string;
	state: CommitState;
}
