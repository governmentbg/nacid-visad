import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class Configuration {
  clientUrl: string;
  restUrl: string;
  loadingTimeoutInSeconds = 2;

  public readonly tokenProperty = 'access_token';
  public readonly fullnameProperty = 'fullname';
  public readonly userRolesProperty = 'user_roles';
  public readonly institutionProperty = 'institution';

  constructor(
    private httpClient: HttpClient
  ) { }

  load(): Promise<{}> {
    return new Promise((resolve) => {
      this.httpClient.get('./../../../configuration.json')
        .subscribe((e) => {
          this.importSettings(e);
          resolve(true);
        });
    });
  }

  private importSettings(config: any): void {
    if (config.restUrlFromAppOrigin) {
      config.urls.restUrl = `${window.location.origin}/api`;
      config.urls.clientUrl = `${window.location.origin}`;
    }

    this.restUrl = config.urls.restUrl;
    this.clientUrl = config.urls.clientUrl;

    this.loadingTimeoutInSeconds = config.loadingTimeoutInSeconds || 2;
  }
}

// tslint:disable-next-line: ban-types
export function configurationFactory(configuration: Configuration): Function {
  return () => configuration.load();
}
