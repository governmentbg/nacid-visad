import { Component, OnInit } from '@angular/core';
import { UnauthorizedUserInfoDto } from '../models/unauthorized-user-info.dto';
import { UserLoginResource } from '../resources/user-login.resource';

@Component({
	selector: 'unauthorized-user-info',
	templateUrl: './unauthorized-user-info.component.html',
	styles: [
		`
		:host {
			display: flex;
    	flex-direction: column;
			text-align: center;
		}
		`
	]
})
export class UnauthorizedUserInfoComponent implements OnInit {
	model = new UnauthorizedUserInfoDto();

	constructor(
		private resource: UserLoginResource
	) { }

	ngOnInit(): void {
		this.resource.getUnauthorizedUserInformation()
			.subscribe((model: UnauthorizedUserInfoDto) => this.model = model);
	}
}
