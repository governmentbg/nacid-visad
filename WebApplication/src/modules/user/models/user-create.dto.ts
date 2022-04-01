import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

export class UserDto {
	id?: number
	username: string;
	firstName: string;
	middleName: string;
	lastName: string;
	email: string;
	phone: string;
	roleId: number;
	roleAlias: string;
	position: string;
	institution: NomenclatureDto;
}
