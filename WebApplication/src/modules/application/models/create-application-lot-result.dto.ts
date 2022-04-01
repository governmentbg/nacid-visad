import { AttachedFile } from 'src/infrastructure/models/attached-file.model';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ApplicationLotResultType } from '../enums/application-lot-result-type.enum';

export class CreateApplicationLotResultDto {
  lotId: number;
  type: ApplicationLotResultType;
  file: AttachedFile;
  note: string;
  regulation: NomenclatureDto;

  constructor() {
    this.type = ApplicationLotResultType.certificate;
  }
}
