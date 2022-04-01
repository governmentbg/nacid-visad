import { AttachedFile } from 'src/infrastructure/models/attached-file.model';

export class RepresentativeDocumentFile extends AttachedFile {
	id: number;
	representativeId: number;
}
