import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class SpecialTrainingDto {
  id: number;

  department: string;

  type: NomenclatureDto;
  duration: number;
  durationType: NomenclatureDto;
}
