import { AttachedFile } from 'src/infrastructure/models/attached-file.model';

export class FileTemplate extends AttachedFile {
	id: number;
	alias: string;
	description: string;
	isActive: boolean;

	isEditMode: boolean;
	originalObject: FileTemplate;
}
