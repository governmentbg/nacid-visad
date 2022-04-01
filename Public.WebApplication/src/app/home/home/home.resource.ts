import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Configuration } from "src/infrastructure/configuration/configuration";

@Injectable({
  providedIn: 'root'
})
export class HomeResource {

  constructor(private http: HttpClient, private configuration: Configuration) { }

  getPdfUrl(accessCode: string, captchaToken: string): Observable<string> {
    return this.http.get<string>(`${this.configuration.restUrl}/pdf?accessCode=${accessCode}&captcha=${captchaToken}`);
  }

}