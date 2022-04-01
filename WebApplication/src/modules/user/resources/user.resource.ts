import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { UserStatus } from '../enums/user-status.enum';
import { UserChangePasswordDto } from '../models/user-change-password.dto';
import { UserDto } from '../models/user-create.dto';
import { UserEditDto } from '../models/user-edit.dto';
import { UserSearchResultDto } from '../models/user-search-result.dto';
import { UserSearchFilter } from '../services/user-search-filter.service';

@Injectable({
  providedIn: 'root'
})
export class UserResource extends BaseResource
  implements IFilterable<UserSearchFilter, SearchResultItemDto<UserSearchResultDto>> {

  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'User');
  }

  create(model: UserDto): Observable<number> {
    return this.http.post<number>(this.baseUrl, model);
  }

  getFiltered(filter?: UserSearchFilter): Observable<SearchResultItemDto<UserSearchResultDto>> {
    return this.http.get<SearchResultItemDto<UserSearchResultDto>>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  changePassword(model: UserChangePasswordDto): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/NewPassword`, model);
  }

  getUserDtoById(id: number): Observable<UserEditDto> {
    return this.http.get<UserEditDto>(`${this.baseUrl}/${id}`);
  }

  editUserData(model: UserEditDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}`, model);
  }

  getUserInstitution(institutionName: string): Observable<NomenclatureDto> {
    return this.http.get<NomenclatureDto>(`${this.configuration.restUrl}/Institution/InstitutionByName?institutionName=${institutionName}`);
  }

  changeUserActiveStatus(id: number): Observable<UserStatus> {
    return this.http.put<UserStatus>(`${this.baseUrl}/changeStatus`, id);
  }
}
