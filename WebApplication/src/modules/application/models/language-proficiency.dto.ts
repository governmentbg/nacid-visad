import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class LanguageProficiencyDto {
  id: number;
  language: NomenclatureDto;
  reading: NomenclatureDto;
  writing: NomenclatureDto;
  speaking: NomenclatureDto;
  disable: boolean = false;
  required: boolean = false;
}
