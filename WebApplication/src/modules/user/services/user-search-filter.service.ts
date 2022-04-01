import { Injectable } from '@angular/core';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';
import { UserStatus } from '../enums/user-status.enum';

@Injectable({
  providedIn: 'root'
})
export class UserSearchFilter extends BaseSearchFilter {
  firstName: string;
  middleName: string;
  lastName: string;
  username: string;
  email: string;

  roleId: number | null;
  role: string;
  status: UserStatus | null;

  institution: string;
  institutionId: number;

  constructor() {
    super(10);
    this.status = null;
  }
}
