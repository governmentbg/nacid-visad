import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class UserLoginInfoDto {
	token: string;
	fullname: string;
	institution: NomenclatureDto;
	roleAlias: string;
}
