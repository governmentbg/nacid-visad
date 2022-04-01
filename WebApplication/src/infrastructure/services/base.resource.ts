import { HttpClient } from '@angular/common/http';
import { Configuration } from '../configuration/configuration';

export class BaseResource {
  protected baseUrl: string;

  constructor(
    protected http: HttpClient,
    protected configuration: Configuration,
    private suffix?: string
  ) {
    this.baseUrl = `${this.configuration.restUrl}`;
    this.setSuffix(suffix);
  }

  public getBaseUrl(): string {
    return this.baseUrl;
  }

  public setSuffix(suffix?: string): void {
    if (suffix) {
      this.baseUrl = `${this.configuration.restUrl}/${suffix}`;
    }
  }

  public composeQueryString(object: any): string {
    let result = '';
    let isFirst = true;

    if (object) {
      Object.keys(object)
        .filter(key => object[key] !== null && object[key] !== undefined)
        .forEach(key => {
          let value = object[key];
          if (value instanceof Date) {
            value = value.toISOString();
          }

          if (isFirst) {
            result = '?' + key + '=' + value;
            isFirst = false;
          } else {
            result += '&' + key + '=' + value;
          }
        });
    }

    return result;
  }
}
