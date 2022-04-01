import { Injectable } from '@angular/core';
import { Configuration } from '../configuration/configuration';

@Injectable()
export class RoleService {
	constructor(
		private configuration: Configuration
	) { }

	hasRole(...aliases: string[]): boolean {
		const roleAlias = localStorage.getItem(this.configuration.userRolesProperty);

		let result = false;
		aliases.forEach((alias: string) => result = result || roleAlias === alias);
		return result;
	}
}
