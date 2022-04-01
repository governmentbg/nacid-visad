import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class ApplicationFileTypeNomenclatureDto extends NomenclatureDto {
	hasDate: boolean | null;
	description: string;
}
