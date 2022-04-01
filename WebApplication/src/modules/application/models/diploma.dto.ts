import { DiplomaFileDto } from './diploma-file.dto';
import { DiplomaDocumentFile } from './diplomas/diploma-document-file';

export class DiplomaDto {
	diplomaFiles: DiplomaFileDto[] = [];

	rectorDecisionDocumentFile: DiplomaDocumentFile;
	nacidRecommendation: DiplomaDocumentFile;

	description: string;

	constructor() {
	}
}
