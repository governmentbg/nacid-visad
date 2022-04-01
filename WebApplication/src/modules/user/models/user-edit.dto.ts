import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { UserStatus } from '../enums/user-status.enum';
import { Role } from './role.dto';

export class UserEditDto {
	id: number | null;
	phone: string;
	firstName: string;
	middleName: string;
	lastName: string;
	email: string;
	role: Role = new Role();
	institution: NomenclatureDto;
	position: string;
	status: UserStatus;
	institutionId: number;
}
