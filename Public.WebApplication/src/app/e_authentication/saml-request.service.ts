import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { SamlRequest } from './saml-request.model';

@Injectable({
  providedIn: 'root'
})
export class SamlRequestService {
  private baseUrl: string;
  private headers = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private httpService: HttpClient,
    private config: Configuration) {

    this.baseUrl = this.config.clientUrl + '/api/ems/EAuthentication/';
  }

  getSamlRequest(serviceType: string, alias: string): Observable<SamlRequest> {
    return this.httpService.get<SamlRequest>(this.baseUrl + 'EAuth?serviceType=' + serviceType + '&alias=' + alias
      , { headers: this.headers });
  }
}
