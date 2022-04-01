import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class ApplicantDto {
  institution: NomenclatureDto;

  firstName: string;
  middleName: string;
  lastName: string;

  position: string;

  phone: string;
  mail: string;
}
