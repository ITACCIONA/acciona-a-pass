import { ConfigService } from '@commons';

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

export function MSALConfigFactory(config: ConfigService) {
  return {
    auth: {
      clientId: config.get('auth.clientID'),
      authority: config.get('auth.authority'),
      validateAuthority: true,
      redirectUri: config.get('auth.redirectUri'),
      postLogoutRedirectUri: config.get('auth.redirectUri'),
      navigateToLoginRequestUrl: true
    },
    cache: {
      cacheLocation: 'sessionStorage',
      storeAuthStateInCookie: isIE // set to true for IE 11
    }
  };
}

export function MSALConfigAngularFactory(config: ConfigService) {
  return {
    popUp: false, // !isIE,
    consentScopes: config.get('auth.scopes'),
    extraQueryParameters: {},
    unprotectedResources: [],
    protectedResourceMap: new Map(config.get('auth.protectedResourceMap'))
  };
}
