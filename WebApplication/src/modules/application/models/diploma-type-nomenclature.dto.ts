import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class DiplomaTypeNomenclatureDto extends NomenclatureDto {
	IsRUOVerificationRequired: boolean | null;
	alias: string;
}
