import { DiplomaDto } from 'src/modules/application/models/diploma.dto';
import { PreviousApplicationDto } from 'src/modules/application/models/previous-application.dto';

export class CandidateApplicationDataDto {
	diploma: DiplomaDto;
	previousApplication: PreviousApplicationDto
}
