import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { DiplomaTypeNomenclatureDto } from './diploma-type-nomenclature.dto';
import { DiplomaDocumentFile } from './diplomas/diploma-document-file';

export class DiplomaFileDto {
	id: number;
	diplomaNumber: string;
	issuedDate: Date | null;
	country: NomenclatureDto;
	city: string;
	type: DiplomaTypeNomenclatureDto;
	organizationName: string;
	diplomaDocumentFile: DiplomaDocumentFile;
	attachedFiles: DiplomaDocumentFile[] = [];

	disabled: boolean = false;
	showNacidRecommendation: boolean = false;
	canEdit: boolean = true;
}
