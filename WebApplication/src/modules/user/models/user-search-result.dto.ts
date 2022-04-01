import { UserStatus } from '../enums/user-status.enum';

export class UserSearchResultDto {
	id: number | null;
	username: string;
	firstName: string;
	middleName: string;
	lastName: string;
	email: string;
	phone: string;
	role: string;
	institutionName: string;
	status: UserStatus;
}
