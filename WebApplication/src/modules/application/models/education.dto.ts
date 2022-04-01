import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { EducationalQualificationNomenclatureDto } from './educational-qualification-nomenclature-dto';

export class EducationDto {
  speciality: any;
  educationalQualification: EducationalQualificationNomenclatureDto;
  form: NomenclatureDto;
  faculty: NomenclatureDto;
  schoolYear: NomenclatureDto;
  educationSpecialityLanguages: NomenclatureDto[] = [];

  duration: number;

  disableSpecialityLanguage: boolean = false;

  specialization: string;
  traineeDuration: string;
}
