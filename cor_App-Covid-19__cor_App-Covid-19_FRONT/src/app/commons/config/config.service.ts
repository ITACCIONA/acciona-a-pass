import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

export interface EnvironmentConfiguration {
  environment: { [key: string]: any };
  currentENV: string;
}

@Injectable()
export class ConfigService {
  private env: any;

  constructor() {
    this.env = environment;
  }

  get(key: string, interpolateParams?: object) {
    const keyArray = key.split('.');
    let value = this.env;
    for (const subkey of keyArray) {
      value = value[subkey];
      if (value === undefined) {
        return undefined;
      }
    }

    if (typeof value === 'string' && interpolateParams) {
      value = this._interpolateString(value, interpolateParams);
    }
    return value;
  }

  private _interpolateString(value: string, interpolateParams: object) {
    return value.replace(
      /\{([^}]+)\}/g,
      (dummy, v) => +(interpolateParams[v] !== 0) && (interpolateParams[v] || dummy)
    );
  }

  getUrl(key: string, interpolateParams?: object, prefix?: string | false) {
    const host = this.get('host');
    const prefixStr = prefix === false ? '' : this.get(prefix ? `prefixes.${prefix}` : 'prefixes.default') || '';
    const url = this.get(`url.${key}`, interpolateParams);
    return url ? `${host}${prefixStr}${url}` : undefined;
  }
}
