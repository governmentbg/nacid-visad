import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { PartDto } from '../models/part.dto';

@Injectable()
export class PartResource extends BaseResource {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration);
  }

  updatePartEntity(partName: string, partId: number, entity: any): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${partName}/${partId}/entity`, entity);
  }

  startPartModification(partName: string, partId: number): Observable<PartDto<any>> {
    return this.http.post<PartDto<any>>(`${this.baseUrl}/${partName}/${partId}/startmodification`, null);
  }

  cancelPartModification(partName: string, partId: number): Observable<PartDto<any>> {
    return this.http.post<PartDto<any>>(`${this.baseUrl}/${partName}/${partId}/cancelmodification`, null);
  }
}
