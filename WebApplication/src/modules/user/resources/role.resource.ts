import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { Role } from '../models/role.dto';

@Injectable({
  providedIn: 'root'
})
export class RoleResource {

  constructor(private http: HttpClient, private configuration: Configuration) { }

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.configuration.restUrl}/role`);
  }
}
