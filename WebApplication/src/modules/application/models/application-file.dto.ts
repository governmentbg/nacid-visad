import { AttachedFile } from 'src/infrastructure/models/attached-file.model';
import { ApplicationFileTypeNomenclatureDto } from './application-file-nomenclature.dto';
export class ApplicationFileDto {
  id: number;
  type: ApplicationFileTypeNomenclatureDto;
  attachedFile: AttachedFile;
  fileDescription: string;
}
