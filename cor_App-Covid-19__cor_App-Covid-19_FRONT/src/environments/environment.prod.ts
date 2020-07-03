import { mergeDeepRight } from 'ramda';
import { environment as _default } from './environment.default';

export const environment = mergeDeepRight(_default, {
  production: true,
  auth: {
    authority: 'https://foo-login.bar/tenant-UUID',
    clientID: '3d2b2f4b-3b7e-41a5-b09d-0d5bcff87bc7',
    redirectUri: 'https://foo-front.bar',
    protectedResourceMap: [
      [
        '//foo-webapi.bar',
        ['api://c482b0ab-eb5f-42e7-8826-d79343358d4e/Read', 'api://c482b0ab-eb5f-42e7-8826-d79343358d4e/Write']
      ],
      [
        '//foo-idservice.bar',
        ['api://c482b0ab-eb5f-42e7-8826-d79343358d4e/Read', 'api://c482b0ab-eb5f-42e7-8826-d79343358d4e/Write']
      ]
    ],
    scopes: ['api://c482b0ab-eb5f-42e7-8826-d79343358d4e/Read', 'api://c482b0ab-eb5f-42e7-8826-d79343358d4e/Write']
  },
  prefixes: {
    default: 'https://foo-webapi.bar/api',
    identity: 'https://foo-idservice.bar/api'
  }
});
