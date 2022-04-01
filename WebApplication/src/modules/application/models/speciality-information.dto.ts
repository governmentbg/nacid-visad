import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class SpecialityInformationDto {
	id: number;
	specialityId: number;
	name: string;
	duration: number;
	qualification: NomenclatureDto;
	form: NomenclatureDto;
	specialityLanguage: NomenclatureDto[] = [];
}
